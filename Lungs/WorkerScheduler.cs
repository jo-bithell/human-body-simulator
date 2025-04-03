using Quartz.Impl;
using Quartz;
using SharedLogic;
using SharedLogic.Models.Cells;

namespace Lungs
{
    internal static class WorkerScheduler
    {
        public static async Task ScheduleJobs()
        {
            StdSchedulerFactory factory = new StdSchedulerFactory();
            IScheduler scheduler = await factory.GetScheduler();
            await scheduler.Start();

            IJobDetail breathingWorker = JobBuilder.Create<BreathingWorker>()
                .Build();

            IJobDetail airDiffusionWorker = JobBuilder.Create<AirDiffusionWorker>()
                .Build();

            IJobDetail myocyteDiffusionWorker = JobBuilder.Create<BloodDiffusionWorker<Myocyte>>()
                .Build();

            IJobDetail alveolarDiffusionWorker = JobBuilder.Create<BloodDiffusionWorker<AlveolarCell>>()
                .Build();

            IJobDetail bloodProducer = JobBuilder.Create<BloodProducerWorker>()
                .Build();

            List<IJobDetail> jobs = [breathingWorker, airDiffusionWorker, myocyteDiffusionWorker, alveolarDiffusionWorker, bloodProducer];

            foreach (var job in jobs)
            {
                ITrigger trigger = TriggerBuilder.Create()
                    .StartNow()
                    .WithSimpleSchedule(x => x
                        .WithIntervalInSeconds(5)
                        .RepeatForever())
                    .Build();

                await scheduler.ScheduleJob(job, trigger);
            }
        }
    }
}
