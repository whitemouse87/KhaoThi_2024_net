//using khaothi_2024_net_server.Core.Interfaces;
//using khaothi_2024_net_server.Features.Authentication;
//using khaothi_2024_net_server.Infrastructure.Data;
//using khaothi_2024_net_server.Infrastructure.Repositories;
//using khaothi_2024_net_server.Infrastructure.TypeHandlers;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.RateLimiting;
//using Microsoft.Extensions.Caching.Distributed;
//using Microsoft.IdentityModel.Tokens;
//using Microsoft.OpenApi.Models;
//using Serilog;
//using System.IdentityModel.Tokens.Jwt;
//using System.Reflection;
//using System.Text;
//using System.Threading.RateLimiting;
//using DotSwashbuckle.AspNetCore.Swagger;
//using DotSwashbuckle.AspNetCore.SwaggerGen;
//using DotSwashbuckle.AspNetCore.SwaggerUI;
//using Microsoft.AspNetCore.Mvc;
//using khaothi_2024_net_server.Features.UserManagement;

//namespace khaothi_2024_net_server;

//public class Program
//{
//    public static void Main(string[] args)
//    {
//        var builder = WebApplication.CreateBuilder(args);

//        ConfigureLogging(builder);
//        ConfigureServices(builder);

//        var app = builder.Build();
//        ConfigureMiddleware(app);

//        app.Run();
//    }

//    #region Cấu Hình Services
//    private static void ConfigureLogging(WebApplicationBuilder builder)
//    {
//        builder.Host.UseSerilog((context, services, configuration) => configuration
//            .ReadFrom.Configuration(context.Configuration)
//            .ReadFrom.Services(services)
//            .Enrich.FromLogContext()
//            .WriteTo.Console()
//            .WriteTo.File("logs/log-.txt",
//                rollingInterval: RollingInterval.Day,
//                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}"));
//    }

//    private static void ConfigureServices(WebApplicationBuilder builder)
//    {
//        builder.Services.AddControllers();
//        builder.Services.AddEndpointsApiExplorer();

//        // Thêm cấu hình cho IIS
//        builder.Services.Configure<IISServerOptions>(options =>
//        {
//            options.AutomaticAuthentication = false;
//            options.MaxRequestBodySize = int.MaxValue;
//        });

//        // Thêm cấu hình cho IIS Integration
//        builder.Services.Configure<IISOptions>(options =>
//        {
//            options.ForwardClientCertificate = false;
//        });

//        builder.Services.AddHttpContextAccessor();
//        builder.Services.AddDistributedMemoryCache();

//        ConfigureCors(builder);
//        ConfigureAuthentication(builder);
//        ConfigureRateLimiting(builder);
//        ConfigureDapper();
//        ConfigureDependencies(builder);
//        ConfigureSwagger(builder);
//    }

//    private static void ConfigureCors(WebApplicationBuilder builder)
//    {
//        builder.Services.AddCors(options =>
//        {
//            options.AddPolicy("AllowAll",
//                policy => policy
//                    .AllowAnyOrigin()
//                    .AllowAnyMethod()
//                    .AllowAnyHeader());

//            options.AddPolicy("ChoDomainDuocPhep", policy =>
//                policy
//                    .WithOrigins(
//                        "https://localhost:44386",
//                        "http://localhost:44386",
//                        "https://localhost:5168",
//                        "http://localhost:5168",
//                        "https://localhost:7168"
//                    )
//                    .SetIsOriginAllowedToAllowWildcardSubdomains()
//                    .AllowAnyMethod()
//                    .AllowAnyHeader()
//                    .AllowCredentials()
//                    .WithExposedHeaders("Content-Disposition", "File-Name")
//            );
//        });
//    }

//    private static void ConfigureAuthentication(WebApplicationBuilder builder)
//    {
//        var jwtConfig = builder.Configuration.GetSection("Jwt");
//        var secretKey = jwtConfig["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey is not configured");

//        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//            .AddJwtBearer(options =>
//            {
//                options.TokenValidationParameters = new TokenValidationParameters
//                {
//                    ValidateIssuerSigningKey = true,
//                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
//                    ValidateIssuer = false,
//                    ValidateAudience = false,
//                    ValidateLifetime = true,
//                    ClockSkew = TimeSpan.Zero
//                };
//            });

//        builder.Services.AddAuthorization();
//    }

//    private static void ConfigureRateLimiting(WebApplicationBuilder builder)
//    {
//        builder.Services.AddRateLimiter(options => options
//            .AddPolicy<string, TuyChinhRateLimiter>("ChinhSachGioiHan"));
//    }

