using Microsoft.Extensions.DependencyInjection;
using Quartz;
using SharedLogic.Diffusion.Jobs;
using SharedLogic.Diffusion.Services;
using SharedLogic.Digestion.Jobs;
using SharedLogic.Messaging.Factories;
using SharedLogic.Messaging.Factories.Interfaces;
using SharedLogic.Messaging.Models;
using SharedLogic.Messaging.Services;
using SharedLogic.Models;
using SharedLogic.Models.Cells;
using StackExchange.Redis;
using SharedLogic.Caching.Services;
using SharedLogic.Caching.Services.Interfaces;
using SharedLogic.Motion.Services;
using SharedLogic.Motion;
using SharedLogic.Respiration.Factories;
using SharedLogic.Respiration.Jobs;
using SharedLogic.Respiration.Services.Interfaces;
using SharedLogic.Respiration.Services;

namespace SharedLogic
{
    public static class ServiceRegistrationHelper
    {
        public static void RegisterCommonServices(string organName, IServiceCollection services)
        {
            RegisterOrganName(organName, services);
            RegisterCommonBloodMessagingServices(services);
            RegisterCommonRedisServices(services);
            RegisterServicesForCell<Myocyte>(services);
            RegisterMotionService(services);
        }

        public static void RegisterServicesForCell<TCell>(IServiceCollection services) where TCell : Cell
        {
            services.AddHostedService<CellCachePopulatorService<TCell>>();
            services.AddSingleton<ICacheManagementService<TCell>, CacheManagementService<TCell>>();
            services.AddSingleton<IRespirationProcessorFactory<TCell>, RespirationProcessorFactory<TCell>>();
            services.AddSingleton<IRespirationService<TCell>, RespirationService<TCell>>();
            services.AddScoped<RespirationJob<TCell>>();
            services.AddQuartz(q =>
            {
                q.ScheduleJob<RespirationJob<TCell>>(trigger => trigger
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(5)
                    .RepeatForever()));
            });
            services.AddSingleton<BloodDiffusionService<TCell>>();
            services.AddScoped<BloodDiffusionJob<TCell>>();
            services.AddQuartz(q =>
            {
                q.ScheduleJob<BloodDiffusionJob<TCell>>(trigger => trigger
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(5)
                    .RepeatForever()));
            });
            services.AddQuartzHostedService();
        }

        public static void RegisterDigestionServices(IServiceCollection services)
        {
            services.AddScoped<DigestionJob>();
            services.AddQuartz(q =>
            {
                q.ScheduleJob<DigestionJob>(trigger => trigger
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(5)
                    .RepeatForever()));
            });
        }

        private static void RegisterOrganName(string organName, IServiceCollection services)
        {
            services.AddSingleton(organName);
        }

        private static void RegisterMotionService(IServiceCollection services)
        {
            services.AddSingleton<IMotionService, MotionService>();
        }

        private static void RegisterCommonRedisServices(IServiceCollection services)
        {
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost: 6801"));
            services.AddSingleton<IRedisCacheService, RedisCacheService>();
        }

        private static void RegisterCommonBloodMessagingServices(IServiceCollection services)
        {
            services.AddSingleton<SnapshotCache<Blood>>();
            services.AddHostedService<MessageConsumer<Blood>>();

            services.AddSingleton<IMessageProducerFactory, MessageProducerFactory>();
            services.AddScoped<BloodProducerJob>();
            services.AddQuartz(q =>
            {
                q.ScheduleJob<BloodProducerJob>(trigger => trigger
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(5)
                    .RepeatForever()));
            });
        }
    }
}
