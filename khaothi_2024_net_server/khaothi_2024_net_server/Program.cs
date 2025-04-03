using DotSwashbuckle.AspNetCore.SwaggerGen;
using DotSwashbuckle.AspNetCore.SwaggerUI;
using khaothi_2024_net_server.Core.Interfaces;
using khaothi_2024_net_server.Features.Authentication;
using khaothi_2024_net_server.Features.BaoCaoSoLieuConThiTHPT;
using khaothi_2024_net_server.Features.BaoCaoSoLieuConThiTHPT.Interfaces;
using khaothi_2024_net_server.Features.BaoCaoSoLieuLanhDaoTHPT;
using khaothi_2024_net_server.Features.BaoCaoSoLieuLanhDaoTHPT.Interfaces;
using khaothi_2024_net_server.Features.BaoCaoSoLieuThiTHPT;
using khaothi_2024_net_server.Features.BaoCaoSoLieuThiTHPT.Interfaces;
using khaothi_2024_net_server.Features.BaoCaoSoLieuThiTHPT_HS12_NhomMon;
using khaothi_2024_net_server.Features.BaoCaoSoLieuThiTHPT_HS12_NhomMon.Interfaces;
using khaothi_2024_net_server.Features.BaoCaoSoLieuTruongDiemTHPT;
using khaothi_2024_net_server.Features.BaoCaoSoLieuTruongDiemTHPT.Interfaces;
using khaothi_2024_net_server.Features.Shares;
using khaothi_2024_net_server.Features.UserManagement;
using khaothi_2024_net_server.Infrastructure.Data;
using khaothi_2024_net_server.Infrastructure.Repositories;
using khaothi_2024_net_server.Infrastructure.Security;
using khaothi_2024_net_server.Infrastructure.TypeHandlers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Threading.RateLimiting;

namespace khaothi_2024_net_server;

public class Program
{
    public static void Main(string[] args)
    {
        //var builder = WebApplication.CreateBuilder(args);
        //var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
        //Console.WriteLine($"Running in {environment} environment");

        //ConfigureLogging(builder);
        //ConfigureServices(builder);

        //var app = builder.Build();
        //ConfigureMiddleware(app);
        //app.MapBankEndpoints();
        //app.MapTruongEndpoints();
        //app.MapQuanEndpoints();


        //app.Run();



        var builder = WebApplication.CreateBuilder(args);

        // Thêm cấu hình từ web.config
        builder.Configuration.AddXmlFile("web.config", optional: true, reloadOnChange: true);

        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
        Console.WriteLine($"Running in {environment} environment");

        ConfigureLogging(builder);
        ConfigureServices(builder);

        var app = builder.Build();
        ConfigureMiddleware(app);
        app.MapBankEndpoints();
        app.MapTruongEndpoints();
        app.MapQuanEndpoints();

        app.Run();
    }

    #region Cấu Hình Services
    private static void ConfigureLogging(WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
            .WriteTo.File("logs/log-.txt",
                rollingInterval: RollingInterval.Day,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();

        builder.Host.UseSerilog();
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        if (builder.Environment.IsProduction())
        {
            var webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            if (!Directory.Exists(webRootPath))
            {
                Directory.CreateDirectory(webRootPath);
            }
            builder.WebHost.UseWebRoot(webRootPath);
        }
        // Controllers và JSON options
        builder.Services.AddControllers()
      .AddJsonOptions(options =>
      {
          options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
          options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
          options.JsonSerializerOptions.PropertyNamingPolicy = null;
          options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
          options.JsonSerializerOptions.WriteIndented = true;
          options.JsonSerializerOptions.TypeInfoResolver = new DefaultJsonTypeInfoResolver();
      });

        builder.Services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.PropertyNamingPolicy = null;
            options.SerializerOptions.WriteIndented = false;
            options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
            options.SerializerOptions.TypeInfoResolver = new DefaultJsonTypeInfoResolver();
        });

        builder.Services.AddEndpointsApiExplorer();

        // IIS Configurations
        builder.Services.Configure<IISServerOptions>(options =>
        {
            options.AutomaticAuthentication = false;
            options.MaxRequestBodySize = int.MaxValue;
        });

        builder.Services.Configure<IISOptions>(options =>
        {
            options.ForwardClientCertificate = false;
        });

