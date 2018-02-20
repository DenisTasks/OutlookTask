using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using MVVM.Services;
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
            SimpleIoc.Default.Register(() => new AddUserViewModel());
            SimpleIoc.Default.Register(() => new ShowAllUsersViewModel());
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

        public static void Cleanup()
        {
        }
    }
}
