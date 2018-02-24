﻿using System;
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
        public static DependencyProperty BeginDateProperty = DependencyProperty.RegisterAttached("BeginDate", typeof(DateTime), typeof(ListViewItem));
        public static DependencyProperty EndDateProperty = DependencyProperty.RegisterAttached("EndDate", typeof(DateTime), typeof(ListViewItem));

        private CalendarViewPeriodCollection _periods;

        public BindingBase ItemBeginDateBinding { get; set; }

        public BindingBase ItemEndDateBinding { get; set; }

        public CalendarViewPeriodCollection Periods
        {
            get
            {
                if (_periods == null)
                    _periods = new CalendarViewPeriodCollection()
                    {
                        new CalendarViewPeriod() {BeginDate = DateTime.Parse("02/19/2018 12:00 AM"), EndDate = DateTime.Parse("02/19/2018 11:30 PM"), Header = "Monday"},
                        new CalendarViewPeriod() {BeginDate = DateTime.Parse("02/20/2018 12:00 AM"), EndDate = DateTime.Parse("02/20/2018 11:30 PM"), Header = "Tuesday"},
                        new CalendarViewPeriod() {BeginDate = DateTime.Parse("02/21/2018 12:00 AM"), EndDate = DateTime.Parse("02/21/2018 11:30 PM"), Header = "Wednesday"},
                        new CalendarViewPeriod() {BeginDate = DateTime.Parse("02/22/2018 12:00 AM"), EndDate = DateTime.Parse("02/22/2018 11:30 PM"), Header = "Thursday"},
                        new CalendarViewPeriod() {BeginDate = DateTime.Parse("02/23/2018 12:00 AM"), EndDate = DateTime.Parse("02/23/2018 11:30 PM"), Header = "Friday"},
                    };
                return _periods;
            }
        }

        protected override object DefaultStyleKey => new ComponentResourceKey(GetType(), "DefaultStyleKey");

        protected override object ItemContainerDefaultStyleKey => new ComponentResourceKey(GetType(), "ItemContainerDefaultStyleKey");

        public static DateTime GetBegin(DependencyObject item)
        {
            return (DateTime)item.GetValue(BeginDateProperty);
        }

        public static DateTime GetEnd(DependencyObject item)
        {
            return (DateTime)item.GetValue(EndDateProperty);
        }

        public static void SetBegin(DependencyObject item, DateTime value)
        {
            item.SetValue(BeginDateProperty, value);
        }

        public static void SetEnd(DependencyObject item, DateTime value)
        {
            item.SetValue(EndDateProperty, value);
        }

        protected override void PrepareItem(ListViewItem item)
        {
            item.SetBinding(BeginDateProperty, ItemBeginDateBinding);
            item.SetBinding(EndDateProperty, ItemEndDateBinding);
        }

        public bool PeriodContainsItem(ListViewItem item, CalendarViewPeriod period)
        {
            DateTime itemBegin = (DateTime)item.GetValue(BeginDateProperty);
            DateTime itemEnd = (DateTime)item.GetValue(EndDateProperty);

            return (((itemBegin <= period.BeginDate) && (itemEnd >= period.BeginDate)) || ((itemBegin <= period.EndDate) && (itemEnd >= period.BeginDate)));
        }
    }
}
