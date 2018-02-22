using BLL;
using BLL.Interfaces;
using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Model;
using Model.Entities;
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
            //Bind<IBLLService>().To<BLLService>();
            Bind(typeof(IGenericRepository<>)).To(typeof(GenericRepository<>));
            Bind<WPFOutlookContext>().ToSelf();
            Bind<IBLLService>().ToMethod(ctx =>
            {
                var context = new WPFOutlookContext();
                return new BLLService(new GenericRepository<Appointment>(context), new GenericRepository<User>(context), new GenericRepository<Location>(context));
            });
        }
    }

    public class RunTimeModule : NinjectModule
    {
        public override void Load()
        {
            //Bind<IBLLService>().To<BLLService>();
            Bind(typeof(IGenericRepository<>)).To(typeof(GenericRepository<>));
            Bind<WPFOutlookContext>().ToSelf();
            Bind<IBLLService>().ToMethod(ctx =>
            {
                var context = new WPFOutlookContext();
                return new BLLService(new GenericRepository<Appointment>(context), new GenericRepository<User>(context), new GenericRepository<Location>(context));
            });
        }
    }

    public class ViewModelLocator
    {
        private IKernel kernel;

        public ViewModelLocator()
        {

            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            if (ViewModelBase.IsInDesignModeStatic)
            {
                kernel = new StandardKernel(new DesignTimeModule());

                //SimpleIoc.Default.Register<IBLLService, BLLService>();
                //SimpleIoc.Default.Register<IGenericRepository<Appointment>, GenericRepository<Appointment>>();
                //SimpleIoc.Default.Register<IGenericRepository<User>, GenericRepository<User>>();
                //SimpleIoc.Default.Register<IGenericRepository<Location>, GenericRepository<Location>>();

            }
            else
            {
                kernel = new StandardKernel(new RunTimeModule());

                //SimpleIoc.Default.Register<IBLLService, BLLService>();
                //SimpleIoc.Default.Register<IGenericRepository<Appointment>, GenericRepository<Appointment>>();
                //SimpleIoc.Default.Register<IGenericRepository<User>, GenericRepository<User>>();
                //SimpleIoc.Default.Register<IGenericRepository<Location>, GenericRepository<Location>>();

            }
            // Register my view models
            //SimpleIoc.Default.Register<MainWindowViewModel>();
            //SimpleIoc.Default.Register<AddAppWindowViewModel>();
            //SimpleIoc.Default.Register<AboutAppointmentWindowViewModel>();
            //SimpleIoc.Default.Register<AllAppByLocationWindowViewModel>();
        }

        public MainWindowViewModel MainWindow => kernel.Get<MainWindowViewModel>();

        public AddAppWindowViewModel AddAppWindow => kernel.Get<AddAppWindowViewModel>();

        public AboutAppointmentWindowViewModel AboutAppWindow => kernel.Get<AboutAppointmentWindowViewModel>();

        public AllAppByLocationWindowViewModel AllAppByLocWindow => kernel.Get<AllAppByLocationWindowViewModel>();

    }
}
