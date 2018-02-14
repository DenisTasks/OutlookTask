using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ViewModel.Authentication;

namespace TestWpf
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {

            //Create a custom principal with an anonymous identity at startup
            CustomPrincipal customPrincipal = new CustomPrincipal();
            AppDomain.CurrentDomain.SetThreadPrincipal(customPrincipal);

            base.OnStartup(e);

            //Show the login view
            AuthenticationViewModel viewModel = new AuthenticationViewModel();
            IView loginWindow = new LoginWindow(viewModel);
            loginWindow.Show();

        }
    }
}
