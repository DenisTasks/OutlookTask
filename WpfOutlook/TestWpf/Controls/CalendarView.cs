using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace TestWpf.Controls
{
    public class CalendarView : ViewBase
    {
        private CalendarViewPeriodCollection periods;

        public BindingBase ItemBeginningDateBinding { get; set; }

        public BindingBase ItemEndingDateBinding { get; set; }

        public CalendarViewPeriodCollection Periods
        {
            get
            {
                if (periods == null)
                    periods = new CalendarViewPeriodCollection();

                return periods;
            }
        }

        protected override object DefaultStyleKey
        {
            get { return new ComponentResourceKey(GetType(), "DefaultStyleKey"); }
        }

        protected override object ItemContainerDefaultStyleKey
        {
            get { return new ComponentResourceKey(this.GetType(), "ItemContainerDefaultStyleKey"); }
        }
    }
}