//    private static void ConfigureDapper()
//    {
//        Dapper.SqlMapper.AddTypeHandler(new DateTimeHandler());
//        Dapper.SqlMapper.AddTypeHandler(new NullableDateTimeHandler());
//    }

//    private static void ConfigureDependencies(WebApplicationBuilder builder)
//    {
//        builder.Services
//            .AddSingleton<IDataAccessLayer, MyDataAccessLayer>()
//        .AddScoped<IKhaoThiUserRepository, KhaoThiUserRepository>()
//        .AddScoped<IKhaoThiUserService, KhaoThiUserService>()  // Thêm dòng này
//        .AddScoped<IAuthService, AuthService>();

//    }

//    private static void ConfigureSwagger(WebApplicationBuilder builder)
//    {
//        builder.Services.AddEndpointsApiExplorer();
//        builder.Services.AddSwaggerGen(options =>
//        {
//            options.SwaggerDoc("v1", new OpenApiInfo
//            {
//                Version = "2.0",
//                Title = "API Khảo Thí",
//                Description = "API quản lý hệ thống khảo thí",
//                Contact = new OpenApiContact
//                {
//                    Name = "Support Team",
//                    Email = "support@example.com"
//                },
//                License = new OpenApiLicense
//                {
//                    Name = "MIT License",
//                    Url = new Uri("https://opensource.org/licenses/MIT")
//                }
//            });

//            // Cấu hình JWT Authentication
//            var securityScheme = new OpenApiSecurityScheme
//            {
//                Name = "Authorization",
//                Description = "JWT Authorization header using Bearer scheme. Example: 'Bearer {token}'",
//                Type = SecuritySchemeType.Http,
//                Scheme = "bearer",
//                BearerFormat = "JWT",
//                In = ParameterLocation.Header,
//                Reference = new OpenApiReference
//                {
//                    Type = ReferenceType.SecurityScheme,
//                    Id = "Bearer"
//                }
//            };

//            options.AddSecurityDefinition("Bearer", securityScheme);
//            options.AddSecurityRequirement(new OpenApiSecurityRequirement
//        {
//            { securityScheme, Array.Empty<string>() }
//        });

//            // Cấu hình XML Comments
//            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
//            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
//            if (File.Exists(xmlPath))
//            {
//                options.IncludeXmlComments(xmlPath);
//            }

//            // Tối ưu hóa hiển thị
//            options.EnableAnnotations();
//            options.DescribeAllParametersInCamelCase();
//            options.UseInlineDefinitionsForEnums();
//            options.CustomSchemaIds(type => type.FullName);

//            // Nhóm API theo tags
//            options.TagActionsBy(api =>
//            {
//                if (api.GroupName != null)
//                {
//                    return new[] { api.GroupName };
//                }

//                var controllerName = api.ActionDescriptor.RouteValues["controller"];
//                return new[] { controllerName };
//            });

//            options.DocInclusionPredicate((docName, api) => true);
//        });
//    }
//    #endregion

//    #region Cấu Hình Middleware
//    private static void ConfigureMiddleware(WebApplication app)
//    {
//        if (app.Environment.IsDevelopment())
//        {
//            app.UseDeveloperExceptionPage();
//        }
//        else
//        {
//            app.UseExceptionHandler("/Error");
//            app.UseHsts();
//        }

//        // Swagger Configuration
//        app.UseSwagger(options =>
//        {
//            options.RouteTemplate = "swagger/{documentName}/swagger.json";
//            options.SerializeAsV2 = true;

//            // Thêm server URLs
//            options.PreSerializeFilters.Add((swagger, httpReq) =>
//            {
//                swagger.Servers = new List<OpenApiServer>
//            {
//                new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}" }
//            };
//            });
//        });

//        app.UseSwaggerUI(options =>
//        {
//            options.SwaggerEndpoint("/swagger/v1/swagger.json", "API Khảo Thí v1");
//            options.RoutePrefix = "swagger";
//            options.DocumentTitle = "API Documentation - Khảo Thí";
//            options.DocExpansion(DocExpansion.None);
//            options.DefaultModelsExpandDepth(-1);
//            options.DisplayRequestDuration();
//            options.EnableDeepLinking();
//            options.EnableFilter();
//            options.EnableValidator();
//            options.DisplayOperationId();
//        });

//        app.UseHttpsRedirection();
//        app.UseStaticFiles(); // Thêm cho IIS
//        app.UseRouting();

//        // Cấu hình CORS
//        if (app.Environment.IsDevelopment())
//        {
//            app.UseCors("ChoDomainDuocPhep");
//        }
//        else
//        {
//            app.UseCors("AllowAll");
//            app.UseHsts();
//        }

