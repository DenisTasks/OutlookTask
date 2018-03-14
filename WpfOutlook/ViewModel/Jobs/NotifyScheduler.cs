using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
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
            WpfScheduler?.Shutdown();
        }
        public static void Start()
        {
            NameValueCollection config = (NameValueCollection) ConfigurationManager.GetSection("quartz");
            ISchedulerFactory sf = new StdSchedulerFactory(config);
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

            WpfScheduler.Start();
        }
    }
}
