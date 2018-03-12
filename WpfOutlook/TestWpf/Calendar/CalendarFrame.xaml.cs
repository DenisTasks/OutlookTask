using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using GalaSoft.MvvmLight.Messaging;
using TestWpf.Pages;

namespace TestWpf.Calendar
{
    /// <summary>
    /// Interaction logic for CalendarFrame.xaml
    /// </summary>
    public partial class CalendarFrame : Window
    {
        public CalendarFrame()
        {
            InitializeComponent();
            int start = 0;
            CalendarPage calendar = new CalendarPage();
            mainCalendarFrame.NavigationService.LoadCompleted += calendar.NavigationService_LoadCompleted;
            mainCalendarFrame.NavigationService.Navigate(calendar, start);
        }
    }
}
