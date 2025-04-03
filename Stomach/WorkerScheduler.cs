using Quartz.Impl;
using Quartz;
using SharedLogic;
using SharedLogic.Models.Cells;

namespace Stomach
{
    internal static class WorkerScheduler
    {
        public static async Task ScheduleJobs()
        {
            StdSchedulerFactory factory = new StdSchedulerFactory();
            IScheduler scheduler = await factory.GetScheduler();
            await scheduler.Start();

            IJobDetail myocyteDiffusionWorker = JobBuilder.Create<BloodDiffusionWorker<Myocyte>>()
                .Build();

            IJobDetail bloodProducer = JobBuilder.Create<BloodProducerWorker>()
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(5)
                    .RepeatForever())
                .Build();

            // Schedule the job
            await scheduler.ScheduleJob(myocyteDiffusionWorker, trigger);
            await scheduler.ScheduleJob(bloodProducer, trigger);
        }
    }
}
