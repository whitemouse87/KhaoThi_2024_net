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

//var builder = WebAssemblyHostBuilder.CreateDefault(args);

//// Root Components Registration
//builder.RootComponents.Add<App>("#app");
//builder.RootComponents.Add<HeadOutlet>("head::after");

//// Configure Services
//ConfigureServices(builder.Services, builder.Configuration);

//// Configure Logging
//ConfigureLogging();

//try
//{
//    Log.Information("===== KHỞI ĐỘNG ỨNG DỤNG =====");
//    var app = builder.Build();

//    // Initialize Logger Service
//    var loggingService = app.Services.GetRequiredService<ILoggingService>();
//    Logger.Initialize(loggingService);

//    await app.RunAsync();
//}
//catch (Exception ex)
//{
//    Log.Error(ex, "LỖI KHỞI ĐỘNG: {Message}", ex.Message);
//    throw;
//}
//finally
//{
//    Log.CloseAndFlush();
//}

//static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
//{
//    // HTTP Client Configuration
//    services.AddScoped(sp => new HttpClient
//    {
//        BaseAddress = new Uri("http://localhost:5168/api/"),
//        Timeout = TimeSpan.FromSeconds(30)
//    });

//    // Security Services
//    services.AddAuthorizationCore(options =>
//    {
//        // Default policy yêu cầu xác thực
//        options.DefaultPolicy = new AuthorizationPolicyBuilder()
//            .RequireAuthenticatedUser()
//            .Build();

//        // Policy cho các trang công khai
//        options.AddPolicy("AllowAnonymous", policy =>
//        {
//            policy.RequireAssertion(_ => true);
//        });

//        // Có thể thêm các policy khác cho từng role
//        options.AddPolicy("RequireAdmin", policy =>
//        policy.RequireAssertion(context =>
//            context.User.FindFirst("maChucVu")?.Value == Constants.Roles.Admin));
//    });

//    // Authentication State Provider
//    services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

//    // Local Storage
//    services.AddBlazoredLocalStorage();

//    // Application Services
//    services.AddScoped<IAuthService, AuthService>();
//    services.AddScoped<ILoggingService, LoggingService>();

//    // Route Configuration
//    services.Configure<RouteOptions>(options =>
//    {
//        options.LowercaseUrls = true;
//    });

//    // Fluxor State Management
//    services.AddFluxor(options =>
//    {
//        options.ScanAssemblies(typeof(Program).Assembly);
//        options.AddMiddleware<FluxorLoggingMiddleware>();
//#if DEBUG
//        options.UseReduxDevTools();
//#endif
//    });
//}

//static void ConfigureLogging()
//{
//    Log.Logger = new LoggerConfiguration()
//      .MinimumLevel.Debug()
//      .WriteTo.BrowserConsole(
//          restrictedToMinimumLevel: LogEventLevel.Information,
//          outputTemplate: "[{Level}] {Message}{Exception}"
//      )
//#if DEBUG
//      .WriteTo.Debug()
//#endif
//      .CreateLogger();
//}


using Blazored.LocalStorage;
using Fluxor;
using KhaoThi_2024_net_client;
using KhaoThi_2024_net_client.Components.Auth;
using KhaoThi_2024_net_client.Middleware;
using KhaoThi_2024_net_client.Services.Auth;
using KhaoThi_2024_net_client.Services.Logging;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Authorization;
using Serilog;
using Fluxor.Blazor.Web.ReduxDevTools;
using Microsoft.AspNetCore.Routing;
using Serilog.Events;
using System.Security.Claims;

public class Program
{
    private const string API_BASE_URL = "http://localhost:5168/api/";

    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        // Root Components Registration
        ConfigureRootComponents(builder);

        // Configure Services
        ConfigureServices(builder.Services, builder.Configuration);

        // Configure Logging
        ConfigureLogging();

        try
        {
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

    private static void ConfigureRootComponents(WebAssemblyHostBuilder builder)
    {
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        // HTTP Client
        ConfigureHttpClient(services);

        // Authentication & Authorization
        ConfigureAuth(services);

        // State Management & Storage
        ConfigureStateManagement(services);

        // Application Services
        ConfigureApplicationServices(services);

        // Route Configuration
        ConfigureRouting(services);
    }

    private static void ConfigureHttpClient(IServiceCollection services)
    {
        services.AddScoped(sp => new HttpClient
        {
            BaseAddress = new Uri(API_BASE_URL),
            Timeout = TimeSpan.FromSeconds(30)
        });
    }

    private static void ConfigureAuth(IServiceCollection services)
    {
        services.AddAuthorizationCore(options =>
        {
            // Default policy yêu cầu xác thực
            var defaultPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
            options.DefaultPolicy = defaultPolicy;

            // Policy cho các trang công khai
            options.AddPolicy("AllowAnonymous", policy =>
            {
                policy.RequireAssertion(_ => true);
            });

            // Policy cho Admin
            options.AddPolicy("RequireAdmin", policy =>
                policy.RequireAssertion(context =>
                {
                    var maChucVu = context.User.FindFirst("maChucVu")?.Value;
                    return maChucVu == "01";
                }));
        });

        // Authentication State Provider với Singleton pattern
        services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>(provider =>
        {
            var localStorage = provider.GetRequiredService<ILocalStorageService>();
            var authService = provider.GetRequiredService<IAuthService>();
            var loggingService = provider.GetRequiredService<ILoggingService>();
            return new CustomAuthStateProvider(localStorage, authService, loggingService);
        });
    }

    private static void ConfigureStateManagement(IServiceCollection services)
    {
        // Local Storage
        services.AddBlazoredLocalStorage();

        // Fluxor State Management
        services.AddFluxor(options =>
        {
            options.ScanAssemblies(typeof(Program).Assembly);
            options.AddMiddleware<FluxorLoggingMiddleware>();
#if DEBUG
            options.UseReduxDevTools();
#endif
        });
    }

    private static void ConfigureApplicationServices(IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ILoggingService, LoggingService>();

        // Thêm các service khác ở đây
    }

    private static void ConfigureRouting(IServiceCollection services)
    {
        services.Configure<RouteOptions>(options =>
        {
            options.LowercaseUrls = true;
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