using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Quartz;
using ViewModel.Models;

namespace ViewModel.Jobs
{
    public class NotifyCreater : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            var dataMap = context.MergedJobDataMap;
            var instance = (AppointmentModel)dataMap["myApp"];

            MessageBox.Show($"{instance.Subject} from {instance.BeginningDate} to {instance.EndingDate} at {instance.Room}");
        }
    }

    public class MissedNotifyCreater : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            var dataMap = context.MergedJobDataMap;
            var instance = (List<AppointmentModel>)dataMap["myApp"];
            string missedApps = String.Empty;
            for (int i = 0; i < instance.Count; i++)
            {
                AppointmentModel infoApp = instance.ElementAt(i);
                string finallyInfo =
                    $"{infoApp.Subject} at {infoApp.Room} from {infoApp.BeginningDate} to {infoApp.EndingDate} - MISSED! \r\n \r\n";
                missedApps += finallyInfo;
            }
            MessageBox.Show(missedApps);
        }
    }
}
