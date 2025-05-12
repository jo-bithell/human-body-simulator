using StackExchange.Redis;

class Program
{
    static async Task Main(string[] args)
    {
        // Connection string for Redis running in Docker (default port 6379)
        ConfigurationOptions option = new ConfigurationOptions
        {
            AbortOnConnectFail = false,
            EndPoints = { "redis:6379" }
        };

        try
        {
            // Connect to Redis
            using var redis = await ConnectionMultiplexer.ConnectAsync(option);
            Console.WriteLine("Connected to Redis!");

            // Get a database instance
            var db = redis.GetDatabase();

            // Set a key-value pair
            string key = "exampleKey";
            string value = "Hello, Redis!";
            await db.StringSetAsync(key, value);
            Console.WriteLine($"Set key '{key}' with value '{value}'");

            // Retrieve the value
            string retrievedValue = await db.StringGetAsync(key);
            Console.WriteLine($"Retrieved value for key '{key}': {retrievedValue}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