//        // Security và Authentication
//        ConfigureSecurityHeaders(app);
//        app.UseAuthentication();
//        app.UseAuthorization();
//        app.UseRateLimiter();

//        // Custom Middleware
//        ConfigureTokenBlacklist(app);
//        ConfigureGlobalExceptionHandler(app);

//        //app.MapControllers().RequireAuthorization();
//        app.MapControllers();
//    }

//    private static void ConfigureSecurityHeaders(WebApplication app)
//    {
//        app.Use(async (context, next) =>
//        {
//            var csp = "default-src 'self';" +
//                "script-src 'self' 'unsafe-inline' 'unsafe-eval';" +
//                "style-src 'self' 'unsafe-inline';" +
//                "img-src 'self' data:;" +
//                "font-src 'self' data:;" +
//                "connect-src 'self' *;" +
//                "frame-ancestors 'self';" +   // Thêm frame-ancestors
//                "base-uri 'self';";           // Thêm base-uri

//            context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
//            context.Response.Headers.Append("X-Frame-Options", "SAMEORIGIN");
//            context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
//            context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");
//            context.Response.Headers.Append("Content-Security-Policy", csp);

//            await next();
//        });
//    }

//    private static void ConfigureTokenBlacklist(WebApplication app)
//    {
//        app.Use(async (context, next) =>
//        {
//            var authHeader = context.Request.Headers.Authorization.FirstOrDefault();
//            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
//            {
//                var token = authHeader["Bearer ".Length..].Trim();
//                try
//                {
//                    var tokenHandler = new JwtSecurityTokenHandler();
//                    if (tokenHandler.CanReadToken(token))
//                    {
//                        var jwtToken = tokenHandler.ReadJwtToken(token);
//                        var jti = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;

//                        if (!string.IsNullOrEmpty(jti))
//                        {
//                            var cache = context.RequestServices.GetRequiredService<IDistributedCache>();
//                            var isRevoked = await cache.GetStringAsync($"blacklist_{jti}");
//                            if (!string.IsNullOrEmpty(isRevoked))
//                            {
//                                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
//                                await context.Response.WriteAsJsonAsync(new { message = "Token đã bị thu hồi" });
//                                return;
//                            }
//                        }
//                    }
//                }
//                catch (SecurityTokenException)
//                {
//                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
//                    await context.Response.WriteAsJsonAsync(new { message = "Token không hợp lệ" });
//                    return;
//                }
//            }
//            await next();
//        });
//    }

//    private static void ConfigureGlobalExceptionHandler(WebApplication app)
//    {
//        app.Use(async (context, next) =>
//        {
//            try
//            {
//                await next();
//            }
//            catch (Exception ex)
//            {
//                app.Logger.LogError(ex, "Lỗi không xử lý được: {Message}", ex.Message);

//                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
//                context.Response.ContentType = "application/json";
//                await context.Response.WriteAsJsonAsync(new { message = "Đã xảy ra lỗi hệ thống" });
//            }
//        });
//    }
//    #endregion
//}

//#region Các lớp bổ trợ
//public class TuyChinhRateLimiter : IRateLimiterPolicy<string>
//{
//    private readonly ILogger<TuyChinhRateLimiter> _logger;

//    public TuyChinhRateLimiter(ILogger<TuyChinhRateLimiter> logger)
//    {
//        _logger = logger;
//    }

//    public RateLimitPartition<string> GetPartition(HttpContext context)
//    {
//        var diaChiIP = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

//        return RateLimitPartition.GetSlidingWindowLimiter(
//            partitionKey: diaChiIP,
//            factory: _ => new SlidingWindowRateLimiterOptions
//            {
//                AutoReplenishment = true,
//                PermitLimit = 150,
//                QueueLimit = 20,
//                Window = TimeSpan.FromMinutes(2),
//                SegmentsPerWindow = 8
//            });
//    }

//    public Func<OnRejectedContext, CancellationToken, ValueTask>? OnRejected =>
//        (context, token) =>
//        {
//            _logger.LogWarning(
//                "Vượt quá giới hạn truy cập cho IP {DiaChiIP}",
//                context.HttpContext.Connection.RemoteIpAddress);

//            context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;

//            if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var thoiGianCho))
//            {
//                context.HttpContext.Response.Headers.RetryAfter =
//                    ((int)thoiGianCho.TotalSeconds).ToString();
//            }

//            return ValueTask.CompletedTask;
//        };
//}
//#endregion thêm ConfigureKestrel vào đâu


