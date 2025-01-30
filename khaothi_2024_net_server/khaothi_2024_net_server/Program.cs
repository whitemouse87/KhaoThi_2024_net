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

    #region Cấu Hình Ứng Dụng
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
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddDistributedMemoryCache();

        ConfigureCors(builder);
        ConfigureAuthentication(builder);
        ConfigureRateLimiting(builder);
        ConfigureDapper();
        ConfigureDependencies(builder);
        ConfigureSwagger(builder);
    }

    private static void ConfigureCors(WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options => options.AddPolicy("ChoDomainDuocPhep", policy =>
            policy
                .WithOrigins(
                    "https://localhost:44386",  // Origin chính của Blazor WebAssembly
                    "http://localhost:44386",   // Hỗ trợ cả HTTP
                    "https://localhost:5168",   // Origin của API (nếu cần thiết)
                    "http://localhost:5168"     // Hỗ trợ cả HTTP cho API
                )
                .SetIsOriginAllowedToAllowWildcardSubdomains()
                .AllowAnyMethod()              // Cho phép tất cả các phương thức HTTP
                .AllowAnyHeader()              // Cho phép tất cả các header
                .AllowCredentials()            // Cho phép gửi credentials
                .WithExposedHeaders("Content-Disposition", "File-Name") // Thêm nếu cần
        ));
    }

    private static void ConfigureAuthentication(WebApplicationBuilder builder)
    {
        var jwtConfig = builder.Configuration.GetSection("Jwt");
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtConfig["SecretKey"])),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("YeuCauAdmin", policy =>
                policy.RequireClaim("MaChucVu", "1"));
            options.AddPolicy("YeuCauNguoiDung", policy =>
                policy.RequireAuthenticatedUser());
        });
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
            .AddScoped<IAuthService, AuthService>();
    }

    private static void ConfigureSwagger(WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "API Khảo Thí",
                Version = "v1",
                Contact = new OpenApiContact
                {
                    Name = "Hỗ trợ",
                    Email = "hotro@khaothi.com"
                }
            });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using Bearer scheme",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
        });
    }

    private static void ConfigureMiddleware(WebApplication app)
    {
        ConfigureSecurityHeaders(app);
        ConfigureTokenBlacklist(app);
        ConfigureDevelopment(app);
        ConfigureStandardMiddleware(app);
        ConfigureGlobalExceptionHandler(app);

        app.MapControllers();
    }
    #endregion

    #region Cấu Hình Middleware
    private static void ConfigureSecurityHeaders(WebApplication app)
    {
        app.Use(async (context, next) =>
        {
            context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
            context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
            context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
            context.Response.Headers.Add("Referrer-Policy", "strict-origin-when-cross-origin");

            // Điều chỉnh CSP để cho phép kết nối từ client Blazor
            context.Response.Headers.Add(
                "Content-Security-Policy",
                "default-src 'self'; " +
                "script-src 'self' 'unsafe-inline' 'unsafe-eval'; " +
                "style-src 'self' 'unsafe-inline'; " +
                "img-src 'self' data: https:; " +
                "font-src 'self' data:; " +
                "connect-src 'self' ws: wss: http://localhost:* https://localhost:* " +
                $"https://localhost:44386 http://localhost:44386;");

            await next();
        });
    }

    private static void ConfigureTokenBlacklist(WebApplication app)
    {
        app.Use(async (context, next) =>
        {
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
            {
                var token = authHeader["Bearer ".Length..];
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
                                await context.Response.WriteAsJsonAsync(new ErrorResponse
                                {
                                    Message = "Token đã bị thu hồi"
                                });
                                return;
                            }
                        }
                    }
                }
                catch (SecurityTokenException)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsJsonAsync(new ErrorResponse
                    {
                        Message = "Token không hợp lệ"
                    });
                    return;
                }
            }
            await next();
        });
    }

    private static void ConfigureDevelopment(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Khảo Thí v1"));
        }
        else
        {
            app.UseHsts();
            app.UseHttpsRedirection();
        }
    }

    private static void ConfigureStandardMiddleware(WebApplication app)
    {
        app.UseRouting();

        // CORS phải được đặt trước Authentication và Authorization
        app.UseCors("ChoDomainDuocPhep");

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseRateLimiter();
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

                await context.Response.WriteAsJsonAsync(new ErrorResponse
                {
                    Message = "Đã xảy ra lỗi hệ thống"
                });
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
                context.HttpContext.Response.Headers["Retry-After"] =
                    thoiGianCho.TotalSeconds.ToString();
            }

            return ValueTask.CompletedTask;
        };
}
#endregion