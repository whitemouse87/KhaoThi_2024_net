using DotSwashbuckle.AspNetCore.SwaggerUI;
using khaothi_2024_net_server.Core.Interfaces;
using khaothi_2024_net_server.Features.Authentication;
using khaothi_2024_net_server.Features.UserManagement;
using khaothi_2024_net_server.Infrastructure.Data;
using khaothi_2024_net_server.Infrastructure.Repositories;
using khaothi_2024_net_server.Infrastructure.Security;
using khaothi_2024_net_server.Infrastructure.TypeHandlers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;

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
        // Controllers và JSON options
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
                options.JsonSerializerOptions.WriteIndented = false;
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


            // Cấu hình JWT Authentication với mô tả chi tiết hơn
            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = @"JWT Bearer Token. 
                        Nhập theo định dạng: 'Bearer {your_token}'
                        Ví dụ: Bearer eyJhbGciOiJIUzI1NiIs...",
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
                    ClockSkew = TimeSpan.Zero,
                    RoleClaimType = "MaChucVu"
                };
            });

        builder.Services.AddAuthorization();
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
        // Development specific middleware
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseCors("AllowedOrigins");
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
            app.UseCors("AllowAll");
        }

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
        app.Use(async (context, next) =>
        {
            try
            {
                await next();
            }
            catch (Exception ex)
            {
                var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "Unhandled exception");

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new { message = "Đã xảy ra lỗi hệ thống" });
            }
        });

        app.MapControllers();
    }
    #endregion
}