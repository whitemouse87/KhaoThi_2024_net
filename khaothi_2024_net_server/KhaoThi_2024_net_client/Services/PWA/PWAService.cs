using Microsoft.JSInterop;

namespace KhaoThi_2024_net_client.Services.PWA
{
    public class PWAService
    {
        private readonly IJSRuntime _jsRuntime;

        public PWAService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task RegisterServiceWorkerAsync()
        {
            try
            {
                await _jsRuntime.InvokeVoidAsync("navigator.serviceWorker.register", "/service-worker.js");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Service Worker registration failed: {ex.Message}");
            }
        }
    }
}
