using BLL.EntitesDTO;
using BLL.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MVVM.ViewModels.Administration.Users
{
    public class EditUserViewModel : ViewModelBase
    {
        private readonly IAdministrationService _administrationService;

        private UserDTO _user;

        private ObservableCollection<RoleDTO> _roleList;
        private ObservableCollection<RoleDTO> _selectedRoleList;

        private ObservableCollection<GroupDTO> _groupList;
        private ObservableCollection<GroupDTO> _selectedGroupList;

        public UserDTO User
        {
            get => _user;
            set
            {
                if (value != null)
                {
                    _user = value; base.RaisePropertyChanged();
                }
            }
        }

        public ObservableCollection<RoleDTO> RoleList
        {
            get => _roleList;
            set
            {
                if (value != null)
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
                if (value != null)
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
                if (value != null)
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
                if (value != null)
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

        private RelayCommand<Window> _editUserCommand;

        public RelayCommand<Window> EditUserCommand { get { return _editUserCommand; } }

        public void EditUser(Window window)
        {
            if (User.UserName != null)
            {
                User.Roles = SelectedRoleList;
                User.Groups = SelectedGroupList;
                _administrationService.EditUser(User);
                window.Close();
            }
            else
            {
                MessageBox.Show("Fill empty fields!");
            }
        }


        public EditUserViewModel(IAdministrationService administrationService)
        {
            Messenger.Default.Register<UserDTO>(this, user =>
            {
                if (user != null)
                {
                    User = user;
                    SelectedRoleList = new ObservableCollection<RoleDTO>(User.Roles);
                    ICollection<RoleDTO> roleCollection = _administrationService.GetRoles();
                    foreach (var item in User.Roles)
                    {
                        roleCollection = roleCollection.Where(r => r.Name != item.Name).ToList();
                    }
                    RoleList = new ObservableCollection<RoleDTO>(roleCollection);

                    _groupList = new ObservableCollection<GroupDTO>(_administrationService.GetGroups());
                    _selectedGroupList = new ObservableCollection<GroupDTO>(User.Groups);
                    Messenger.Default.Unregister<UserDTO>(this);
                    //foreach (var item in SelectedGroupList)
                    //    _groupList.Remove(item);
                }
            });
            _administrationService = administrationService;
            
            _addRoleCommand = new RelayCommand<RoleDTO>(AddRole);
            _removeRoleCommand = new RelayCommand<RoleDTO>(RemoveRole);
            _addGroupCommand = new RelayCommand<GroupDTO>(AddGroup);
            _removeGroupCommand = new RelayCommand<GroupDTO>(RemoveGroup);
            _editUserCommand = new RelayCommand<Window>(EditUser);
        }
        
    }
}

