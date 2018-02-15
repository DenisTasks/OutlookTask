using System;
using System.Windows;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using TestWpf.Helpers;
using TestWpf.ViewModel;
using ViewModel.Authentication;
using ViewModel.Interfaces;

namespace TestWpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {

            CustomPrincipal customPrincipal = new CustomPrincipal();
            AppDomain.CurrentDomain.SetThreadPrincipal(customPrincipal);

            base.OnStartup(e);
            
            //AuthenticationViewModel viewModel = new AuthenticationViewModel();
            //IView loginWindow = new LoginWindow(viewModel);
            //loginWindow.Show();
        }

        static App()
        {
            DispatcherHelper.Initialize();
        }
    }
}
