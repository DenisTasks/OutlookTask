using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TestWpf.Controls
{
    // appointment
    public class CalendarViewPeriod : DependencyObject
    {
        public static readonly DependencyProperty BeginningDateProperty =
            DependencyProperty.Register("BeginningDate", typeof(DateTime),
                typeof(CalendarViewPeriod));
        public static readonly DependencyProperty EndingDateProperty =
            DependencyProperty.Register("EndingDate", typeof(DateTime),
                typeof(CalendarViewPeriod));
        public static readonly DependencyProperty SubjectProperty =
            DependencyProperty.Register("Subject", typeof(object),
                typeof(CalendarViewPeriod));

        public DateTime BeginningDate
        {
            get { return (DateTime)this.GetValue(BeginningDateProperty); }
            set { this.SetValue(BeginningDateProperty, value); }
        }

        public DateTime EndingDate
        {
            get { return (DateTime)this.GetValue(EndingDateProperty); }
            set { this.SetValue(EndingDateProperty, value); }
        }

        public object Subject
        {
            get { return (object)this.GetValue(SubjectProperty); }
            set { this.SetValue(SubjectProperty, value); }
        }

    }
}
