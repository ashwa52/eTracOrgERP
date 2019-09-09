using Quartz;
using Quartz.Impl;
using System;


namespace WorkOrderEMS.Controllers.Common
{
    public class JobScheduler
    {
        public static void Start()
        {
            ISchedulerFactory schedFact = new StdSchedulerFactory();
            IScheduler scheduler = schedFact.GetScheduler().GetAwaiter().GetResult();
            //IScheduler scheduler = schedFact.StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            IJobDetail job = JobBuilder.Create<Schedular>().Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithDailyTimeIntervalSchedule
                  (s =>
                     s.WithIntervalInHours(24)
                    .OnEveryDay()
                    .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(0, 1))
                  )
                .Build();
            //ITrigger trigger = TriggerBuilder.Create()
            //.WithCronSchedule(string.Format("0 {0} {1} ? * *", 0, 13))
            //   .Build();
            //scheduler.ScheduleJob(job, trigger);
        }
    }
}