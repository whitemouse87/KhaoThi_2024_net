


//using Blazored.LocalStorage;
//using Fluxor;
//using KhaoThi_2024_net_client;
//using KhaoThi_2024_net_client.Components.Auth;
//using KhaoThi_2024_net_client.Middleware;
//using KhaoThi_2024_net_client.Services.Auth;
//using KhaoThi_2024_net_client.Services.Logging;
//using Microsoft.AspNetCore.Components.Authorization;
//using Microsoft.AspNetCore.Components.Web;
//using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
//using Microsoft.AspNetCore.Authorization;
//using Serilog;
//using Fluxor.Blazor.Web.ReduxDevTools;
//using Microsoft.AspNetCore.Routing;
//using Serilog.Events;
//using System.Security.Claims;
//using MudBlazor;
//using MudBlazor.Services;
//using KhaoThi_2024_net_client.Components;

//using MudBlazor.Extensions;

//public class Program
//{
//    private const string API_BASE_URL = "http://localhost:5168/api/";

//    public static async Task Main(string[] args)
//    {
//        var builder = WebAssemblyHostBuilder.CreateDefault(args);


//        // Root Components Registration
//        ConfigureRootComponents(builder);

//        // Configure Services
//        ConfigureServices(builder.Services, builder.Configuration);

//        // Configure Logging
//        ConfigureLogging();

//        try
//        {
//            Log.Information("===== KHỞI ĐỘNG ỨNG DỤNG =====");
//            var app = builder.Build();

//            // Initialize Logger Service
//            var loggingService = app.Services.GetRequiredService<ILoggingService>();
//            Logger.Initialize(loggingService);

//            await app.RunAsync();
//        }
//        catch (Exception ex)
//        {
//            Log.Error(ex, "LỖI KHỞI ĐỘNG: {Message}", ex.Message);
//            throw;
//        }
//        finally
//        {
//            Log.CloseAndFlush();
//        }
//    }

//    private static void ConfigureRootComponents(WebAssemblyHostBuilder builder)
//    {
//        builder.RootComponents.Add<App>("#app");
//        builder.RootComponents.Add<HeadOutlet>("head::after");
//    }

//    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
//    {
//        // HTTP Client
//        ConfigureHttpClient(services);

//        // Authentication & Authorization
//        ConfigureAuth(services);

//        // State Management & Storage
//        ConfigureStateManagement(services);

//        // Application Services
//        ConfigureApplicationServices(services);

//        // Route Configuration
//        ConfigureRouting(services);

//        services.AddScoped<CircularProgress>();
//    }

//    private static void ConfigureHttpClient(IServiceCollection services)
//    {
//        services.AddScoped(sp => new HttpClient
//        {
//            BaseAddress = new Uri(API_BASE_URL),
//            Timeout = TimeSpan.FromSeconds(30)
//        });
//    }

//    private static void ConfigureAuth(IServiceCollection services)
//    {
//        services.AddAuthorizationCore(options =>
//        {
//            // Default policy yêu cầu xác thực
//            var defaultPolicy = new AuthorizationPolicyBuilder()
//                .RequireAuthenticatedUser()
//                .Build();
//            options.DefaultPolicy = defaultPolicy;

//            // Policy cho các trang công khai
//            options.AddPolicy("AllowAnonymous", policy =>
//            {
//                policy.RequireAssertion(_ => true);
//            });

//            // Policy cho Admin
//            options.AddPolicy("RequireAdmin", policy =>
//                policy.RequireAssertion(context =>
//                {
//                    var maChucVu = context.User.FindFirst("maChucVu")?.Value;
//                    return maChucVu == "01";
//                }));
//        });

//        // Authentication State Provider với Singleton pattern
//        services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>(provider =>
//        {
//            var localStorage = provider.GetRequiredService<ILocalStorageService>();
//            var authService = provider.GetRequiredService<IAuthService>();
//            var loggingService = provider.GetRequiredService<ILoggingService>();
//            return new CustomAuthStateProvider(localStorage, authService, loggingService);
//        });
//    }

//    private static void ConfigureStateManagement(IServiceCollection services)
//    {
//        // Local Storage
//        services.AddBlazoredLocalStorage();

//        // Fluxor State Management
//        services.AddFluxor(options =>
//        {
//            options.ScanAssemblies(typeof(Program).Assembly);
//            options.AddMiddleware<FluxorLoggingMiddleware>();
//#if DEBUG
//            options.UseReduxDevTools();
//#endif
//        });
//    }

//    private static void ConfigureApplicationServices(IServiceCollection services)
//    {
//        services.AddScoped<IAuthService, AuthService>();
//        services.AddScoped<ILoggingService, LoggingService>();

//        // Thêm các service khác ở đây
//        services.AddMudServices();
//        // Thêm MudBlazor Extensions
//        services.AddMudExtensions();

//        // Hoặc thêm với cấu hình tùy chỉnh
//        //services.AddMudExtensions(config =>
//        //{
//        //    config.EnableRipple = true;   // Enable ripple effect
//        //    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
//        //    // ... các cấu hình khác
//        //});
//    }

//    private static void ConfigureRouting(IServiceCollection services)
//    {
//        services.Configure<RouteOptions>(options =>
//        {
//            options.LowercaseUrls = true;
//        });
//    }

//    private static void ConfigureLogging()
//    {
//        var logConfig = new LoggerConfiguration()
//            .MinimumLevel.Debug()
//            .WriteTo.BrowserConsole(
//                restrictedToMinimumLevel: LogEventLevel.Information,
//                outputTemplate: "[{Level}] {Message}{Exception}"
//            );

//#if DEBUG
//        logConfig.WriteTo.Debug();
//#endif

//        Log.Logger = logConfig.CreateLogger();
//    }
//}

