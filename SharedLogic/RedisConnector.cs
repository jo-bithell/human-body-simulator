using StackExchange.Redis;

public class RedisConnector
{
    private const string RedisConnectionString = "redis:6379"; // <- your Redis container name
    private const int MaxRetries = 5;
    private const int DelayBetweenRetriesMs = 2000; // 2 seconds

    public static ConnectionMultiplexer ConnectWithRetry()
    {
        int attempt = 0;

        while (true)
        {
            try
            {
                Console.WriteLine($"Attempting to connect to Redis... (Attempt {attempt + 1})");
                var connection = ConnectionMultiplexer.Connect(RedisConnectionString);
                Console.WriteLine("Connected to Redis successfully!");
                return connection;
            }
            catch (RedisConnectionException ex)
            {
                attempt++;
                Console.WriteLine($"Failed to connect to Redis: {ex.Message}");

                if (attempt >= MaxRetries)
                {
                    Console.WriteLine("Max retries reached. Throwing exception.");
                    throw;
                }

                Console.WriteLine($"Retrying in {DelayBetweenRetriesMs / 1000} seconds...");
            }
        }
    }
}
