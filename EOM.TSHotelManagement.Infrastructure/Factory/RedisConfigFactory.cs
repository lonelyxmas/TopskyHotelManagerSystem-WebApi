using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace EOM.TSHotelManagement.Infrastructure
{
    public class RedisConfigFactory
    {
        private readonly IConfiguration _configuration;

        public RedisConfigFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public RedisConfig GetRedisConfig()
        {
            var redisSection = _configuration.GetSection("Redis");
            var redisConfig = new RedisConfig
            {
                ConnectionString = redisSection.GetValue<string>("ConnectionString"),
                Enable = redisSection.GetValue<bool>("Enable")
            };
            return redisConfig;
        }
    }
}