using Blazored.LocalStorage;
using Fluxor;
using Fluxor.Blazor.Web.ReduxDevTools;
using KhaoThi_2024_net_client;
using KhaoThi_2024_net_client.Components;
using KhaoThi_2024_net_client.Components.Auth;
using KhaoThi_2024_net_client.Middleware;
using KhaoThi_2024_net_client.Services.Auth;
using KhaoThi_2024_net_client.Services.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Routing;
using MudBlazor;
using MudBlazor.Extensions;
using MudBlazor.Services;
using Serilog;
using Serilog.Events;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;

public class Program
{
    private const string API_BASE_URL = "https://localhost:7168/api/";
    private const int HTTP_TIMEOUT_SECONDS = 30;

    public static async Task Main(string[] args)
    {
        try
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            ConfigureApp(builder);

            Log.Information("===== KHỞI ĐỘNG ỨNG DỤNG =====");
            var app = builder.Build();

            // Initialize Logger Service
            var loggingService = app.Services.GetRequiredService<ILoggingService>();
            Logger.Initialize(loggingService);

            await app.RunAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "LỖI KHỞI ĐỘNG: {Message}", ex.Message);
            throw;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static void ConfigureApp(WebAssemblyHostBuilder builder)
    {
        ConfigureRootComponents(builder);
        ConfigureServices(builder.Services, builder.Configuration);
        ConfigureLogging();
    }

    private static void ConfigureRootComponents(WebAssemblyHostBuilder builder)
    {
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        ConfigureHttpClient(services);
        ConfigureAuth(services);
        ConfigureStateManagement(services);
        ConfigureMudBlazor(services);
        ConfigureApplicationServices(services);
        ConfigureRouting(services);

        // Add WebAssembly specific services
        services.AddScoped<CircularProgress>();
        services.AddScoped<IWebAssemblyHostEnvironment>(sp =>
            sp.GetRequiredService<IWebAssemblyHostEnvironment>());
    }

    private static void ConfigureHttpClient(IServiceCollection services)
    {
        services.AddScoped(sp => new HttpClient
        {
            BaseAddress = new Uri(API_BASE_URL),
            Timeout = TimeSpan.FromSeconds(HTTP_TIMEOUT_SECONDS)
        });
    }

    private static void ConfigureAuth(IServiceCollection services)
    {
        services.AddAuthorizationCore(options =>
        {
            ConfigureAuthorizationPolicies(options);
        });

        services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>(provider =>
            CreateCustomAuthStateProvider(provider));
    }

    private static void ConfigureAuthorizationPolicies(AuthorizationOptions options)
    {
        // Default policy yêu cầu xác thực
        var defaultPolicy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();
        options.DefaultPolicy = defaultPolicy;

        // Policy cho các trang công khai
        options.AddPolicy("AllowAnonymous", policy =>
            policy.RequireAssertion(_ => true));

        // Policy cho Admin
        options.AddPolicy("RequireAdmin", policy =>
            policy.RequireAssertion(context =>
                IsUserAdmin(context)));
    }

    private static bool IsUserAdmin(AuthorizationHandlerContext context)
    {
        var maChucVu = context.User.FindFirst("maChucVu")?.Value;
        return maChucVu == "01";
    }

    private static CustomAuthStateProvider CreateCustomAuthStateProvider(IServiceProvider provider)
    {
        var localStorage = provider.GetRequiredService<ILocalStorageService>();
        var authService = provider.GetRequiredService<IAuthService>();
        var loggingService = provider.GetRequiredService<ILoggingService>();
        return new CustomAuthStateProvider(localStorage, authService, loggingService);
    }

    private static void ConfigureStateManagement(IServiceCollection services)
    {
        services.AddBlazoredLocalStorage();
        services.AddFluxor(options =>
        {
            options.ScanAssemblies(typeof(Program).Assembly);
            options.AddMiddleware<FluxorLoggingMiddleware>();
#if DEBUG
            
            options.UseReduxDevTools(options =>
            {
                // Cấu hình thêm nếu cần
                options.Name = "KhaoThi_2024"; // Tên của ứng dụng
                //options.trac(); // Hiển thị stack trace
                //options.EnableStackTrace();
            });
#endif
        });
    }

    private static void ConfigureMudBlazor(IServiceCollection services)
    {
        services.AddMudServices(config =>
        {
            config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
            config.SnackbarConfiguration.PreventDuplicates = false;
            config.SnackbarConfiguration.NewestOnTop = true;
            config.SnackbarConfiguration.ShowCloseIcon = true;
            config.SnackbarConfiguration.VisibleStateDuration = 5000;
            config.SnackbarConfiguration.HideTransitionDuration = 500;
            config.SnackbarConfiguration.ShowTransitionDuration = 500;
            config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
        });

        services.AddMudMarkdownServices();
        services.AddMudExtensions();
    }

    private static void ConfigureApplicationServices(IServiceCollection services)
    {
        // Core Services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ILoggingService, LoggingService>();

        // Add other application services here
        ConfigureAdditionalServices(services);
    }

    private static void ConfigureAdditionalServices(IServiceCollection services)
    {
        // Add any additional application-specific services here
        // Example: services.AddScoped<IMyService, MyService>();
    }

    private static void ConfigureRouting(IServiceCollection services)
    {
        services.Configure<RouteOptions>(options =>
        {
            options.LowercaseUrls = true;
         
            options.AppendTrailingSlash = false;
        });
    }

    private static void ConfigureLogging()
    {
        var logConfig = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.BrowserConsole(
                restrictedToMinimumLevel: LogEventLevel.Information,
                outputTemplate: "[{Level}] {Message}{Exception}"
            );

#if DEBUG
        logConfig.WriteTo.Debug();
#endif

        Log.Logger = logConfig.CreateLogger();
    }
}