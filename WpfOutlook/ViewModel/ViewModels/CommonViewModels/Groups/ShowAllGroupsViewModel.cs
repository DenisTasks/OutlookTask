using BLL.EntitesDTO;
using BLL.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using MVVM.ViewModels.Administration.Groups;
using MVVM.ViewModels.CommonViewModels.Groups;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using ViewModel.ViewModels.Administration.Groups;

namespace ViewModel.ViewModels.CommonViewModels.Groups
{
    [PrincipalPermission(SecurityAction.Demand, Role = "admin")]
    public class ShowAllGroupsViewModel : ViewModelBase
    {
        private readonly IAdministrationService _administationService;
        private ObservableCollection<GroupDTO> _groups;
        private RelayCommand<GroupDTO> _editUserCommand { get; }
        private RelayCommand _addUserCommand { get; }
        private RelayCommand<GroupDTO> _deleteGroupCommand { get; }

        public ShowAllGroupsViewModel(IAdministrationService administationService)
        {
            _administationService = administationService;
            LoadData();
            _editUserCommand = new RelayCommand<GroupDTO>(EditGroup);
            _addUserCommand = new RelayCommand(AddGroup);
            _deleteGroupCommand = new RelayCommand<GroupDTO>(DeleteGroup);
        }

        public RelayCommand<GroupDTO> EditUserCommand { get { return _editUserCommand; } }

        public RelayCommand AddUserCommand { get { return _addUserCommand; } }

        public RelayCommand<GroupDTO> DeleteGroupCommand { get { return _deleteGroupCommand; } }

        public ObservableCollection<GroupDTO> Groups
        {
            get => _groups;
            set
            {
                if (value != null)
                {
                    _groups = value;
                    base.RaisePropertyChanged();
                }
            }
        }

        private void DeleteGroup(GroupDTO group)
        {
            _administationService.DeleteGroup(group);
            LoadData();
            Groups = _groups;

        }

        private void AddGroup()
        {
            var addGroupWindow = new AddGroupWindow();
            var result = addGroupWindow.ShowDialog();
            LoadData();
            Groups = _groups;
        }

        private void EditGroup(GroupDTO group)
        {
            if (group != null)
            {
                var editGroupWindow = new EditGroupWindow();
                Messenger.Default.Send<GroupDTO, EditGroupViewModel>(group);
                var result = editGroupWindow.ShowDialog();
                LoadData();
                Groups = _groups;
            }
        }

        private void LoadData()
        {
            _groups = new ObservableCollection<GroupDTO>(_administationService.GetGroups());
        }

    }
}
