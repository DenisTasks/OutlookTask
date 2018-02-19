using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using MVVM.ViewModel.Authenication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<ModalWindowViewModel>();
            SimpleIoc.Default.Register<PrintViewModel>();
            SimpleIoc.Default.Register<PrintAppointmentViewModel>();
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

        public PrintAppointmentViewModel AppointmentWindow
        {
            get
            {
                return ServiceLocator.Current.GetInstance<PrintAppointmentViewModel>();
            }
        }

        public static void Cleanup()
        {
        }
    }
}
