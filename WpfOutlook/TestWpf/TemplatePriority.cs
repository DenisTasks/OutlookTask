using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using BLL.DTO;
using ViewModel.Models;

namespace TestWpf
{
    public class TemplatePriority : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;

            if (element != null && item != null && item is AppointmentModel)
            {
                AppointmentModel taskitem = item as AppointmentModel;

                if (taskitem.Subject != null)
                { return
                        element.FindResource("TestTemplate") as DataTemplate;}
                
            }

            return null;
        }
    }
}