using khaothi_2024_net_server.Core.Interfaces;
using khaothi_2024_net_server.Features.Authentication;
using khaothi_2024_net_server.Infrastructure.Data;
using khaothi_2024_net_server.Infrastructure.Repositories;
using khaothi_2024_net_server.Infrastructure.TypeHandlers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;
using System.Threading.RateLimiting;
using DotSwashbuckle.AspNetCore.Swagger;
using DotSwashbuckle.AspNetCore.SwaggerGen;
using DotSwashbuckle.AspNetCore.SwaggerUI;
using Microsoft.AspNetCore.Mvc;
using khaothi_2024_net_server.Features.UserManagement;

namespace khaothi_2024_net_server;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ConfigureLogging(builder);
        ConfigureServices(builder);

        var app = builder.Build();
        ConfigureMiddleware(app);

        app.Run();
    }

    #region Cấu Hình Services
    private static void ConfigureLogging(WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, services, configuration) => configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File("logs/log-.txt",
                rollingInterval: RollingInterval.Day,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}"));
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        //ConfigureKestrel(builder);

        // Thêm cấu hình cho IIS
        builder.Services.Configure<IISServerOptions>(options =>
        {
            options.AutomaticAuthentication = false;
            options.MaxRequestBodySize = int.MaxValue;
        });

        // Thêm cấu hình cho IIS Integration
        builder.Services.Configure<IISOptions>(options =>
        {
            options.ForwardClientCertificate = false;
        });

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddDistributedMemoryCache();

        ConfigureCors(builder);
        ConfigureAuthentication(builder);
        ConfigureRateLimiting(builder);
        ConfigureDapper();
        ConfigureDependencies(builder);
        ConfigureSwagger(builder);
    }
    //private static void ConfigureKestrel(WebApplicationBuilder builder)
    //{
    //    builder.WebHost.ConfigureKestrel(options =>
    //    {
    //        // 🛠️ Cấu hình kích thước tối đa của request body (mặc định là 30MB, ở đây là 50MB)
    //        options.Limits.MaxRequestBodySize = 50 * 1024 * 1024; // 50MB

    //        // 🛠️ Tăng số lượng kết nối đồng thời
    //        options.Limits.MaxConcurrentConnections = 1000;

    //        //// 🛠️ Giới hạn tốc độ đọc/ghi (Rate Limit)
    //        //options.Limits.MaxRequestBufferSize = 1024 * 1024; // 1MB buffer
    //        //options.Limits.MaxResponseBufferSize = 1024 * 1024; // 1MB buffer

    //        //// 🛠️ Hỗ trợ HTTP/2
    //        //options.ListenAnyIP(5000, listenOptions =>
    //        //{
    //        //    listenOptions.UseHttps(); // Kích hoạt HTTPS (nếu cần)
    //        //    listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
    //        //});

    //        //options.ListenAnyIP(5001, listenOptions =>
    //        //{
    //        //    listenOptions.UseHttps(); // Chạy trên HTTPS
    //        //    listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1AndHttp2;
    //        //});

    //        //// 🛠️ Định nghĩa số lượng request tối đa mà server có thể xử lý đồng thời
            
    //    });
    //}

    private static void ConfigureCors(WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                policy => policy
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());

            options.AddPolicy("ChoDomainDuocPhep", policy =>
                policy
                    .WithOrigins(
                        "https://localhost:44386",
                        "http://localhost:44386",
                        "https://localhost:5168",
                        "http://localhost:5168",
                        "https://localhost:7168"
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
        var jwtConfig = builder.Configuration.GetSection("Jwt");
        var secretKey = jwtConfig["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey is not configured");

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
                    ClockSkew = TimeSpan.Zero
                };
            });

        builder.Services.AddAuthorization();
    }

    private static void ConfigureRateLimiting(WebApplicationBuilder builder)
    {
        builder.Services.AddRateLimiter(options => options
            .AddPolicy<string, TuyChinhRateLimiter>("ChinhSachGioiHan"));
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
        .AddScoped<IKhaoThiUserService, KhaoThiUserService>()  // Thêm dòng này
        .AddScoped<IAuthService, AuthService>();

    }

    private static void ConfigureSwagger(WebApplicationBuilder builder)
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

            // Cấu hình JWT Authentication
            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "JWT Authorization header using Bearer scheme. Example: 'Bearer {token}'",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            };

            options.AddSecurityDefinition("Bearer", securityScheme);
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            { securityScheme, Array.Empty<string>() }
        });

            // Cấu hình XML Comments
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
            if (File.Exists(xmlPath))
            {
                options.IncludeXmlComments(xmlPath);
            }

            // Tối ưu hóa hiển thị
            options.EnableAnnotations();
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
    }
    #endregion

    #region Cấu Hình Middleware
    private static void ConfigureMiddleware(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        // Swagger Configuration
        app.UseSwagger(options =>
        {
            options.RouteTemplate = "swagger/{documentName}/swagger.json";
            options.SerializeAsV2 = true;

            // Thêm server URLs
            options.PreSerializeFilters.Add((swagger, httpReq) =>
            {
                swagger.Servers = new List<OpenApiServer>
            {
                new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}" }
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
        });

        app.UseHttpsRedirection();
        app.UseStaticFiles(); // Thêm cho IIS
        app.UseRouting();

        // Cấu hình CORS
        if (app.Environment.IsDevelopment())
        {
            app.UseCors("ChoDomainDuocPhep");
        }
        else
        {
            app.UseCors("AllowAll");
            app.UseHsts();
        }

        // Security và Authentication
        ConfigureSecurityHeaders(app);
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseRateLimiter();

        // Custom Middleware
        ConfigureTokenBlacklist(app);
        ConfigureGlobalExceptionHandler(app);

        //app.MapControllers().RequireAuthorization();
        app.MapControllers();
    }

    private static void ConfigureSecurityHeaders(WebApplication app)
    {
        app.Use(async (context, next) =>
        {
            var csp = "default-src 'self';" +
                "script-src 'self' 'unsafe-inline' 'unsafe-eval';" +
                "style-src 'self' 'unsafe-inline';" +
                "img-src 'self' data:;" +
                "font-src 'self' data:;" +
                "connect-src 'self' *;" +
                "frame-ancestors 'self';" +   // Thêm frame-ancestors
                "base-uri 'self';";           // Thêm base-uri

            context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
            context.Response.Headers.Append("X-Frame-Options", "SAMEORIGIN");
            context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
            context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");
            context.Response.Headers.Append("Content-Security-Policy", csp);

            await next();
        });
    }

    private static void ConfigureTokenBlacklist(WebApplication app)
    {
        app.Use(async (context, next) =>
        {
            var authHeader = context.Request.Headers.Authorization.FirstOrDefault();
            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
            {
                var token = authHeader["Bearer ".Length..].Trim();
                try
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    if (tokenHandler.CanReadToken(token))
                    {
                        var jwtToken = tokenHandler.ReadJwtToken(token);
                        var jti = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;

                        if (!string.IsNullOrEmpty(jti))
                        {
                            var cache = context.RequestServices.GetRequiredService<IDistributedCache>();
                            var isRevoked = await cache.GetStringAsync($"blacklist_{jti}");
                            if (!string.IsNullOrEmpty(isRevoked))
                            {
                                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                                await context.Response.WriteAsJsonAsync(new { message = "Token đã bị thu hồi" });
                                return;
                            }
                        }
                    }
                }
                catch (SecurityTokenException)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsJsonAsync(new { message = "Token không hợp lệ" });
                    return;
                }
            }
            await next();
        });
    }

    private static void ConfigureGlobalExceptionHandler(WebApplication app)
    {
        app.Use(async (context, next) =>
        {
            try
            {
                await next();
            }
            catch (Exception ex)
            {
                app.Logger.LogError(ex, "Lỗi không xử lý được: {Message}", ex.Message);

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new { message = "Đã xảy ra lỗi hệ thống" });
            }
        });
    }
    #endregion
}

#region Các lớp bổ trợ
public class TuyChinhRateLimiter : IRateLimiterPolicy<string>
{
    private readonly ILogger<TuyChinhRateLimiter> _logger;

    public TuyChinhRateLimiter(ILogger<TuyChinhRateLimiter> logger)
    {
        _logger = logger;
    }

    public RateLimitPartition<string> GetPartition(HttpContext context)
    {
        var diaChiIP = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        return RateLimitPartition.GetSlidingWindowLimiter(
            partitionKey: diaChiIP,
            factory: _ => new SlidingWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 150,
                QueueLimit = 20,
                Window = TimeSpan.FromMinutes(2),
                SegmentsPerWindow = 8
            });
    }

    public Func<OnRejectedContext, CancellationToken, ValueTask>? OnRejected =>
        (context, token) =>
        {
            _logger.LogWarning(
                "Vượt quá giới hạn truy cập cho IP {DiaChiIP}",
                context.HttpContext.Connection.RemoteIpAddress);

            context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;

            if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var thoiGianCho))
            {
                context.HttpContext.Response.Headers.RetryAfter =
                    ((int)thoiGianCho.TotalSeconds).ToString();
            }

            return ValueTask.CompletedTask;
        };
}
#endregion