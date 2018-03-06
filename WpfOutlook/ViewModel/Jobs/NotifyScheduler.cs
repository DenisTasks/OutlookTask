using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using BLL.EntitesDTO;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using ViewModel.Models;

namespace ViewModel.Jobs
{
    public static class NotifyScheduler
    {
        public static IScheduler WpfScheduler;

        public static void Shutdown()
        {
            WpfScheduler.Shutdown();
        }
        public static void Start()
        {
            NameValueCollection properties = new NameValueCollection();

            properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";
            properties["quartz.threadPool.makeThreadsDaemons"] = "true";
            properties["quartz.threadPool.threadCount"] = "5";
            properties["quartz.threadPool.threadPriority"] = "Normal";

            properties["quartz.scheduler.instanceName"] = "TestScheduler";
            properties["quartz.jobStore.misfireThreshold"] = "60000";
            properties["quartz.jobStore.type"] = "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz";
            properties["quartz.jobStore.driverDelegateType"] = "Quartz.Impl.AdoJobStore.SqlServerDelegate, Quartz";
            properties["quartz.jobStore.useProperties"] = "false";
            properties["quartz.jobStore.tablePrefix"] = "QRTZ_";
            properties["quartz.jobStore.clustered"] = "true";
            properties["quartz.scheduler.instanceId"] = "AUTO";
            properties["quartz.scheduler.dbFailureRetryInterval"] = "60000";
            properties["quartz.jobStore.dataSource"] = "default";
            properties["quartz.dataSource.default.provider"] = "SqlServer-20";
            properties["quartz.dataSource.default.connectionString"] = "Server=EPBYGROW0335\\DB;Database=Quartz;Trusted_Connection=True;";

            ISchedulerFactory sf = new StdSchedulerFactory(properties);
            WpfScheduler = sf.GetScheduler();

            List<AppointmentModel> missedApps = new List<AppointmentModel>();

            foreach (var group in WpfScheduler.GetJobGroupNames())
            {
                foreach (var myJob in WpfScheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(group)))
                {
                    var app = (AppointmentModel)WpfScheduler.GetJobDetail(myJob).JobDataMap["myApp"];
                    if (app != null && app.BeginningDate < DateTime.Now)
                    {
                        missedApps.Add(app);
                        var triggerKeyList = WpfScheduler.GetTriggersOfJob(myJob);
                        var triggerKey = triggerKeyList[0].Key;
                        WpfScheduler.UnscheduleJob(WpfScheduler.GetTrigger(triggerKey).Key);
                    }
                }
            }

            if (missedApps.Count > 0)
            {
                ITrigger missedTrigger = TriggerBuilder.Create()
                    .StartNow()
                    .Build();

                IJobDetail missedJob = JobBuilder.Create<MissedNotifyCreater>()
                    .Build();
                missedJob.JobDataMap.Put("myApp", missedApps);

                WpfScheduler.ScheduleJob(missedJob, missedTrigger);
            }

            //ITrigger trigger777 = TriggerBuilder.Create()
            //    .WithIdentity("myTrigger777", "myGroup")
            //    .StartAt(DateTime.Now.AddSeconds(300))
            //    .Build();

            //IJobDetail job777 = JobBuilder.Create<NotifyCreater>()
            //    .WithIdentity("myJob777", "myGroup")
            //    .Build();
            //job777.JobDataMap.Put("myApp", new AppointmentDTO() { Subject = "MySubject 777", BeginningDate = DateTime.Now.AddSeconds(20), EndingDate = DateTime.Now.AddHours(2) });

            //WpfScheduler.ScheduleJob(job777, trigger777);

            //ITrigger trigger666 = TriggerBuilder.Create()
            //    .WithIdentity("myTrigger666", "myGroup")
            //    .StartAt(DateTime.Now.AddSeconds(300))
            //    .Build();

            //IJobDetail job666 = JobBuilder.Create<NotifyCreater>()
            //    .WithIdentity("myJob666", "myGroup")
            //    .Build();
            //job666.JobDataMap.Put("myApp", new AppointmentDTO() { Subject = "MySubject 666", BeginningDate = DateTime.Now.AddSeconds(20), EndingDate = DateTime.Now.AddHours(2) });

            //WpfScheduler.ScheduleJob(job666, trigger666);

            WpfScheduler.Start();
        }
    }

}
