using AutoMapper;
using BLL.BLLService;
using BLL.EntitesDTO;
using BLL.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using MVVM.ViewModels.Administration.Users;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            _administrationService.DeactivateUser(user.UserId);
            if (user.IsActive == true)
            {
                user.IsActive = false;
            }
            else
            {
                user.IsActive = true;
            }
            Users.Add(user);
            Users.Remove(Users.Last());
            Users = new ObservableCollection<UserModel>(Users.OrderBy(s => s.UserId));
        }

        private void AddUser()
        {
            var addUserWindow = new AddUserWindow();
            var result = addUserWindow.ShowDialog();
            LoadData();
            Users = _users;
        }

        private void EditUser(UserModel user)
        {
            if (user != null)
            {
                var editUserWindow = new EditUserWindow();
                Messenger.Default.Send<UserModel, EditUserViewModel>(user);
                var result = editUserWindow.ShowDialog();
                LoadData();
                Users = _users;
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
                    .ForMember(d => d.Groups, opt => opt.MapFrom(s => _administrationService.GetUserGroups(s.UserId)))
                    .ForMember(d => d.Roles, opt => opt.MapFrom(s => _administrationService.GetUserRoles(s.UserId)));

            }).CreateMapper();
            _users = new ObservableCollection<UserModel>(mapper.Map<IEnumerable<UserDTO>,ICollection<UserModel>>(_administrationService.GetUsers()));
        }

        //public event PropertyChangedEventHandler PropertyChanged;

        //private void NotifyPropertyChanged(string propertyName)
        //{
        //    if (PropertyChanged != null)
        //        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //}
    }
}
