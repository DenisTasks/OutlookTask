using AutoMapper;
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
using ViewModel.Models;

namespace ViewModel.ViewModels.Administration.Users
{
    public class EditUserViewModel : ViewModelBase
    {
        private readonly IAdministrationService _administrationService;

        private UserModel _user;
        private string _oldUserName;

        private ObservableCollection<RoleDTO> _roleList;

        private ObservableCollection<GroupDTO> _groupList;

        public UserModel User
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
            User.Roles.Add(role);
            RoleList.Remove(role);
            base.RaisePropertyChanged();
        }

        public void RemoveRole(RoleDTO role)
        {
            RoleList.Add(role);
            User.Roles.Remove(role);
            base.RaisePropertyChanged();
        }


        private RelayCommand<GroupDTO> _addGroupCommand;
        private RelayCommand<GroupDTO> _removeGroupCommand;

        public RelayCommand<GroupDTO> AddGroupCommand { get { return _addGroupCommand; } }
        public RelayCommand<GroupDTO> RemoveGroupCommand { get { return _removeGroupCommand; } }

        public void AddGroup(GroupDTO group)
        {
            User.Groups.Add(group);
            GroupList.Remove(group);
            base.RaisePropertyChanged();
        }

        public void RemoveGroup(GroupDTO group)
        {
            GroupList.Add(group);
            User.Groups.Remove(group);
            base.RaisePropertyChanged();
        }

        private RelayCommand<Window> _editUserCommand;

        public RelayCommand<Window> EditUserCommand { get { return _editUserCommand; } }

        public void EditUser(Window window)
        {
            if (_oldUserName == User.UserName)
            {
                _administrationService.EditUser(GetMapper().Map<UserModel, UserDTO>(User), User.Groups, User.Roles);
                window.Close();
            }
            else
            {
                if (User.UserName != null)
                {
                    if (_administrationService.CheckUser(User.UserName))
                    {
                        _administrationService.EditUser(GetMapper().Map<UserModel, UserDTO>(User), User.Groups, User.Roles);
                        window.Close();
                    }
                    else
                    {
                        MessageBox.Show("User with this name already exists");
                    }
                }
                else
                {
                    MessageBox.Show("Fill empty fields!");
                }
            }
        }


        public EditUserViewModel(IAdministrationService administrationService)
        {
            Messenger.Default.Register<UserModel>(this, user =>
            {
                if (user != null)
                {
                    User = user;
                    _oldUserName = user.UserName;
                    RoleList = new ObservableCollection<RoleDTO>(_administrationService.GetRoles());
                    foreach (var item in User.Roles)
                    {
                        foreach(var temp in RoleList.ToList())
                        {
                            if(item.Name == temp.Name)
                            {
                                RoleList.Remove(temp);
                            }
                        }
                    }
                    
                    GroupList = new ObservableCollection<GroupDTO>(_administrationService.GetGroups());
                    foreach (var item in User.Groups)
                    {
                        foreach (var temp in GroupList.ToList())
                        {
                            if (item.GroupName == temp.GroupName)
                            {
                                GroupList.Remove(temp);
                            }
                        }
                    }
                    
                    //ICollection<GroupDTO> groupCollection = _administrationService.GetGroups();
                    //foreach (var item in _selectedGroupList)
                    //{
                    //    groupCollection = groupCollection.Where(g => g.GroupName != item.GroupName).ToList();
                    //}
                    //GroupList = new ObservableCollection<GroupDTO>(groupCollection);
                    Messenger.Default.Unregister<UserDTO>(this);
                }
            });
            _administrationService = administrationService;
            
            _addRoleCommand = new RelayCommand<RoleDTO>(AddRole);
            _removeRoleCommand = new RelayCommand<RoleDTO>(RemoveRole);
            _addGroupCommand = new RelayCommand<GroupDTO>(AddGroup);
            _removeGroupCommand = new RelayCommand<GroupDTO>(RemoveGroup);
            _editUserCommand = new RelayCommand<Window>(EditUser);
        }

        private IMapper GetMapper()
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserModel, UserDTO>()
                    .ForMember(d => d.UserId, opt => opt.MapFrom(s => s.UserId))
                    .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name))
                    .ForMember(d => d.UserName, opt => opt.MapFrom(s => s.UserName))
                    .ForMember(d => d.IsActive, opt => opt.MapFrom(s => s.IsActive))
                    .ForMember(d => d.Password, opt => opt.MapFrom(s => s.Password));

            }).CreateMapper();
            return mapper;
        }
    }
}

