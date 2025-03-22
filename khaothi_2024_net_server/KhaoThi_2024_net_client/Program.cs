using Blazored.LocalStorage;
using Fluxor;
using Fluxor.Blazor.Web.ReduxDevTools;
using KhaoThi_2024_net_client;
using KhaoThi_2024_net_client.Components;
using KhaoThi_2024_net_client.Components.Auth;
using KhaoThi_2024_net_client.Middleware;
using KhaoThi_2024_net_client.Services.Auth;
using KhaoThi_2024_net_client.Services.User;
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
using Microsoft.Extensions.Hosting;
using AutoMapper;
using KhaoThi_2024_net_client.Models.Users;
using KhaoThi_2024_net_client.Models.Auth;
using KhaoThi_2024_net_client.Services.Shares;
using KhaoThi_2024_net_client.Services.PWA;
using Blazored.Modal;
using Radzen;
using KhaoThi_2024_net_client.Services.BC_1_ThongTinDonVi;
using KhaoThi_2024_net_client.Services.BC_2_NhomMonDonVi;
using KhaoThi_2024_net_client.Services.BC_4_LanhDaoDonVi;

public class Program
{
    private const string API_BASE_URL = "https://localhost:7168/api/";
    private const int HTTP_TIMEOUT_SECONDS = 59;

    public static async Task Main(string[] args)
    {
        //try
        //{
        //    var builder = WebAssemblyHostBuilder.CreateDefault(args);
        //    ConfigureApp(builder);

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

        try
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            ConfigureApp(builder);

            Log.Information("===== KHỞI ĐỘNG ỨNG DỤNG =====");
            var app = builder.Build();

            // Initialize Logger Service
            var loggingService = app.Services.GetRequiredService<ILoggingService>();
            Logger.Initialize(loggingService);

            // Đăng ký Service Worker nếu trong môi trường Production
            if (builder.HostEnvironment.IsProduction())
            {
                var pwaService = app.Services.GetRequiredService<PWAService>();
                await pwaService.RegisterServiceWorkerAsync();
            }

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
        ConfigureBlazoredModal(services); // Gọi hàm cấu hình BlazoredModal
        ConfigureRadzenBlazor(services);
        ConfigureApplicationServices(services);
        ConfigureRouting(services);
        ConfigureAutoMapper(services); // Gọi hàm cấu hình AutoMapper
        // Add WebAssembly specific services
        services.AddScoped<CircularProgress>();
        services.AddScoped<IWebAssemblyHostEnvironment>(sp =>
            sp.GetRequiredService<IWebAssemblyHostEnvironment>());
    }

    private static void ConfigureHttpClient(IServiceCollection services)
    {
        // Đăng ký AuthInterceptor
        services.AddScoped<AuthInterceptor>();

        // Đăng ký named HttpClient với AuthInterceptor
        services.AddHttpClient("API", client =>
        {
            client.BaseAddress = new Uri(API_BASE_URL);
            client.Timeout = TimeSpan.FromSeconds(HTTP_TIMEOUT_SECONDS);
        }).AddHttpMessageHandler<AuthInterceptor>();

        // Đăng ký default HttpClient (giữ nguyên cái cũ nếu cần)
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
        var maChucVu = context.User.FindFirst("MaChucVu")?.Value;
        return maChucVu == "01";
    }

