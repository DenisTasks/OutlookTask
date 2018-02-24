using BLL;
using BLL.Interfaces;
using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Model;
using Model.Interfaces;
using Model.ModelVIewElements;
using Ninject;
using Ninject.Modules;
using ViewModel.ViewModels;

namespace ViewModel
{
    public class DesignTimeModule : NinjectModule
    {
        public override void Load()
        {
        }
    }

    public class RunTimeModule : NinjectModule
    {
        public override void Load()
        {
            Bind<WPFOutlookContext>().ToSelf().WithConstructorArgument("connectionString", "WPFOutlookContext");
            Bind(typeof(IGenericRepository<>)).To(typeof(GenericRepository<>)).WithConstructorArgument("context", Kernel.Get<WPFOutlookContext>());
            Bind<IBLLService>().To<BLLService>();
        }
    }

    public class ViewModelLocator
    {
        public MainWindowViewModel MainWindow => _kernel.Get<MainWindowViewModel>();
        public AddAppWindowViewModel AddAppWindow => _kernel.Get<AddAppWindowViewModel>();
        public AboutAppointmentWindowViewModel AboutAppWindow => _kernel.Get<AboutAppointmentWindowViewModel>();
        public AllAppByLocationWindowViewModel AllAppByLocWindow => _kernel.Get<AllAppByLocationWindowViewModel>();
        //public CalendarView CalendarWindow => _kernel.Get<CalendarView>();

        private readonly IKernel _kernel;
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            if (ViewModelBase.IsInDesignModeStatic)
            {
                _kernel = new StandardKernel(new DesignTimeModule());
            }
            else
            {
                _kernel = new StandardKernel(new RunTimeModule());
            }
        }
    }
}
