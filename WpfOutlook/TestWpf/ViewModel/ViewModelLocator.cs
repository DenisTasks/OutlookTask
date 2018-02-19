using GalaSoft.MvvmLight.Ioc;
using CommonServiceLocator;
using GalaSoft.MvvmLight;
using Model.Interfaces;
using Model.ModelVIewElements;
using BLL.Interfaces;
using BLL;

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
            }
            else
            {
                // Create run time view services and models
                SimpleIoc.Default.Register<IUnitOfWork, UnitOfWork>();
                SimpleIoc.Default.Register<IBLLService, BLLService>();
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