using EOM.TSHotelManagement.Shared;
using Microsoft.Extensions.Configuration;

namespace EOM.TSHotelManagement.WebApi
{
    public class LskyConfigFactory : ILskyConfigFactory
    {
        private readonly IConfiguration _configuration;

        public LskyConfigFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public LskyConfig GetLskyConfig()
        {
            var lskyConfig = new LskyConfig
            {
                BaseAddress = _configuration.GetSection("Lsky").GetValue<string>("BaseAddress"),
                Email = _configuration.GetSection("Lsky").GetValue<string>("Email"),
                Password = _configuration.GetSection("Lsky").GetValue<string>("Password"),
                UploadApi = _configuration.GetSection("Lsky").GetValue<string>("UploadApi"),
                GetTokenApi = _configuration.GetSection("Lsky").GetValue<string>("GetTokenApi")
            };
            return lskyConfig;
        }
    }
}
