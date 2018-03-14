﻿using AutoMapper;
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
using System.Windows;
using ViewModel.Models;

namespace ViewModel.ViewModels.Administration.Users
{
    public class ShowAllUsersViewModel: ViewModelBase
    {
        private readonly IAdministrationService _administrationService;
        private ObservableCollection<UserModel> _users;
        private RelayCommand<UserModel> _editUserCommand { get; }
        private RelayCommand _addUserCommand { get; }
        private RelayCommand<UserModel> _deactivateUserCommand { get; }

        public ShowAllUsersViewModel(IAdministrationService administrationService)
        {
            _administrationService = administrationService;
            LoadData();
            _editUserCommand = new RelayCommand<UserModel>(EditUser);
            _addUserCommand = new RelayCommand(AddUser);
            _deactivateUserCommand = new RelayCommand<UserModel>(DeactivateUser);
        }

        public RelayCommand<UserModel> EditUserCommand { get { return _editUserCommand; } }

        public RelayCommand AddUserCommand { get { return _addUserCommand; } }

        public RelayCommand<UserModel> DeactivateUserCommand { get { return _deactivateUserCommand; } }

        public ObservableCollection<UserModel> Users
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

        private void DeactivateUser(UserModel user)
        {
            if (user.Roles.Any(r => r.Name.Equals("admin")) && _administrationService.GetNumberOfAdmins() == 1 && user.IsActive)
            {
                MessageBox.Show("You need to have one or more admin usres in the system");
            }
            else
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
            }
        }

        private void AddUser()
        {
            Messenger.Default.Send<NotificationMessage>(new NotificationMessage("AddUserWindow"));
            LoadData();
            Users = _users;
        }

        private void EditUser(UserModel user)
        {
            if (user != null)
            {
                Messenger.Default.Send<UserModel>(user);
            }
        }

        private void LoadData()
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserDTO, UserModel>()
                    .ForMember(d => d.UserId, opt => opt.MapFrom(s => s.UserId))
                    .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name))
                    .ForMember(d => d.UserName, opt => opt.MapFrom(s => s.UserName))
                    .ForMember(d => d.IsActive, opt => opt.MapFrom(s => s.IsActive))
                    .ForMember(d => d.Password, opt => opt.MapFrom(s => s.Password))
                    .ForMember(d => d.Groups, opt => opt.MapFrom(s =>new  ObservableCollection<GroupDTO>(_administrationService.GetUserGroups(s.UserId))))
                    .ForMember(d => d.Roles, opt => opt.MapFrom(s => new ObservableCollection<RoleDTO>(_administrationService.GetUserRoles(s.UserId))));

            }).CreateMapper();
            _users = new ObservableCollection<UserModel>(mapper.Map<IEnumerable<UserDTO>,ICollection<UserModel>>(_administrationService.GetUsers()));
        }
    }
}
