using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using GalaSoft.MvvmLight.Messaging;
using TestWpf.Appointments;
using TestWpf.Calendar;
using ViewModel.Helpers;
using ViewModel.Jobs;
using ViewModel.ViewModels.Authenication;

namespace TestWpf.Pages
{
    public partial class MainWindowPage : Page
    {
        public MainWindowPage()
        {
            InitializeComponent();
            NotifyScheduler.Start();
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
                    if (message.Type == WindowType.Sync && message.User != null)
                    {
                        var addSync = new SyncWindow();
                        Messenger.Default.Send(new OpenWindowMessage() { Type = WindowType.None, User = message.User });
                        var result = addSync.ShowDialog();
                    }
                });
        }

        private void ButtonBase_Click_ToAdmin(object sender, RoutedEventArgs e)
        {
            try
            {
                this.NavigationService.Navigate(new AdminPage());
            }
            catch (System.Security.SecurityException)
            {
                MessageBox.Show("You have no rights to acces this menu" );
            }
            catch (Exception)
            {
                //MessageBox.Show(ex.ToString());
            }
        }

        private void ButtonBase_Click_ToLoginPage(object sender, RoutedEventArgs e)
        {
            CustomPrincipal customPrincipal = Thread.CurrentPrincipal as CustomPrincipal;
            customPrincipal.Identity = new AnonymousIdentity();
            Messenger.Default.Send<NotificationMessage, AuthenticationViewModel>(new NotificationMessage("LogOut"));
            this.NavigationService.GoBack();
        }
    }
}
