using khaothi_2024_net_server.Core.Interfaces;

namespace khaothi_2024_net_server.Infrastructure.Security;

public class BCryptPasswordHasher : IPasswordHasher
{
    private readonly IConfiguration _configuration;

    public BCryptPasswordHasher(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password,
            _configuration.GetValue<int>("Security:PasswordHashIterations", 12));
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}

