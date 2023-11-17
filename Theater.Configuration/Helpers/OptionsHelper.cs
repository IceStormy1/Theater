using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;

namespace Theater.Configuration.Helpers;

public static class OptionsHelper
{
    public static ConfigurationOptions GetRedisConfigurationOptions(IConfiguration configuration, IHostEnvironment hostEnvironment)
    {
        var redisConfigurationOptions = ConfigurationOptions.Parse(configuration.GetConnectionString("RedisConnectionString")!);
        redisConfigurationOptions.ChannelPrefix = "Theater_" + hostEnvironment.EnvironmentName;
        redisConfigurationOptions.AbortOnConnectFail = false;
        redisConfigurationOptions.ConnectTimeout = 5000;
        redisConfigurationOptions.SyncTimeout = 5000;
        redisConfigurationOptions.ConnectRetry = 2;

        return redisConfigurationOptions;
    }
}