using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using KhaoThi_2024_net_client.Services.Logging;
using Microsoft.AspNetCore.Authorization;

namespace KhaoThi_2024_net_client.Components.Auth
{
    [AllowAnonymous]
    public class LoginRedirect : ComponentBase
    {
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;
        [Inject] private ILoggingService LoggingService { get; set; } = default!;
        [Inject] private IJSRuntime JSRuntime { get; set; } = default!;
        [CascadingParameter] private Task<AuthenticationState>? AuthState { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (AuthState == null) return;

            var authState = await AuthState;
            if (authState?.User?.Identity?.IsAuthenticated ?? false)
            {
                NavigationManager.NavigateTo("/dashboard");
            }
            else
            {
                var currentUrl = NavigationManager.Uri;
                // Chỉ log warning nếu cố truy cập trang yêu cầu xác thực
                if (!currentUrl.EndsWith("/login") && !currentUrl.EndsWith("/"))
                {
                    try
                    {
                        var ipAddress = await JSRuntime.InvokeAsync<string>("getClientIP");
                        await Logger.Warning(
                            $"[SECURITY] Unauthorized access attempt - IP: {ipAddress}, URL: {currentUrl}");
                    }
                    catch (Exception ex)
                    {
                        await Logger.Error("Error logging unauthorized access", ex);
                    }
                }
                NavigationManager.NavigateTo("/login");
            }
        }
    }
}