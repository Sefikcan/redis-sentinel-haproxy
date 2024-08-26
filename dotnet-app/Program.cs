using System;
using System.Threading.Tasks;
using StackExchange.Redis;

class Program
{
    static async Task Main(string[] args)
    {
        var options = new ConfigurationOptions
        {
            EndPoints = { "haproxy:6379" },
            ConnectRetry = 5,
            ConnectTimeout = 5000,
            KeepAlive = 180, 
            AbortOnConnectFail = false 
        };

        var redis = await ConnectionMultiplexer.ConnectAsync(options);

        var db = redis.GetDatabase();

        while (true)
        {
            try
            {
                await db.StringSetAsync("test", "Hello, Redis!");

                var value = await db.StringGetAsync("test");
                Console.WriteLine($"Value from Redis: {value}");
            }
            catch (RedisConnectionException ex)
            {
                Console.WriteLine($"Redis connection error: {ex.Message}");
            }

            await Task.Delay(2000);
        }
    }
}
