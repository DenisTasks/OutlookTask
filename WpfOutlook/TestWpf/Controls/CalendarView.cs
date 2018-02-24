using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace TestWpf.Controls
{
    [TemplatePart(Name="PART_Header", Type=typeof(TextBlock))]
    [TemplatePart(Name="PART_TimeScale", Type=typeof(TextBlock))]
    [TemplatePart(Name="PART_ContentPresenter", Type=typeof(TextBlock))]
    public class CalendarView : ViewBase
    {
        public static DependencyProperty BeginningDateProperty = DependencyProperty.RegisterAttached("BeginningDate", typeof(DateTime), typeof(ListViewItem));
        public static DependencyProperty EndingDateProperty = DependencyProperty.RegisterAttached("EndingDate", typeof(DateTime), typeof(ListViewItem));

        private CalendarViewPeriodCollection _periods;

        public BindingBase ItemBeginningDateBinding { get; set; }

        public BindingBase ItemEndingDateBinding { get; set; }

        public CalendarViewPeriodCollection Periods
        {
            get
            {
                if (_periods == null)
                    _periods = new CalendarViewPeriodCollection();

                return _periods;
            }
        }

        protected override object DefaultStyleKey => new ComponentResourceKey(GetType(), "DefaultStyleKey");

        protected override object ItemContainerDefaultStyleKey => new ComponentResourceKey(GetType(), "ItemContainerDefaultStyleKey");

        public static DateTime GetBegin(DependencyObject item)
        {
            return (DateTime)item.GetValue(BeginningDateProperty);
        }

        public static DateTime GetEnd(DependencyObject item)
        {
            return (DateTime)item.GetValue(EndingDateProperty);
        }

        public static void SetBegin(DependencyObject item, DateTime value)
        {
            item.SetValue(BeginningDateProperty, value);
        }

        public static void SetEnd(DependencyObject item, DateTime value)
        {
            item.SetValue(EndingDateProperty, value);
        }

        protected override void PrepareItem(ListViewItem item)
        {
            item.SetBinding(BeginningDateProperty, ItemBeginningDateBinding);
            item.SetBinding(EndingDateProperty, ItemEndingDateBinding);
        }

        public bool PeriodContainsItem(ListViewItem item, CalendarViewPeriod period)
        {
            DateTime itemBegin = (DateTime)item.GetValue(BeginningDateProperty);
            DateTime itemEnd = (DateTime)item.GetValue(EndingDateProperty);

            return (((itemBegin <= period.BeginningDate) && (itemEnd >= period.BeginningDate)) || ((itemBegin <= period.EndingDate) && (itemEnd >= period.BeginningDate)));
        }
    }
}
