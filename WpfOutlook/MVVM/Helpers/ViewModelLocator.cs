using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using MVVM.Models;
using MVVM.Models.Authenication;
using MVVM.ViewModels.Administration;
using MVVM.ViewModels.Authenication;
using MVVM.ViewModels.TestShell;

namespace MVVM.Helpers
{
    class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            
            //if (ViewModelBase.IsInDesignModeStatic)
            //{
            //    // Create design time view services and models
            //    SimpleIoc.Default.Register<IViewModel, AuthenticationViewModel>();
            //}
            //else
            //{
            //    // Create run time view services and models
            //    SimpleIoc.Default.Register<IViewModel, AuthenticationViewModel>();
            //}

            SimpleIoc.Default.Register(()=>new AuthenticationViewModel(new AuthenticationService()));
            SimpleIoc.Default.Register(()=>new AdministrationViewModel(new AdministrationService()));
            SimpleIoc.Default.Register(() => new ShellViewModel(new AuthenticationService()));
        }
        

        public AdministrationViewModel AdminWindow
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AdministrationViewModel>();
            }
        }

        public AuthenticationViewModel LoginWindow
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AuthenticationViewModel>();
            }
        }

        public ShellViewModel Page1
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ShellViewModel>();
            }
        }

        public AdministrationViewModel Page2
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AdministrationViewModel>();
            }
        }

        public static void Cleanup()
        {
        }
    }
}
