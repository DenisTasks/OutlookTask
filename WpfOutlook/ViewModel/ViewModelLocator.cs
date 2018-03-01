﻿using BLL.BLLService;
using BLL.Interfaces;
using GalaSoft.MvvmLight;
using Model;
using Model.Interfaces;
using Model.ModelService;
using Ninject;
using Ninject.Modules;
using ViewModel.ViewModels.Appointments;
using ViewModel.ViewModels.Calendar;

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
            Bind<IBLLServiceMain>().To<BLLServiceMain>();
        }
    }

    public class ViewModelLocator
    {
        private readonly IKernel _kernel;

        public MainWindowViewModel MainWindow => _kernel.Get<MainWindowViewModel>();
        public AddAppWindowViewModel AddAppWindow => _kernel.Get<AddAppWindowViewModel>();
        public AboutAppointmentWindowViewModel AboutAppWindow => _kernel.Get<AboutAppointmentWindowViewModel>();
        public AllAppByLocationWindowViewModel AllAppByLocWindow => _kernel.Get<AllAppByLocationWindowViewModel>();
        public CalendarWindowViewModel CalendarWindow => _kernel.Get<CalendarWindowViewModel>();
        public ToastListViewModel ToastWindow => _kernel.Get<ToastListViewModel>();
        public SyncWindowViewModel SyncWindow => _kernel.Get<SyncWindowViewModel>();

        public ViewModelLocator()
        {
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
