using System;
using BLL;
using BLL.Interfaces;
using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Model.Entities;
using Model.Interfaces;
using Model.ModelVIewElements;
using ViewModel.ViewModels;

namespace ViewModel
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
                return ServiceLocator.Current.GetInstance<MainWindowViewModel>(Guid.NewGuid().ToString());
            }
        }

        public static void CleanUp()
        {
            SimpleIoc.Default.Unregister<AddAppWindowViewModel>();
            SimpleIoc.Default.Register<AddAppWindowViewModel>();
        }
    }

}