    private static CustomAuthStateProvider CreateCustomAuthStateProvider(IServiceProvider provider)
    {
        var localStorage = provider.GetRequiredService<ILocalStorageService>();
        var authService = provider.GetRequiredService<IAuthService>();
        var khaothiuserService = provider.GetRequiredService<IUserService>();
        var BCThongTinDonVi = provider.GetRequiredService<IBCThongTinDonViService>();
        var BCNhomMonDonVi = provider.GetRequiredService<IBCNhomMonDonViService>();
        var BCLanhDaoDonVi = provider.GetRequiredService<IBCLanhDaoDonViService>();
        var mapper = provider.GetRequiredService<IMapper>();
        return new CustomAuthStateProvider(localStorage, authService, khaothiuserService, mapper);
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
            config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopRight;
            config.SnackbarConfiguration.PreventDuplicates = true;
            config.SnackbarConfiguration.NewestOnTop = true;
            config.SnackbarConfiguration.ShowCloseIcon = true;
            config.SnackbarConfiguration.VisibleStateDuration = 3000;
            config.SnackbarConfiguration.HideTransitionDuration = 500;
            config.SnackbarConfiguration.ShowTransitionDuration = 500;
            config.SnackbarConfiguration.SnackbarVariant = MudBlazor.Variant.Filled;
            config.ResizeOptions = new ResizeOptions
            {
                ReportRate = 100,            // Milliseconds between Resize updates
                EnableLogging = false,        // Log resize events to console
                SuppressInitEvent = true,    // Don't raise initial event on startup
                NotifyOnBreakpointOnly = true // Notify only on breakpoint change
            };
        });

        services.AddMudMarkdownServices();
        services.AddMudExtensions();
    }

    private static void ConfigureApplicationServices(IServiceCollection services)
    {


        // Core Services

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ILoggingService, LoggingService>();
        services.AddScoped<IUserService, KhaoThiUserService>();
        services.AddScoped<IPageTitleService, PageTitleService>();
        services.AddScoped<IBCThongTinDonViService, BCThongTinDonViService>();
        services.AddScoped<IBCNhomMonDonViService, BCNhomMonDonViService>();
        services.AddScoped<IBCLanhDaoDonViService, BCLanhDaoDonViService>();
        // Add other application services here
        ConfigureAdditionalServices(services);
    }

    private static void ConfigureAdditionalServices(IServiceCollection services)
    {
        // Add any additional application-specific services here
        // Example: services.AddScoped<IMyService, MyService>();
    }
    private static void ConfigureAutoMapper(IServiceCollection services)
    {
        services.AddAutoMapper(config =>
        {
            config.CreateMap<KhaoThiUserModel, UserInfo>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                .ForMember(dest => dest.TenDangNhap, opt => opt.MapFrom(src => src.TenDangNhap))
                .ForMember(dest => dest.HoTen, opt => opt.MapFrom(src => src.HoTen))
                .ForMember(dest => dest.MaDonVi, opt => opt.MapFrom(src => src.MaDonVi))
                .ForMember(dest => dest.TenDonVi, opt => opt.MapFrom(src => src.TenDonVi))
                .ForMember(dest => dest.MaChucVu, opt => opt.MapFrom(src => src.MaChucVu))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

        });
    }
    private static void ConfigureRouting(IServiceCollection services)
    {
        services.Configure<RouteOptions>(options =>
        {
            options.LowercaseUrls = true;

            options.AppendTrailingSlash = false;
        });
    }
    private static void ConfigureBlazoredModal(IServiceCollection services)
    {
        services.AddBlazoredModal();

    }
    private static void ConfigureRadzenBlazor(IServiceCollection services)
    {
        services.AddRadzenComponents();

    }

    //    private static void ConfigureLogging()
    //    {


    //        var logConfig = new LoggerConfiguration()
    //                .MinimumLevel.Warning()  // Tăng mức log tối thiểu lên Warning thay vì Error
    //                .Filter.ByExcluding(e => e.Properties.ContainsKey("SourceContext") &&
    //                    e.Properties["SourceContext"].ToString().Contains("Microsoft.AspNetCore"))
    //                .WriteTo.BrowserConsole(
    //                    restrictedToMinimumLevel: LogEventLevel.Warning,
    //                    outputTemplate: "[{Level}] {Message}{Exception}"
    //                )
    //                .Enrich.FromLogContext();

    //#if DEBUG
    //        logConfig.WriteTo.Debug(
    //            restrictedToMinimumLevel: LogEventLevel.Warning,
    //            outputTemplate: "[{Level}] {Message}{Exception}"
    //        );
    //#endif

    //        Log.Logger = logConfig.CreateLogger();
    //    }
    private static void ConfigureLogging()
    {
        const string outputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";

        var logConfig = new LoggerConfiguration()
            // Cấu hình mức log tối thiểu
            .MinimumLevel.Warning()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
            .MinimumLevel.Override("System", LogEventLevel.Error)

            // Lọc các log không cần thiết
            .Filter.ByExcluding(e =>
                e.Properties.ContainsKey("SourceContext") &&
                (e.Properties["SourceContext"].ToString().Contains("Microsoft.AspNetCore") ||
                 e.Properties["SourceContext"].ToString().Contains("System.Net.Http")))

            // Thêm các thuộc tính từ context
            .Enrich.FromLogContext()

            // Cấu hình đầu ra cho Browser Console
            .WriteTo.BrowserConsole(
                restrictedToMinimumLevel: LogEventLevel.Warning,
                outputTemplate: outputTemplate
            );

        // Cấu hình cho môi trường Development
#if DEBUG
        logConfig
            .WriteTo.Debug(
                restrictedToMinimumLevel: LogEventLevel.Warning,
                outputTemplate: outputTemplate
            )
            .MinimumLevel.Override("KhaoThi_2024_net_client", LogEventLevel.Debug); // Log chi tiết cho namespace của ứng dụng
#endif

        try
        {
            Log.Logger = logConfig.CreateLogger();
            Log.Information("Logging configuration initialized successfully");
        }
        catch (Exception ex)
        {
            // Fallback logger trong trường hợp không thể tạo logger chính
            Log.Logger = new LoggerConfiguration()
                .WriteTo.BrowserConsole()
                .CreateLogger();
            Log.Error(ex, "Failed to create logger configuration");
        }
    }
    //    private static void ConfigureLogging()
    //    {
    //        const string outputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";
    //        var logConfig = new LoggerConfiguration()
    //            // Thay đổi mức log tối thiểu thành Verbose (mức thấp nhất)
    //            .MinimumLevel.Verbose()
    //            // Ghi đè mức log cho thư viện Microsoft và System ở mức Information
    //            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    //            .MinimumLevel.Override("System", LogEventLevel.Information)
    //            // Bỏ filter lọc các log - để ghi tất cả log
    //            // .Filter.ByExcluding(...) - đã bỏ dòng này
    //            // Thêm các thuộc tính từ context
    //            .Enrich.FromLogContext()
    //            // Cấu hình đầu ra cho Browser Console - thay đổi mức log
    //            .WriteTo.BrowserConsole(
    //                restrictedToMinimumLevel: LogEventLevel.Verbose,
    //                outputTemplate: outputTemplate
    //            );

    //        // Cấu hình cho môi trường Development
    //#if DEBUG
    //        logConfig
    //            .WriteTo.Debug(
    //                restrictedToMinimumLevel: LogEventLevel.Verbose,
    //                outputTemplate: outputTemplate
    //            )
    //            .MinimumLevel.Override("KhaoThi_2024_net_client", LogEventLevel.Verbose); // Log chi tiết cho namespace của ứng dụng
    //#endif

    //        try
    //        {
    //            Log.Logger = logConfig.CreateLogger();
    //            Log.Information("Logging configuration initialized successfully");
    //        }
    //        catch (Exception ex)
    //        {
    //            // Fallback logger trong trường hợp không thể tạo logger chính
    //            Log.Logger = new LoggerConfiguration()
    //                .MinimumLevel.Verbose() // Đảm bảo fallback logger cũng ghi tất cả log
    //                .WriteTo.BrowserConsole()
    //                .CreateLogger();
    //            Log.Error(ex, "Failed to create logger configuration");
    //        }
    //    }
    //    private static void ConfigureLogging()
    //    {
    //        const string outputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";

    //        var baseAddress = API_BASE_URL;  // Đổi thành base URL thích hợp

    //        var logConfig = new LoggerConfiguration()
    //            .MinimumLevel.Verbose()
    //            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    //            .MinimumLevel.Override("System", LogEventLevel.Information)
    //            .Enrich.FromLogContext()
    //            .WriteTo.BrowserConsole(
    //                restrictedToMinimumLevel: LogEventLevel.Verbose,
    //                outputTemplate: outputTemplate
    //            )
    //            // Thêm HTTP sink để gửi logs về API endpoint 'logs'
    //            .WriteTo.Http(
    //                requestUri: $"{baseAddress}logs",
    //                queueLimitBytes: 10 * 1024 * 1024, // 10MB queue limit
    //                                                   // Số lượng events tối đa trong một batch
    //                period: TimeSpan.FromSeconds(5),   // Khoảng thời gian giữa các lần gửi
    //                restrictedToMinimumLevel: LogEventLevel.Warning
    //            );

    //#if DEBUG
    //        logConfig
    //            .WriteTo.Debug(
    //                restrictedToMinimumLevel: LogEventLevel.Verbose,
    //                outputTemplate: outputTemplate
    //            )
    //            .MinimumLevel.Override("KhaoThi_2024_net_client", LogEventLevel.Verbose);
    //#endif

    //        try
    //        {
    //            Log.Logger = logConfig.CreateLogger();
    //            Log.Information("Logging configuration initialized successfully");
    //        }
    //        catch (Exception ex)
    //        {
    //            Log.Logger = new LoggerConfiguration()
    //                .MinimumLevel.Verbose()
    //                .WriteTo.BrowserConsole()
    //                .CreateLogger();
    //            Log.Error(ex, "Failed to create logger configuration");
    //        }
    //    }
}