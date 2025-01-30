namespace khaothi_2024_net_server.Features.Authentication.DTOs
{
    public enum LoginErrorType
    {
        InvalidCredentials,
        AccountLocked,
        TooManyAttempts,
        AccountInactive
    }
}
