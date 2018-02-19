using System.Windows;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using ViewModel.Helpers;
using ViewModel.ViewModels;

namespace TestWpf
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Messenger.Default.Register<OpenWindowMessage>(
                this,
                message => {
                    if (message.Type == WindowType.AddAppWindow && message.Argument != "Load this appointment")
                    {
                        var addAppWindowVM = SimpleIoc.Default.GetInstance<AddAppWindowViewModel>();
                        var addAppWindow = new AddAppWindow()
                        {
                            DataContext = addAppWindowVM
                        };
                        var result = addAppWindow.ShowDialog();
                    }
                    if (message.Type == WindowType.AddAboutAppointmentWindow)
                    {
                        var addAboutWindowVM = SimpleIoc.Default.GetInstance<AboutAppointmentWindowViewModel>();
                        var addAboutWindow = new AboutAppWindow()
                        {
                            DataContext = addAboutWindowVM
                        };
                        Messenger.Default.Send(new OpenWindowMessage() { Argument = message.Argument, Appointment = message.Appointment });
                        var result = addAboutWindow.ShowDialog();
                    }
                    if (message.Type == WindowType.None)
                    {
                        Messenger.Default.Send(new OpenWindowMessage() { Type = WindowType.Refresh });
                    }
                    if (message.Type == WindowType.AddAllAppByLocationWindow)
                    {
                        var addAllAppWindowVM = SimpleIoc.Default.GetInstance<AllAppByLocationWindowViewModel>();
                        var addAllAppWindow = new AllAppByLocation()
                        {
                            DataContext = addAllAppWindowVM
                        };
                        Messenger.Default.Send(new OpenWindowMessage() { Type = WindowType.LoadLocations, Argument = message.Argument });
                        var result = addAllAppWindow.ShowDialog();
                    }
                });
        }
    }
}
