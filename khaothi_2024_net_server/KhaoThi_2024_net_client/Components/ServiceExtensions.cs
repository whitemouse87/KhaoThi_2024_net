namespace KhaoThi_2024_net_client.Components
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddReportServices(this IServiceCollection services)
        {
            // Register the Word Report Service
            services.AddScoped<IReportService, ReportService>();

            return services;
        }
    }
}
