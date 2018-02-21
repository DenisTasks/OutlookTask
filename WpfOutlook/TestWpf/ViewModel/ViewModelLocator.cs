using GalaSoft.MvvmLight.Ioc;
using CommonServiceLocator;
using GalaSoft.MvvmLight;
using Model.Interfaces;
using Model.ModelVIewElements;
using BLL.Interfaces;
using BLL;
using ViewModel.ViewModels;
using Model.Entities;

namespace TestWpf.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            if (ViewModelBase.IsInDesignModeStatic)
            {
                // Create design time view services and models
                SimpleIoc.Default.Register<IUnitOfWork, UnitOfWork>();
                SimpleIoc.Default.Register<IBLLService, BLLService>();
                SimpleIoc.Default.Register<ITestInterface<Appointment>, TestGeneric<Appointment>>();
                SimpleIoc.Default.Register<ITestInterface<User>, TestGeneric<User>>();
                SimpleIoc.Default.Register<ITestInterface<Location>, TestGeneric<Location>>();
            }
            else
            {
                // Create run time view services and models
                SimpleIoc.Default.Register<IUnitOfWork, UnitOfWork>();
                SimpleIoc.Default.Register<IBLLService, BLLService>();
                SimpleIoc.Default.Register<ITestInterface<Appointment>, TestGeneric<Appointment>>();
                SimpleIoc.Default.Register<ITestInterface<User>, TestGeneric<User>>();
                SimpleIoc.Default.Register<ITestInterface<Location>, TestGeneric<Location>>();
            }
            // Register my view models
            SimpleIoc.Default.Register<MainWindowViewModel>();
            SimpleIoc.Default.Register<AddAppWindowViewModel>();
            SimpleIoc.Default.Register<AboutAppointmentWindowViewModel>();
            SimpleIoc.Default.Register<AllAppByLocationWindowViewModel>();
        }

        public MainWindowViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainWindowViewModel>();
            }
        }
    }
}