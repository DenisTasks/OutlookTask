using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            NameValueCollection properties = new NameValueCollection();

            // configure Thread Pool
            properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";
            properties["quartz.threadPool.makeThreadsDaemons"] = "true";
            properties["quartz.threadPool.threadCount"] = "5";
            properties["quartz.threadPool.threadPriority"] = "Normal";

            properties["quartz.scheduler.instanceName"] = "TestScheduler";
            properties[StdSchedulerFactory.PropertySchedulerInterruptJobsOnShutdown] = "true";
            properties[StdSchedulerFactory.PropertySchedulerMakeSchedulerThreadDaemon] = "true;";
            // configure Job Store
            properties["quartz.jobStore.misfireThreshold"] = "60000";
            properties["quartz.jobStore.type"] = "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz";
            properties["quartz.jobStore.driverDelegateType"] = "Quartz.Impl.AdoJobStore.SqlServerDelegate, Quartz";
            // all values in JobDataMaps will be Strings, and therefore can be stored as name-value pairs
            properties["quartz.jobStore.useProperties"] = "true";
            properties["quartz.jobStore.tablePrefix"] = "QRTZ_";
            properties["quartz.jobStore.clustered"] = "true";
            properties["quartz.scheduler.instanceId"] = "AUTO";
            properties["quartz.scheduler.dbFailureRetryInterval"] = "60000";
            properties["quartz.jobStore.dataSource"] = "default";
            properties["quartz.dataSource.default.provider"] = "SqlServer-20";
            properties["quartz.dataSource.default.connectionString"] = "data source=EPBYGROW0342\\MYSSQLSERVER;Database=quartz;Trusted_Connection=True;";
            properties["quartz.dataSource.default.maxConnections"] = "10";


            // First we must get a reference to a scheduler
            ISchedulerFactory sf = new StdSchedulerFactory(properties);
            IScheduler sched = sf.GetScheduler();

            sched.Start();

            IJobDetail job = JobBuilder.Create()
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .StartAt(DateTime.Now.AddSeconds(3))
                .Build();

            sched.ScheduleJob(job, trigger);
        }
    }
}
