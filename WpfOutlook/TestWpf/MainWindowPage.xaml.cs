using System.Windows.Controls;
using GalaSoft.MvvmLight.Messaging;
using ViewModel.Helpers;

namespace TestWpf
{
    public partial class MainWindowPage : Page
    {
        public MainWindowPage()
        {
            InitializeComponent();
            Messenger.Default.Register<OpenWindowMessage>(
                this,
                message => {
                    if (message.Type == WindowType.AddAppWindow)
                    {
                        var addAppWindow = new AddAppWindow();
                        var result = addAppWindow.ShowDialog();
                    }
                    if (message.Type == WindowType.AddAboutAppointmentWindow)
                    {
                        var addAboutWindow = new AboutAppWindow();
                        // send initialize information after create, but before show window!
                        // send this message => initialize new check at this cycle
                        Messenger.Default.Send(new OpenWindowMessage() {Type = WindowType.None, Argument = message.Argument, Appointment = message.Appointment });
                        var result = addAboutWindow.ShowDialog();
                    }
                    if (message.Type == WindowType.AddAllAppByLocationWindow)
                    {
                        var addAllAppWindow = new AllAppByLocation();
                        // send initialize information after create, but before show window!
                        Messenger.Default.Send(new OpenWindowMessage() { Type = WindowType.LoadLocations, Argument = message.Argument });
                        var result = addAllAppWindow.ShowDialog();
                    }
                    if (message.Type == WindowType.Calendar)
                    {
                        var addCalendarWindow = new CalendarWindow();
                        var result = addCalendarWindow.ShowDialog();
                    }
                });
        }
    }
}
