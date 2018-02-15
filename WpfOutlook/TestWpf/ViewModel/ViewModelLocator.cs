using GalaSoft.MvvmLight.Ioc;
using CommonServiceLocator;
using GalaSoft.MvvmLight;
using Model.Interfaces;
using Model.ModelVIewElements;
using ViewModel.Helpers;
using ViewModel.Interfaces;

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

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<AddAppWindowViewModel>();
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }
    }
}