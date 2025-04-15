using Microsoft.Extensions.DependencyInjection;
using Quartz;
using SharedLogic.Diffusion;
using SharedLogic.Messaging;
using SharedLogic.Models;
using SharedLogic.Models.Cells;
using SharedLogic.Redis;
using StackExchange.Redis;

namespace SharedLogic
{
    public class ServiceRegistrationHelper
    {
        private static string _sourceProject;
        public ServiceRegistrationHelper(string sourceProject)
        {

        }
        public void RegisterCommonBloodQuartzJobs(IServiceCollection services)
        {
            services.AddQuartz(q =>
            {
                q.ScheduleJob<BloodDiffusionJob<Myocyte>>(trigger => trigger
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(5)
                    .RepeatForever()));

                q.ScheduleJob<BloodProducerJob>(trigger => trigger
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(5)
                    .RepeatForever()));
            });
        }

        public void RegisterCommonRedisServices(IServiceCollection services)
        {
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost: 6379"));
            services.AddSingleton<IRedisCacheService, RedisCacheService>();
            services.AddHostedService<CellCachePopulatorService<Myocyte>>();
        }

        public void RegisterMessagingServices(IServiceCollection services)
        {
            services.AddHostedService<MessageConsumer<Blood>>();
            services.AddSingleton(provider =>
            {
                return new MessagePublisher<Blood>("lungs");
            });
        }
    }
}
