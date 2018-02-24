using System;
using System.Windows;

namespace TestWpf.Controls
{
    public class CalendarViewPeriod : DependencyObject
    {
        public static readonly DependencyProperty BeginningDateProperty =
            DependencyProperty.Register("BeginningDate", typeof(DateTime),
                typeof(CalendarViewPeriod));
        public static readonly DependencyProperty EndingDateProperty =
            DependencyProperty.Register("EndingDate", typeof(DateTime),
                typeof(CalendarViewPeriod));
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(object),
                typeof(CalendarViewPeriod));

        public DateTime BeginningDate
        {
            get => (DateTime)GetValue(BeginningDateProperty);
            set => SetValue(BeginningDateProperty, value);
        }

        public DateTime EndingDate
        {
            get => (DateTime)GetValue(EndingDateProperty);
            set => SetValue(EndingDateProperty, value);
        }

        public object Header
        {
            get => GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

    }
}
