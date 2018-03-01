using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using BLL.DTO;

namespace TestWpf
{
    public class CustomStyleSelector : StyleSelector
    {

        public override Style SelectStyle(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;

            if (element != null && item != null && item is AppointmentDTO)
            {
                AppointmentDTO taskitem = item as AppointmentDTO;

                if (taskitem.Subject != null)
                {
                    return Application.Current.Resources.FindName("OverrideStyleListViewItem") as Style;
                }
                else
                {
                    return Application.Current.Resources.FindName("OverrideStyleListViewItem") as Style;
                }
            }

            return null;
        }

    }
}
