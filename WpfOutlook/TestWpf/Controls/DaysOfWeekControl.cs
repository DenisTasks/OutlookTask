using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TestWpf.Controls
{
    public class DaysOfWeekControl : Control
    {
        public DaysOfWeekControl()
        {
            this.DefaultStyleKey = typeof(DaysOfWeekControl);
        }

        protected CalendarView _calendarView => ListView.View as CalendarView;

        protected ListView ListView => TemplatedParent as ListView;

        public void test()
        {
            _calendarView.Periods.Count.ToString();
        }
    }
}
