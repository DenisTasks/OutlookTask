using GalaSoft.MvvmLight.Ioc;
using CommonServiceLocator;
using ViewModel.Authentication;
using ViewModel.Interfaces;
using GalaSoft.MvvmLight;
using ViewModel.Print;

namespace TestWpf.ViewModel
{
    public class ViewModelLocator
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
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<ModalWindowViewModel>();
            SimpleIoc.Default.Register<PrintViewModel>();
        }

        public AuthenticationViewModel LoginWindow
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AuthenticationViewModel>();
            }
        }

        public MainViewModel MainWindow
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public PrintViewModel PrintWindow
        {
            get
            {
                return ServiceLocator.Current.GetInstance<PrintViewModel>();
            }
        }

        public static void Cleanup()
        {
        }
    }
}