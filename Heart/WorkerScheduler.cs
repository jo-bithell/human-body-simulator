using Quartz.Impl;
using Quartz;
using SharedLogic;
using SharedLogic.Models.Cells;
using Heart;

namespace Lungs
{
    internal class WorkerScheduler: IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            StdSchedulerFactory factory = new StdSchedulerFactory();
            IScheduler scheduler = await factory.GetScheduler();

            IJobDetail heartBloodProducerWorker = JobBuilder.Create<HeartBloodProducerWorker>()
                .Build();

            IJobDetail myocyteDiffusionWorker = JobBuilder.Create<BloodDiffusionWorker<Myocyte>>()
                .Build();

            IJobDetail bloodProducer = JobBuilder.Create<BloodProducerWorker>()
                .Build();

            List<IJobDetail> jobs = [heartBloodProducerWorker, myocyteDiffusionWorker, bloodProducer];

            // Schedule the job
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

            await scheduler.Start();
        }
    }
}
