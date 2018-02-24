using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace TestWpf
{
    /// <summary>
    /// Interaction logic for CalendarWindow.xaml
    /// </summary>
    public partial class CalendarWindow : Window
    {
        private readonly ObservableCollection<Appointment> _test = new ObservableCollection<Appointment>();

        public CalendarWindow()
        {

            InitializeComponent();
            Appointment app1 = new Appointment()
            {
                Start = DateTime.Parse("02/19/2018 2:00 AM"),
                Finish = DateTime.Parse("02/19/2018 3:00 AM"),
                Subject = "Subject",
                Location = "Location",
                Organizer = "Organizer"
            };
            Appointment app2 = new Appointment()
            {
                Start = DateTime.Parse("02/20/2018 5:00 AM"),
                Finish = DateTime.Parse("02/20/2018 6:00 AM"),
                Subject = "Subject2",
                Location = "Location2",
                Organizer = "Organizer2"
            };
            Appointment app3 = new Appointment()
            {
                Start = DateTime.Parse("02/21/2018 6:00 AM"),
                Finish = DateTime.Parse("02/21/2018 6:30 AM"),
                Subject = "Subject3",
                Location = "Location3",
                Organizer = "Organizer3"
            };
            Appointment app4 = new Appointment()
            {
                Start = DateTime.Parse("02/22/2018 10:00 PM"),
                Finish = DateTime.Parse("02/22/2018 11:30 PM"),
                Subject = "Subject4",
                Location = "Location4",
                Organizer = "Organizer4"
            };
            _test.Add(app1);
            _test.Add(app2);
            _test.Add(app3);
            _test.Add(app4);
            Testing.ItemsSource = _test;
        }
    }
}
