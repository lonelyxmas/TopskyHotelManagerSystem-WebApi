using Microsoft.Extensions.Configuration;

namespace EOM.TSHotelManagement.Infrastructure
{
    public class JwtConfigFactory
    {
        private readonly IConfiguration _configuration;

        public JwtConfigFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public JwtConfig GetJwtConfig()
        {
            var jwtConfig = new JwtConfig
            {
                Key = _configuration["Jwt:Key"],
                ExpiryMinutes = int.Parse(_configuration["Jwt:ExpiryMinutes"])
            };
            return jwtConfig;
        }
    }
}
