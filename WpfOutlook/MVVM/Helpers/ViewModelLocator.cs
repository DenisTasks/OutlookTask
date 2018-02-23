using BLL.Interfaces;
using BLL.Services;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using MVVM.ViewModels.Administration;
using MVVM.ViewModels.Administration.Users;
using MVVM.ViewModels.Authenication;
using MVVM.ViewModels.CommonViewModels.Groups;
using System;

namespace MVVM.Helpers
{
    class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            //if (ViewModelBase.IsInDesignModeStatic)
            //{
            //     Create design time view services and models
            //    SimpleIoc.Default.Register<IAuthenticationService, AuthenticationService>();
            //    SimpleIoc.Default.Register<IAdministrationService, AdministrationService>();
            //}
            //else
            //{
            //     Create run time view services and models
            //    SimpleIoc.Default.Register<IAuthenticationService, AuthenticationViewModel>();
            //}

            SimpleIoc.Default.Register<IAuthenticationService, AuthenticationService>();
            SimpleIoc.Default.Register<IAdministrationService, AdministrationService>();

            SimpleIoc.Default.Register<AdministrationViewModel>();
            SimpleIoc.Default.Register<AuthenticationViewModel>();
            SimpleIoc.Default.Register<EditUserViewModel>();
            SimpleIoc.Default.Register<AddUserViewModel>();
            SimpleIoc.Default.Register<ShowAllUsersViewModel>();
            SimpleIoc.Default.Register<EditUserViewModel>();
            SimpleIoc.Default.Register<ShowAllGroupsViewModel>();
            SimpleIoc.Default.Register<AddGroupViewModel>();
        }
        

      

        public AuthenticationViewModel LoginPage
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AuthenticationViewModel>(Guid.NewGuid().ToString());
            }
        }

        public AdministrationViewModel AdminPage
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AdministrationViewModel>(Guid.NewGuid().ToString());
            }
        }

        public ShowAllUsersViewModel AllUsersPage
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ShowAllUsersViewModel>(Guid.NewGuid().ToString());
            }
        }

        public ShowAllGroupsViewModel AllGroupsPage
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ShowAllGroupsViewModel>(Guid.NewGuid().ToString());
            }
        }

        public AddUserViewModel AddUserWindow
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AddUserViewModel>(Guid.NewGuid().ToString());
            }
        }

        public EditUserViewModel EditUserWindow
        {
            get
            {
                return ServiceLocator.Current.GetInstance<EditUserViewModel>(Guid.NewGuid().ToString());
            }
        }

        public AddGroupViewModel AddGroupWindow
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AddGroupViewModel>(Guid.NewGuid().ToString());
            }
        }

        public static void CleanUp()
        {
        }
    }
}
