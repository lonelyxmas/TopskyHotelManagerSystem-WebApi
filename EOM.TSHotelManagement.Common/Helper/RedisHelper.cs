using EOM.TSHotelManagement.Infrastructure;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;

namespace EOM.TSHotelManagement.Common
{
    public class RedisHelper
    {
        private readonly object _lock = new object();
        private IConnectionMultiplexer _connection;
        private readonly ILogger<RedisHelper> logger;
        private readonly RedisConfigFactory configFactory;

        public RedisHelper(RedisConfigFactory configFactory, ILogger<RedisHelper> logger)
        {
            this.configFactory = configFactory;
            this.logger = logger;
            Initialize();
        }

        public void Initialize()
        {
            lock (_lock)
            {
                if (_connection != null) return;
                try
                {
                    var redisConfig = configFactory.GetRedisConfig();

                    if (!redisConfig.Enable)
                    {
                        logger.LogInformation("Redis功能未启用，跳过初始化");
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(redisConfig?.ConnectionString))
                        throw new ArgumentException("Redis连接字符串不能为空");

                    var options = ConfigurationOptions.Parse(redisConfig.ConnectionString);
                    options.AbortOnConnectFail = false;
                    options.ConnectTimeout = 5000;
                    options.ReconnectRetryPolicy = new ExponentialRetry(3000);

                    _connection = ConnectionMultiplexer.Connect(options);
                    _connection.GetDatabase().Ping();
                }
                catch (Exception ex)
                {
                    logger.LogCritical(ex, "Redis初始化失败");
                    throw;
                }
            }
        }

        public IDatabase GetDatabase()
        {
            if (_connection == null)
                throw new System.Exception("RedisHelper not initialized. Call Initialize first.");

            return _connection.GetDatabase();
        }

        public async Task<bool> CheckServiceStatusAsync()
        {
            try
            {
                var db = GetDatabase();
                var ping = await db.PingAsync();
                logger.LogInformation($"Redis响应时间：{ping.TotalMilliseconds} ms");
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Redis服务检查失败");
                return false;
            }
        }

        public async Task<bool> SetAsync(string key, string value, TimeSpan? expiry = null)
        {
            try
            {
                var db = GetDatabase();
                if (expiry.HasValue)
                {
                    return await db.StringSetAsync(key, value, new StackExchange.Redis.Expiration(expiry.Value));
                }
                else
                {
                    return await db.StringSetAsync(key, value);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Redis设置值失败，键：{key}！");
                return false;
            }
        }

        public async Task<string> GetAsync(string key)
        {
            try
            {
                var db = GetDatabase();
                return await db.StringGetAsync(key);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Redis获取值失败，键：{key}！");
                return null;
            }
        }

        public async Task<bool> DeleteAsync(string key)
        {
            try
            {
                var db = GetDatabase();
                return await db.KeyDeleteAsync(key);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Redis删除键失败，键：{key}！");
                return false;
            }
        }

        public async Task<bool> KeyExistsAsync(string key)
        {
            try
            {
                var db = GetDatabase();
                return await db.KeyExistsAsync(key);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Redis键存在检查失败，键：{key}！");
                return false;
            }
        }

    }
}
