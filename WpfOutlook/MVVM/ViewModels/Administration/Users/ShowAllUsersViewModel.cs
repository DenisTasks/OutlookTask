using BLL.BLLService;
using BLL.EntitesDTO;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.ViewModels.Administration.Users
{
    public class ShowAllUsersViewModel: ViewModelBase
    {
        private ObservableCollection<UserDTO> _users;
        private RelayCommand<UserDTO> _editUserCommand { get; }
        private RelayCommand _addUserCommand { get; }
        private RelayCommand<UserDTO> _deactivateUserCommand { get; }

        public ShowAllUsersViewModel()
        {
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
                if (value != _users)
                {
                    _users = value;
                    base.RaisePropertyChanged();
                }
            }
        }

        private void DeactivateUser(UserDTO user)
        {

        }

        private void AddUser()
        {
            var addAppWindow = new AddUserWindow();
            var result = addAppWindow.ShowDialog();
        }

        private void EditUser(UserDTO user)
        {
            if (user != null)
            {
                var addAppWindow = new EditUserWindow();
                var result = addAppWindow.ShowDialog();
            }
        }

        private void LoadData()
        {
            using (var bll = new BLLService())
            {
                _users = new ObservableCollection<UserDTO>(bll.GetUsers());
            }
        }


    }
}
