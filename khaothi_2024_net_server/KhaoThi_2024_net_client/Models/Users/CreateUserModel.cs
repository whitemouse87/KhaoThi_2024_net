namespace KhaoThi_2024_net_client.Models.Users
{
    public class CreateUserModel:KhaoThiUserModel
    {
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
