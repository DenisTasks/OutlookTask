using BLL.Interfaces;
using BLL.Services;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using MVVM.ViewModels.Administration;
using MVVM.ViewModels.Administration.Users;
using MVVM.ViewModels.Authenication;

namespace MVVM.Helpers
{
    class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            //if (ViewModelBase.IsInDesignModeStatic)
            //{
            //     Create design time view services and models
            //    SimpleIoc.Default.Register<IAuthenticationService, AuthenticationService>();
            //    SimpleIoc.Default.Register<IAdministrationService, AdministrationService>();
            //}
            //else
            //{
            //     Create run time view services and models
            //    SimpleIoc.Default.Register<IAuthenticationService, AuthenticationViewModel>();
            //}

            SimpleIoc.Default.Register<IAuthenticationService, AuthenticationService>();
            SimpleIoc.Default.Register<IAdministrationService, AdministrationService>();

            SimpleIoc.Default.Register<AdministrationViewModel>();
            SimpleIoc.Default.Register<AuthenticationViewModel>();
            SimpleIoc.Default.Register<EditUserViewModel>();
            SimpleIoc.Default.Register<AddUserViewModel>();
            SimpleIoc.Default.Register<ShowAllUsersViewModel>();
        }
        

      

        public AuthenticationViewModel LoginPage
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AuthenticationViewModel>();
            }
        }

        public AdministrationViewModel AdminPage
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AdministrationViewModel>();
            }
        }

        public ShowAllUsersViewModel AllUsersPage
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ShowAllUsersViewModel>();
            }
        }

        public AddUserViewModel AddUserWindow
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AddUserViewModel>();
            }
        }

        public EditUserViewModel EditUserWindow
        {
            get
            {
                return ServiceLocator.Current.GetInstance<EditUserViewModel>();
            }
        }

        public static void Cleanup()
        {
        }
    }
}
