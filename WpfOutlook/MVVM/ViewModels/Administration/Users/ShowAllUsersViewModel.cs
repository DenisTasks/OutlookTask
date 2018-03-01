using BLL.BLLService;
using BLL.EntitesDTO;
using BLL.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.ViewModels.Administration.Users
{
    public class ShowAllUsersViewModel: ViewModelBase
    {
        private readonly IAdministrationService _administrationService;
        private ObservableCollection<UserDTO> _users;
        private RelayCommand<UserDTO> _editUserCommand { get; }
        private RelayCommand _addUserCommand { get; }
        private RelayCommand<UserDTO> _deactivateUserCommand { get; }

        public ShowAllUsersViewModel(IAdministrationService administrationService)
        {
            _administrationService = administrationService;
            LoadData();
            _editUserCommand = new RelayCommand<UserDTO>(EditUser);
            _addUserCommand = new RelayCommand(AddUser);
            _deactivateUserCommand = new RelayCommand<UserDTO>(DeactivateUser);
        }

        public RelayCommand<UserDTO> EditUserCommand { get { return _editUserCommand; } }

        public RelayCommand AddUserCommand { get { return _addUserCommand; } }

        public RelayCommand<UserDTO> DeactivateUserCommand { get { return _deactivateUserCommand; } }

        public ObservableCollection<UserDTO> Users
        {
            get => _users;
            set
            {
                if (value != null)
                {
                    _users = value;
                    base.RaisePropertyChanged();
                }
            }
        }

        private void DeactivateUser(UserDTO user)
        {
            _administrationService.DeactivateUser(user.UserId);
            if (user.IsActive == true)
            {
                user.IsActive = false;
            }
            else
            {
                user.IsActive = true;
            }
            Users.Add(user);
            Users.Remove(Users.Last());
            Users = new ObservableCollection<UserDTO>(Users.OrderBy(s => s.UserId));
        }

        private void AddUser()
        {
            var addUserWindow = new AddUserWindow();
            var result = addUserWindow.ShowDialog();
            LoadData();
            Users = _users;
        }

        private void EditUser(UserDTO user)
        {
            if (user != null)
            {
                var editUserWindow = new EditUserWindow();
                Messenger.Default.Send<UserDTO, EditUserViewModel>(user);
                var result = editUserWindow.ShowDialog();
                LoadData();
                Users = _users;
            }
        }

        private void LoadData()
        {
            using (var bll = new BLLService())
            {
                _users = new ObservableCollection<UserDTO>(bll.GetUsers());
            }
        }

        //public event PropertyChangedEventHandler PropertyChanged;

        //private void NotifyPropertyChanged(string propertyName)
        //{
        //    if (PropertyChanged != null)
        //        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //}
    }
}
