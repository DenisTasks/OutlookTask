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

namespace ViewModel.ViewModels.Administration.Groups
{
    public class EditGroupViewModel : ViewModelBase
    {
        private readonly IAdministrationService _administrationService;

        private bool _editor;
        private string _oldName;
        private ICollection<GroupDTO> _hiddenGroupAncestors;

        private ICollection<GroupDTO> _selectedGroupChildren;

        private ObservableCollection<GroupDTO> _groupList;
        private ObservableCollection<GroupDTO> _selectedGroupList;

        private ObservableCollection<UserDTO> _userList;
        private ObservableCollection<UserDTO> _selectedUserList;

        private ICollection<GroupDTO> _groupsForComboBox;

        public ICollection<GroupDTO> GroupsForComboBox
        {
            get => _groupsForComboBox;
            set
            {
                _groupsForComboBox = value;
            }
        }

        private GroupDTO _groupNameForFilter;

        public GroupDTO GroupNameForFilter
        {
            get => _groupNameForFilter;
            set
            {
                _groupNameForFilter = value;
                FilterGroupList(_groupNameForFilter);
                base.RaisePropertyChanged();
            }
        }

        private void FilterGroupList(GroupDTO group)
        {
            if (_editor)
            {
                if (group.GroupName != "Not")
                {
                    Group.ParentId = group.GroupId;
                    ICollection<string> groupNameList = _administrationService.GetGroupAncestors(group.GroupName);
                    foreach (var groupName in groupNameList)
                    {
                        foreach (var item in GroupList.ToList())
                        {
                            if (item.GroupName == groupName)
                            {
                                GroupList.Remove(item);
                                _hiddenGroupAncestors.Add(item);
                            }
                        }
                    }
                    SelectedGroupList = new ObservableCollection<GroupDTO>();
                }
                else
                {
                    Group.ParentId = null;
                    SelectedGroupList = new ObservableCollection<GroupDTO>();
                    foreach (var item in _hiddenGroupAncestors.ToList())
                    {
                        GroupList.Add(item);
                        _hiddenGroupAncestors.Remove(item);
                    }
                }
            }
            else
            {
                _editor = true;
                if (Group.ParentId != null)
                {
                    ICollection<string> groupNameList = _administrationService.GetGroupAncestors(Group.GroupName);
                    foreach (var ancestor in groupNameList)
                    {
                        foreach (var item in GroupList.ToList())
                        {
                            if (item.GroupName == ancestor)
                            {
                                GroupList.Remove(item);
                            }
                        }
                    }
                }
                GroupList.Remove(GroupList.FirstOrDefault(g=>g.GroupId == Group.GroupId));
            }
        }

        private GroupDTO _group;

        public GroupDTO Group
        {
            get => _group;
            set
            {
                if (value != null)
                {
                    _group = value; base.RaisePropertyChanged();
                }
            }
        }

        public EditGroupViewModel(IAdministrationService administrationService)
        {
            Messenger.Default.Register<GroupDTO>(this, group =>
            {
                if (group != null)
                {
                    Group = group;
                    _oldName = group.GroupName;
                
                    SelectedUserList = new ObservableCollection<UserDTO>(_administrationService.GetGroupUsers(group.GroupId));
                    UserList = new ObservableCollection<UserDTO>(_administrationService.GetUsers());
                    foreach (var item in SelectedUserList)
                    {
                        foreach (var temp in UserList.ToList())
                        {
                            if (item.Name == temp.Name)
                            {
                                UserList.Remove(temp);
                            }
                        }
                    }

                    SelectedGroupList = new ObservableCollection<GroupDTO>(_administrationService.GetGroupFirstGeneration(Group.GroupId));
                    GroupList = new ObservableCollection<GroupDTO>(_administrationService.GetGroupsWithNoAncestors());

                    _groupsForComboBox.Remove(_groupsForComboBox.FirstOrDefault(g => g.GroupId == Group.GroupId));
                    GroupNameForFilter = _groupsForComboBox.FirstOrDefault(g => g.GroupId == group.ParentId);
                    Messenger.Default.Unregister<GroupDTO>(this);

                    //for graph
                    //GroupList = new ObservableCollection<GroupDTO>(_administrationService.GetGroups());
                    //foreach(var childName in _administrationService.GetGroupChildren(group.GroupId))
                    //{
                    //    if (SelectedGroupList.Any(g => g.GroupName == childName))
                    //    {
                    //        var item = SelectedGroupList.FirstOrDefault(g => g.GroupName == childName);
                    //        GroupList.Remove(item);
                    //        _selectedGroupChildren.Add(item);
                    //    }
                    //    else
                    //    {
                    //        foreach (var item in GroupList.Where(g => g.GroupName == childName).ToList())
                    //        {
                    //            GroupList.Remove(item);
                    //            _selectedGroupChildren.Add(item);
                    //        }
                    //    }
                    //}
                }
            });

            _administrationService = administrationService;
            _editor = false;
            _hiddenGroupAncestors = new List<GroupDTO>();

            _groupsForComboBox = _administrationService.GetGroups();
            _groupsForComboBox.Add(new GroupDTO { GroupName = "Not" });

            _addUserCommand = new RelayCommand<UserDTO>(AddUser);
            _removeUserCommand = new RelayCommand<UserDTO>(RemoveUser);
            _addGroupCommand = new RelayCommand<GroupDTO>(AddGroup);
            _removeGroupCommand = new RelayCommand<GroupDTO>(RemoveGroup);
            _createGroupCommand = new RelayCommand<Window>(CreateGroup);
        }

        public ObservableCollection<UserDTO> UserList
        {
            get => _userList;
            set
            {
                if (value != _userList)
                {
                    _userList = value;
                    base.RaisePropertyChanged();
                }
            }
        }

        public ObservableCollection<UserDTO> SelectedUserList
        {
            get => _selectedUserList;
            set
            {
                if (value != _selectedUserList)
                {
                    _selectedUserList = value;
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

        private RelayCommand<UserDTO> _addUserCommand;
        private RelayCommand<UserDTO> _removeUserCommand;

        public RelayCommand<UserDTO> AddUserCommand { get { return _addUserCommand; } }
        public RelayCommand<UserDTO> RemoveUserCommand { get { return _removeUserCommand; } }

        public void AddUser(UserDTO user)
        {
            SelectedUserList.Add(user);

            UserList.Remove(user);
            base.RaisePropertyChanged();
        }

        public void RemoveUser(UserDTO user)
        {
            UserList.Add(user);
            SelectedUserList.Remove(user);
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
            SelectedGroupList.Remove(group);
            GroupList.Add(group); 
            base.RaisePropertyChanged();
        }

        private RelayCommand<Window> _createGroupCommand;

        public RelayCommand<Window> CreateGroupCommand { get { return _createGroupCommand; } }

        public void CreateGroup(Window window)
        {
            if (Group.GroupName == _oldName)
            {
                _administrationService.EditGroup(Group, SelectedGroupList, SelectedUserList);
                window.Close();
            }
            else
            { 
                if (Group.GroupName != null)
                {
                   if( _administrationService.CheckGroup(Group.GroupName))
                    {
                        _administrationService.EditGroup(Group, SelectedGroupList, SelectedUserList);
                        window.Close();
                    }
                    else { MessageBox.Show("This name already exists"); }
                }
                else
                {
                    MessageBox.Show("Fill empty fields!");
                }
            }
        }
    }
}
