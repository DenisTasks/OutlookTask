using BLL.EntitesDTO;
using BLL.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ViewModel.ViewModels.CommonViewModels.Groups
{
    public class AddGroupViewModel : ViewModelBase
    {
        private readonly IAdministrationService _administrationService;

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
            }
        }

        private void FilterGroupList(GroupDTO group)
        {
            if (group.GroupName != "Not")
            {
                Group.ParentId = group.GroupId;
                ICollection<string> groupNameList = _administrationService.GetGroupAncestors(group.GroupName);
                ICollection<GroupDTO> filterGroupCollection = _administrationService.GetGroups();
                foreach (var item in groupNameList)
                {
                    filterGroupCollection = filterGroupCollection.Where(r => r.GroupName != item).ToList();
                }
                GroupList = new ObservableCollection<GroupDTO>(filterGroupCollection);
                SelectedGroupList = new ObservableCollection<GroupDTO>();
            }
            else
            {
                Group.ParentId = null;
                SelectedGroupList = new ObservableCollection<GroupDTO>();
                GroupList = new ObservableCollection<GroupDTO>(_administrationService.GetGroups());
            }
        }

        

        public GroupDTO Group { get; set; }

        public AddGroupViewModel(IAdministrationService administrationService)
        {
            _administrationService = administrationService;

            _selectedGroupChildren = new List<GroupDTO>();

            _userList = new ObservableCollection<UserDTO>(_administrationService.GetUsers());
            _selectedUserList = new ObservableCollection<UserDTO>();

            _groupsForComboBox = _administrationService.GetGroups();
            _groupList = new ObservableCollection<GroupDTO>(_groupsForComboBox);
            _selectedGroupList = new ObservableCollection<GroupDTO>();
            _groupsForComboBox.Add(new GroupDTO { GroupName = "Not" }); 

            _addUserCommand = new RelayCommand<UserDTO>(AddUser);
            _removeUserCommand = new RelayCommand<UserDTO>(RemoveUser);
            _addGroupCommand = new RelayCommand<GroupDTO>(AddGroup);
            _removeGroupCommand = new RelayCommand<GroupDTO>(RemoveGroup);
            _createGroupCommand = new RelayCommand<Window>(CreateGroup);

            Group = new GroupDTO();
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
            foreach(var childName in _administrationService.GetGroupChildren(group.GroupId))
            {
                if (SelectedGroupList.Any(g => g.GroupName == childName))
                {
                    var item = SelectedGroupList.FirstOrDefault(g => g.GroupName == childName);
                    SelectedGroupList.Remove(item);
                    _selectedGroupChildren.Add(item);
                }
                else
                {
                    foreach (var item in GroupList.Where(g => g.GroupName == childName).ToList())
                    {
                        GroupList.Remove(item);
                        _selectedGroupChildren.Add(item);
                    }
                }
            }
            GroupList.Remove(group);
            base.RaisePropertyChanged();
        }

        public void RemoveGroup(GroupDTO group)
        {
            SelectedGroupList.Remove(group);
            GroupList.Add(group);
            List<string> childrenInTheStoredList = new List<string>();
            ICollection<string> childrenToGroupList = _administrationService.GetGroupChildren(group.GroupId);
            foreach(var item in SelectedGroupList)
            {
                childrenInTheStoredList.AddRange(_administrationService.GetGroupChildren(item.GroupId));
            }
            childrenInTheStoredList.Distinct();
            foreach (var item in childrenInTheStoredList)
            {
                childrenToGroupList = childrenToGroupList.Where(ch => ch != item).ToList();
            }
            foreach(var item in childrenToGroupList)
            {
                foreach(var tmp in _selectedGroupChildren.ToList())
                {
                    if(item == tmp.GroupName)
                    {
                        _selectedGroupChildren.Remove(tmp);
                        GroupList.Add(tmp);
                    }
                }
            }
            base.RaisePropertyChanged();
        }

        private RelayCommand<Window> _createGroupCommand;

        public RelayCommand<Window> CreateGroupCommand { get { return _createGroupCommand; } }

        public void CreateGroup(Window window)
        {
            if (Group.GroupName!=null && _administrationService.CheckGroup(Group.GroupName))
            {
                _administrationService.CreateGroup(Group, SelectedGroupList, SelectedUserList);
                window.Close();
            }
            else
            {
                MessageBox.Show("Fill empty fields!");
            }
        }
    }
}

