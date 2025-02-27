//using khaothi_2024_net_server.Core.Interfaces;

//namespace khaothi_2024_net_server.Infrastructure.Security;

//public class BCryptPasswordHasher : IPasswordHasher
//{
//    private readonly IConfiguration _configuration;

//    public BCryptPasswordHasher(IConfiguration configuration)
//    {
//        _configuration = configuration;
//    }

//    public string HashPassword(string password)
//    {
//        return BCrypt.Net.BCrypt.HashPassword(password,
//            _configuration.GetValue<int>("Security:PasswordHashIterations", 12));
//    }


//    public bool VerifyPassword(string password, string hashedPassword)
//    {
//        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
//    }
//}

using khaothi_2024_net_server.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace khaothi_2024_net_server.Infrastructure.Security
{
    public class BCryptPasswordHasher : IPasswordHasher
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<BCryptPasswordHasher> _logger;

        public BCryptPasswordHasher(
            IConfiguration configuration,
            ILogger<BCryptPasswordHasher> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public string HashPassword(string password)
        {
            try
            {
                if (string.IsNullOrEmpty(password))
                {
                    _logger.LogError("Attempt to hash empty password");
                    throw new ArgumentNullException(nameof(password));
                }

                var iterations = _configuration.GetValue<int>("Security:PasswordHashIterations", 12);
                //_logger.LogDebug("Hashing password with {Iterations} iterations", iterations);

                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, iterations);

                // Verify format
                if (!hashedPassword.StartsWith("$2a$") || hashedPassword.Length != 60)
                {
                    _logger.LogError("Generated hash has invalid format. Length: {Length}", hashedPassword.Length);
                    throw new InvalidOperationException("Generated hash has invalid format");
                }

                //_logger.LogDebug("Password hashed successfully. Hash format: {Format}", hashedPassword.Substring(0, 7));
                return hashedPassword;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error hashing password");
                throw;
            }
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            try
            {
                // Validate input
                if (string.IsNullOrEmpty(password))
                {
                    _logger.LogError("Attempt to verify with empty password");
                    return false;
                }

                if (string.IsNullOrEmpty(hashedPassword))
                {
                    _logger.LogError("Attempt to verify against empty hash");
                    return false;
                }

                // Check hash format
                if (!hashedPassword.StartsWith("$2a$"))
                {
                    _logger.LogError("Invalid hash format. Hash starts with: {Prefix}",
                        hashedPassword.Substring(0, Math.Min(10, hashedPassword.Length)));
                    return false;
                }

                //_logger.LogError("Verifying password:");
                //_logger.LogError("- Password length: {Length}", password.Length);
                //_logger.LogError("- Hash length: {Length}", hashedPassword.Length);
                //_logger.LogError("- Hash format: {Format}", hashedPassword.Substring(0, 7));

                var isValid = BCrypt.Net.BCrypt.Verify(password, hashedPassword);
                _logger.LogError("Password verification result: {Result}", isValid);

                return isValid;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verifying password. Exception: {Message}", ex.Message);
                return false;
            }
        }
    }
}