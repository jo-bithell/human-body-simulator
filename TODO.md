- Frontend
- signalR
- Digestive system
- CI/CD
- Containerization
- heart muscle

docker run -p 6379:6379 redis
docker run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:4.0-management

using Microsoft.Extensions.Options;
using StackExchange.Redis;
using Web.PlatformAdmin.Models.Configuration;

namespace Web.PlatformAdmin.Services
{
    public class RedisAccessService
    {
        private readonly Lazy<ConnectionMultiplexer> _lazyConnection;
        private readonly string _connectionString;

        // Paramterless constructor needed for mocking in automated tests
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public RedisAccessService()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
        }

        public RedisAccessService(IOptions<ConnectionStringConfiguration> options)
        {
            ArgumentNullException.ThrowIfNull(options, nameof(options));

            _connectionString = options.Value.RedisCache;
            _lazyConnection = new Lazy<ConnectionMultiplexer>(valueFactory: CreateConnectionMultiplexer);
        }

        public virtual IDatabase GetDatabase()
        {
            return _lazyConnection.Value.GetDatabase();
        }

        private ConnectionMultiplexer CreateConnectionMultiplexer()
        {
            return ConnectionMultiplexer.Connect(_connectionString);
        }
    }
}
