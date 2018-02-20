﻿using BLL.EntitesDTO;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using MVVM.Helpers;
using MVVM.Interfaces;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows.Controls;

namespace MVVM.ViewModels.Authenication
{
    public class AuthenticationViewModel : ViewModelBase,  INotifyPropertyChanged
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly DelegateCommand _loginCommand;
        private readonly DelegateCommand _logoutCommand;
        private string _username;
        private string _status;

        public AuthenticationViewModel(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            _loginCommand = new DelegateCommand(Login, CanLogin);
            _logoutCommand = new DelegateCommand(Logout, CanLogout);
        }

        public DelegateCommand LoginCommand { get { return _loginCommand; } }

        public DelegateCommand LogoutCommand { get { return _logoutCommand; } }


        public string Username
        {
            get { return _username; }
            set { _username = value; NotifyPropertyChanged("Username"); }
        }

        public string Status
        {
            get { return _status; }
            set { _status = value; NotifyPropertyChanged("Status"); }
        }

        public string AuthenticatedUser
        {
            get
            {
                if (IsAuthenticated)
                {
                    Messenger.Default.Send(new NotificationMessage("LoginSuccess"));
                    return string.Format("Signed in as {0}", Thread.CurrentPrincipal.Identity.Name);

                }
                else return "You are not authenticated!";
            }
        }


        private void Login(object parameter)
        {
            PasswordBox passwordBox = parameter as PasswordBox;
            string clearTextPassword = passwordBox.Password;
            try
            {
                UserDTO user = _authenticationService.AuthenticateUser(Username, clearTextPassword);
                CustomPrincipal customPrincipal = Thread.CurrentPrincipal as CustomPrincipal;
                if (customPrincipal == null)
                    throw new ArgumentException("The application's default thread principal must be set to a CustomPrincipal object on startup.");
                customPrincipal.Identity = new CustomIdentity(user.UserName, user.Name, user.Roles.Select(r => r.Name).ToArray());


                _loginCommand.RaiseCanExecuteChanged();
                _logoutCommand.RaiseCanExecuteChanged();
                NotifyPropertyChanged("IsAuthenticated");
                NotifyPropertyChanged("AuthenticatedUser");

            }
            catch (UnauthorizedAccessException)
            {
                Status = "Login failed! Please provide some valid credentials.";
                NotifyPropertyChanged("Status");
            }
            catch (Exception ex)
            {
                Status = string.Format("ERROR: {0}", ex.Message);
                NotifyPropertyChanged("Status");
            }
        }

        private bool CanLogin(object parameter)
        {
            return !IsAuthenticated;
        }

        public bool IsAuthenticated
        {
            get { return Thread.CurrentPrincipal.Identity.IsAuthenticated; }
        }

        private void Logout(object parameter)
        {
            CustomPrincipal customPrincipal = Thread.CurrentPrincipal as CustomPrincipal;
            if (customPrincipal != null)
            {
                customPrincipal.Identity = new AnonymousIdentity();
                NotifyPropertyChanged("AuthenticatedUser");
                NotifyPropertyChanged("IsAuthenticated");
                _loginCommand.RaiseCanExecuteChanged();
                _logoutCommand.RaiseCanExecuteChanged();
                Status = string.Empty;
            }
        }


        private bool CanLogout(object parameter)
        {
            return IsAuthenticated;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
