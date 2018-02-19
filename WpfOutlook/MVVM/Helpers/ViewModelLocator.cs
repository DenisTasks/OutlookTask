using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using MVVM.Interfaces;
using MVVM.Models;
using MVVM.ViewModels.Administration;
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

            SimpleIoc.Default.Register<AuthenticationViewModel>();
            SimpleIoc.Default.Register(()=>new AdministrationViewModel(new AdministrationService()));
        }
        

        public AdministrationViewModel AdminWindow
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
