using AutoMapper;
using BLL.EntitesDTO;
using BLL.Interfaces;
using BLL.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Models;
using ViewModel.ViewModels.Administration.Groups;

namespace ViewModel.ViewModels.CommonViewModels.Groups
{
    [PrincipalPermission(SecurityAction.Demand, Role = "admin")]
    public class ShowAllGroupsViewModel : ViewModelBase
    {
        private readonly IAdministrationService _administrationService;
        private ObservableCollection<GroupModel> _groups;
        private RelayCommand<GroupModel> _editUserCommand { get; }
        private RelayCommand _addUserCommand { get; }
        private RelayCommand<GroupModel> _deleteGroupCommand { get; }

        public ShowAllGroupsViewModel(IAdministrationService administationService)
        {
            _administrationService = administationService;
            LoadData();
            _editUserCommand = new RelayCommand<GroupModel>(EditGroup);
            _addUserCommand = new RelayCommand(AddGroup);
            _deleteGroupCommand = new RelayCommand<GroupModel>(DeleteGroup);
        }

        public RelayCommand<GroupModel> EditUserCommand { get { return _editUserCommand; } }

        public RelayCommand AddUserCommand { get { return _addUserCommand; } }

        public RelayCommand<GroupModel> DeleteGroupCommand { get { return _deleteGroupCommand; } }

        public ObservableCollection<GroupModel> Groups
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

        private void DeleteGroup(GroupModel group)
        {
            if (group != null)
            {
                _administrationService.DeleteGroup(group.GroupId);
                Groups.Remove(group);
            }
        }

        private void AddGroup()
        {
            Messenger.Default.Send(new NotificationMessage("AddGrupWindow"));
            LoadData();
        }

        private void EditGroup(GroupModel group)
        {
            if (group != null)
            {
                Messenger.Default.Send<GroupModel>(group);
                LoadData();
            }
        }

        private void LoadData()
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<GroupDTO, GroupModel>()
                    .ForMember(d => d.GroupId, opt => opt.MapFrom(s => s.GroupId))
                    .ForMember(d => d.GroupName, opt => opt.MapFrom(s => s.GroupName))
                    .ForMember(d => d.CreatorId, opt => opt.MapFrom(s => s.CreatorId))
                    .ForMember(d => d.CreatorName, opt => opt.MapFrom(s => _administrationService.GetUserById(s.CreatorId).UserName))
                    .ForMember(d => d.ParentId, opt => opt.MapFrom(S => S.ParentId))
                    .ForMember(d => d.ParentName, opt => opt.MapFrom(s=> _administrationService.GetGroupName(s.ParentId)))
                    .ForMember(d => d.Groups, opt => opt.MapFrom(s => _administrationService.GetGroupFirstGeneration(s.GroupId)))
                    .ForMember(d => d.Users, opt => opt.MapFrom(s => new ObservableCollection<UserDTO>(_administrationService.GetGroupUsers(s.GroupId))));
            }).CreateMapper();
            Groups = new ObservableCollection<GroupModel>(mapper.Map<IEnumerable<GroupDTO>,ICollection<GroupModel>>(_administrationService.GetGroups()));
        }

    }
}