        // Core Services
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddMemoryCache();
        builder.Services.AddDistributedMemoryCache();

        ConfigureSwagger(builder);
        ConfigureCors(builder);
        ConfigureAuthentication(builder);
        ConfigureRateLimiting(builder);
        ConfigureDapper();
        ConfigureDependencies(builder);
        // THÊM ĐOẠN MÃ NÀY Ở ĐÂY
        var dataProtectionPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "khaothi_2024_net_server", "DataProtectionKeys");
        Directory.CreateDirectory(dataProtectionPath);

        builder.Services.AddDataProtection()
            .PersistKeysToFileSystem(new DirectoryInfo(dataProtectionPath))
            .SetApplicationName("khaothi_2024_net_server");
    }

    private static void ConfigureSwagger(WebApplicationBuilder builder)
    {
        try
        {
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "2.0",
                    Title = "API Khảo Thí",
                    Description = "API quản lý hệ thống khảo thí",
                    Contact = new OpenApiContact
                    {
                        Name = "Support Team",
                        Email = "support@example.com"
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT License",
                        Url = new Uri("https://opensource.org/licenses/MIT")
                    }
                });

                // Cấu hình JWT Bearer Authentication cho Swagger UI
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Nhập Bearer token theo định dạng: Bearer {your_token}\r\n\r\nVí dụ: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
                });

                // Thêm requirement bảo mật
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });

                // Cải thiện xử lý enum - thêm SchemaFilter mới
                options.SchemaFilter<EnumSchemaFilter>();

                // Tối ưu hóa hiển thị
                options.DescribeAllParametersInCamelCase();
                options.UseInlineDefinitionsForEnums();
                options.CustomSchemaIds(type => type.FullName);

                // Nhóm API theo tags
                options.TagActionsBy(api =>
                {
                    if (api.GroupName != null)
                    {
                        return new[] { api.GroupName };
                    }
                    var controllerName = api.ActionDescriptor.RouteValues["controller"];
                    return new[] { controllerName };
                });

                options.DocInclusionPredicate((docName, api) => true);
            });

            // Thêm SchemaFilter để xử lý enum tốt hơn
            builder.Services.AddSingleton<EnumSchemaFilter>();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    // Thêm class EnumSchemaFilter
    public class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type.IsEnum)
            {
                // Xóa các giá trị enum hiện có (số)
                schema.Enum.Clear();

                // Thêm lại dưới dạng string
                foreach (var name in Enum.GetNames(context.Type))
                {
                    schema.Enum.Add(new OpenApiString(name));
                }

                // Đặt kiểu dữ liệu là string
                schema.Type = "string";
                schema.Format = null;
            }
        }
    }




    private static void ConfigureCors(WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
                policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());

            options.AddPolicy("AllowedOrigins", policy =>
                policy.WithOrigins(
                    "https://localhost:44386",
                    "http://localhost:44386",
                    "https://localhost:5168",
                    "http://localhost:5168",
                    "https://localhost:7168",
                    "https://thongtinkhaothihcm.com",
                    "http://localhost:5000",
                    "https://localhost:5000",
                    "https://api.thongtinkhaothihcm.com"
                )
                .SetIsOriginAllowedToAllowWildcardSubdomains()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .WithExposedHeaders("Content-Disposition", "File-Name")
            );
        });
    }

    private static void ConfigureAuthentication(WebApplicationBuilder builder)
    {
        // Thêm logging để debug
        // Console.WriteLine("Starting ConfigureAuthentication");

        // Đoạn này gây lỗi - thay thế bằng đoạn mới
        // Console.WriteLine($"Configuration sources count: {builder.Configuration.Providers.Count()}");

        var jwtConfig = builder.Configuration.GetSection("Jwt");
        // Console.WriteLine($"JWT section exists: {jwtConfig.Exists()}");

        // Thay thế đoạn liệt kê providers gây lỗi bằng đoạn này
        try
        {
            // Console.WriteLine("All configuration keys:");
            foreach (var key in builder.Configuration.AsEnumerable())
            {
                //Console.WriteLine($"Key: {key.Key}, Value: {(key.Key.Contains("Secret") ? "[HIDDEN]" : key.Value)}");
            }
        }
        catch (Exception ex)
        {
            //Console.WriteLine($"Error enumerating config: {ex.Message}");
        }

        // Thử đọc trực tiếp từ nhiều định dạng khác nhau
        var secretKey = jwtConfig["SecretKey"];
        //Console.WriteLine($"SecretKey from jwtConfig: {(secretKey != null ? "Found" : "Not found")}");

        if (string.IsNullOrEmpty(secretKey))
        {
            secretKey = builder.Configuration["Jwt:SecretKey"];
            //Console.WriteLine($"SecretKey from direct access: {(secretKey != null ? "Found" : "Not found")}");
        }

        if (string.IsNullOrEmpty(secretKey))
        {
            // Thử đọc trực tiếp từ file
            try
            {
                var basePath = AppContext.BaseDirectory;
                var appSettingsPath = Path.Combine(basePath, "appsettings.json");
                //Console.WriteLine($"Looking for appsettings.json at: {appSettingsPath}");

                if (File.Exists(appSettingsPath))
                {
                    var json = File.ReadAllText(appSettingsPath);
                    //Console.WriteLine("Successfully read appsettings.json");

                    using (var doc = System.Text.Json.JsonDocument.Parse(json))
                    {
                        if (doc.RootElement.TryGetProperty("Jwt", out var jwtElement) &&
                            jwtElement.TryGetProperty("SecretKey", out var secretKeyElement))
                        {
                            secretKey = secretKeyElement.GetString();
                            //Console.WriteLine("Found SecretKey in direct file read");
                        }
                    }
                }
                else
                {
                    //Console.WriteLine("appsettings.json not found at expected location");
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"Error reading appsettings.json directly: {ex.Message}");
            }
        }

        // Cuối cùng, nếu vẫn không tìm thấy, gán một giá trị cứng
        //if (string.IsNullOrEmpty(secretKey))
        //{
        //    // Trong môi trường production, bạn nên xử lý tốt hơn
        //   // Console.WriteLine("CRITICAL: Using hardcoded key as fallback - not recommended for production!");
        //    secretKey = "AZOaiEIWCDlNSUO6IeHmigFPpZ7afCLH-H08LZpFbzc"; // Sử dụng key từ appsettings.json
        //}

        // Thiết lập authentication
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    RoleClaimType = "MaChucVu"
                };
            });

        builder.Services.AddAuthorization();
        // Console.WriteLine("ConfigureAuthentication completed successfully");
    }

    private static void ConfigureRateLimiting(WebApplicationBuilder builder)
    {
        builder.Services.AddRateLimiter(options =>
        {
            options.AddPolicy("StandardRateLimit", context =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "anonymous",
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        AutoReplenishment = true,
                        PermitLimit = 100,
                        QueueLimit = 0,
                        Window = TimeSpan.FromMinutes(1)
                    }));
        });
    }

    private static void ConfigureDapper()
    {
        Dapper.SqlMapper.AddTypeHandler(new DateTimeHandler());
        Dapper.SqlMapper.AddTypeHandler(new NullableDateTimeHandler());
    }

    private static void ConfigureDependencies(WebApplicationBuilder builder)
    {
        builder.Services
            .AddSingleton<IDataAccessLayer, MyDataAccessLayer>()
            .AddScoped<IKhaoThiUserRepository, KhaoThiUserRepository>()
            .AddScoped<IKhaoThiUserService, KhaoThiUserService>()
            .AddScoped<IBCThongTinDonViRepository, BCThongTinDonViRepository>()
            .AddScoped<IBCThongTinDonViService, BCThongTinDonViService>()
            .AddScoped<IBCThongTinNhomMonRepository, BCThongTinNhomMonRepository>()
            .AddScoped<IBCThongTinNhomMonService, BCThongTinNhomMonService>()
            .AddScoped<IBCLanhDaoDonViRepository, BCLanhDaoDonViRepository>()
            .AddScoped<IBCLanhDaoDonViService, BCLanhDaoDonViService>()
            .AddScoped<IBCThongTinTruongDiemRepository, BCThongTinTruongDiemRepository>()
            .AddScoped<IBCThongTinTruongDiemService, BCThongTinTruongDiemService>()
            .AddScoped<IBCThongTinConThiRepository, BCThongTinConThiRepository>()
            .AddScoped<IBCThongTinConThiService, BCThongTinConThiService>()
            .AddScoped<IAuthService, AuthService>()
            .AddScoped<IPasswordHasher, BCryptPasswordHasher>();
        builder.Services.Configure<PasswordOptions>(options =>
        {
            options.RequiredLength = 6;
            options.RequireNonAlphanumeric = true;
            options.RequireDigit = true;
            options.RequireLowercase = true;
            options.RequireUppercase = true;
        });
    }
    #endregion

    #region Cấu Hình Middleware
    private static void ConfigureMiddleware(WebApplication app)
    {
        //app.Use(async (context, next) =>
        //{
        //    // Log path để debug
        //    Log.Information("Request Path: {Path}, Method: {Method}", context.Request.Path, context.Request.Method);

        //    // Xử lý OPTIONS request
        //    if (context.Request.Method == "OPTIONS")
        //    {
        //        context.Response.Headers.Append("Access-Control-Allow-Origin", "*");
        //        context.Response.Headers.Append("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
        //        context.Response.Headers.Append("Access-Control-Allow-Headers", "Content-Type, Authorization, X-Requested-With");
        //        context.Response.StatusCode = 200;
        //        await context.Response.CompleteAsync();
        //        return;
        //    }

        //    await next();

        //    // Log status code sau khi xử lý
        //    Log.Information("Response Status: {StatusCode} for {Path}", context.Response.StatusCode, context.Request.Path);
        //});
        // Development specific middleware
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }
        app.UseCors("AllowedOrigins");

        // Swagger Configuration
        app.UseSwagger(options =>
        {
            options.RouteTemplate = "swagger/{documentName}/swagger.json";
            options.SerializeAsV2 = true;

            // Thêm server URLs với mô tả
            options.PreSerializeFilters.Add((swagger, httpReq) =>
            {
                swagger.Servers = new List<OpenApiServer>
                {
                new OpenApiServer
                {
                    Url = $"{httpReq.Scheme}://{httpReq.Host.Value}",
                    Description = "Current Environment Server"
                }
                };
            });
        });

        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "API Khảo Thí v1");
            options.RoutePrefix = "swagger";
            options.DocumentTitle = "API Documentation - Khảo Thí";
            options.DocExpansion(DocExpansion.None);
            options.DefaultModelsExpandDepth(-1);
            options.DisplayRequestDuration();
            options.EnableDeepLinking();
            options.EnableFilter();
            options.EnableValidator();
            options.DisplayOperationId();

            // Thêm các tùy chỉnh mới
            options.ConfigObject.AdditionalItems["syntaxHighlight"] = new Dictionary<string, object>
            {
                ["activated"] = true,
                ["theme"] = "monokai"
            };

            // Enable try it out feature cho tất cả endpoints
            options.ConfigObject.AdditionalItems["tryItOutEnabled"] = true;
        });

        // Security Headers
        app.Use(async (context, next) =>
        {
            context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
            context.Response.Headers.Append("X-Frame-Options", "SAMEORIGIN");
            context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
            context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");
            await next();
        });

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseRateLimiter();


        // JWT Token Blacklist Check
        app.Use(async (context, next) =>
        {
            var authHeader = context.Request.Headers.Authorization.FirstOrDefault();
            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
            {
                var token = authHeader["Bearer ".Length..].Trim();
                var cache = context.RequestServices.GetRequiredService<IDistributedCache>();
                var isRevoked = await cache.GetStringAsync($"blacklist_{token}");

                if (!string.IsNullOrEmpty(isRevoked))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsJsonAsync(new { message = "Token đã bị thu hồi" });
                    return;
                }
            }
            await next();
        });

        // Global Exception Handler
        // Global Exception Handler cần ghi log chi tiết hơn
        app.UseExceptionHandler(new ExceptionHandlerOptions
        {
            AllowStatusCode404Response = true,
            ExceptionHandler = async context =>
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

                // Tạo chuỗi JSON thủ công để tránh serialization 
                var message = exception?.Message ?? "Unknown error";
                var path = context.Request.Path.ToString();

                var jsonString = @"{""message"":""Đã xảy ra lỗi hệ thống"",""detail"":""" +
                    message.Replace("\"", "\\\"") + @""",""path"":""" +
                    path.Replace("\"", "\\\"") + @"""}";

                await context.Response.WriteAsync(jsonString);
            }
        });

        app.MapControllers();
    }
    #endregion
}