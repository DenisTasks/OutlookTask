using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace TestWpf
{
    public class CalendarView : ViewBase
    {
        private CalendarViewPeriodCollection _periods =
            new CalendarViewPeriodCollection();

        public CalendarViewPeriodCollection Periods
        {
            get { return _periods; }
        }
    }

    public class CalendarViewPeriod : DependencyObject
    {
        public static readonly DependencyProperty BeginProperty =
            DependencyProperty.Register("Begin", typeof(DateTime),
                typeof(CalendarViewPeriod));
        public static readonly DependencyProperty EndProperty =
            DependencyProperty.Register("End", typeof(DateTime),
                typeof(CalendarViewPeriod));
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(object),
                typeof(CalendarViewPeriod));

        public DateTime Begin
        {
            get { return (DateTime)this.GetValue(BeginProperty); }
            set { this.SetValue(BeginProperty, value); }
        }

        public DateTime End
        {
            get { return (DateTime)this.GetValue(EndProperty); }
            set { this.SetValue(EndProperty, value); }
        }

        public object Header
        {
            get { return (object)this.GetValue(HeaderProperty); }
            set { this.SetValue(HeaderProperty, value); }
        }

    }

    public class CalendarViewPeriodCollection : ObservableCollection<CalendarViewPeriod>
    {
    }
}
