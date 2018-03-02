using BLL.EntitesDTO;
using BLL.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Collections.ObjectModel;
using System.Windows;

namespace ViewModel.ViewModels.Administration.Users
{
    public class AddUserViewModel : ViewModelBase
    {
        private readonly IAdministrationService _administrationService;

        private ObservableCollection<RoleDTO> _roleList;
        private ObservableCollection<RoleDTO> _selectedRoleList;

        private ObservableCollection<GroupDTO> _groupList;
        private ObservableCollection<GroupDTO> _selectedGroupList;

        public UserDTO User { get; set; }

        public AddUserViewModel(IAdministrationService administrationService)
        {
            _administrationService = administrationService;
            _roleList = new ObservableCollection<RoleDTO>(_administrationService.GetRoles());
            _selectedRoleList = new ObservableCollection<RoleDTO>();

            _groupList = new ObservableCollection<GroupDTO>(_administrationService.GetGroups());
            _selectedGroupList = new ObservableCollection<GroupDTO>();

            _addRoleCommand = new RelayCommand<RoleDTO>(AddRole);
            _removeRoleCommand = new RelayCommand<RoleDTO>(RemoveRole);
            _addGroupCommand = new RelayCommand<GroupDTO>(AddGroup);
            _removeGroupCommand = new RelayCommand<GroupDTO>(RemoveGroup);
            _createUserCommand = new RelayCommand<Window>(CreateUser);

            User = new UserDTO();
        }

        public ObservableCollection<RoleDTO> RoleList
        {
            get => _roleList;
            set
            {
                if (value!= _roleList)
                {
                    _roleList = value;
                    base.RaisePropertyChanged();
                }
            }
        }

        public ObservableCollection<RoleDTO> SelectedRoleList
        {
            get => _selectedRoleList;
            set
            {
                if (value != _selectedRoleList)
                {
                    _selectedRoleList = value;
                    base.RaisePropertyChanged();
                }
            }
        }

        public ObservableCollection<GroupDTO> SelectedGroupList
        {
            get => _selectedGroupList;
            set
            {
                if (value != _selectedGroupList)
                {
                    _selectedGroupList = value;
                    base.RaisePropertyChanged();
                }
            }
        }

        public ObservableCollection<GroupDTO> GroupList
        {
            get => _groupList;
            set
            {
                if (value != _groupList)
                {
                    _groupList = value;
                    base.RaisePropertyChanged();
                }
            }
        }
        
        private RelayCommand<RoleDTO> _addRoleCommand;
        private RelayCommand<RoleDTO> _removeRoleCommand;

        public RelayCommand<RoleDTO> AddRoleCommand { get { return _addRoleCommand; } }
        public RelayCommand<RoleDTO> RemoveRoleCommand { get { return _removeRoleCommand; } }

        public void AddRole(RoleDTO role)
        {
            SelectedRoleList.Add(role);
            RoleList.Remove(role);
            base.RaisePropertyChanged();
        }

        public void RemoveRole(RoleDTO role)
        {
            RoleList.Add(role);
            SelectedRoleList.Remove(role);
            base.RaisePropertyChanged();
        }


        private RelayCommand<GroupDTO> _addGroupCommand;
        private RelayCommand<GroupDTO> _removeGroupCommand;

        public RelayCommand<GroupDTO> AddGroupCommand { get { return _addGroupCommand; } }
        public RelayCommand<GroupDTO> RemoveGroupCommand { get { return _removeGroupCommand; } }

        public void AddGroup(GroupDTO group)
        {
            SelectedGroupList.Add(group);
            GroupList.Remove(group);
            base.RaisePropertyChanged();
        }

        public void RemoveGroup(GroupDTO group)
        {
            GroupList.Add(group);
            SelectedGroupList.Remove(group);
            base.RaisePropertyChanged();
        }

        private RelayCommand<Window> _createUserCommand;

        public RelayCommand<Window> CreateUserCommand { get { return _createUserCommand; } }

        public void CreateUser(Window window)
        {
            if (User.UserName != null)
            {
                if (_administrationService.CheckUser(User.UserName))
                {
                    _administrationService.CreateUser(User , SelectedGroupList, SelectedRoleList);
                    window.Close();
                }
                else
                {
                    MessageBox.Show("User with that username already exists!");
                }
            } else
            {
                MessageBox.Show("Fill empty fields!");
            }
        }
    }
}
