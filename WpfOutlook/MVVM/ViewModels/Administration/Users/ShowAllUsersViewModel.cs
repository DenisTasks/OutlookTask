using BLL.BLLService;
using BLL.EntitesDTO;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
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
        }

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

        private void LoadData()
        {
            using (var bll = new BLLService())
            {
                _users = new ObservableCollection<UserDTO>(bll.GetUsers());
            }
        }


    }
}
