using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Model.Entities;
using ViewModel.Interfaces;

namespace TestWpf.ViewModel
{
    public class AddAppWindowViewModel : ViewModelBase
    {
        private readonly IBLLService _service;
        public RelayCommand CreateAppCommand { get; }
        public RelayCommand<User> AddUsersToListCommand { get; }
        public RelayCommand<User> RemoveUsersFromListCommand { get; }

        private ObservableCollection<User> _selectedUsersList;
        public ObservableCollection<User> SelectedUsersList
        {
            get => _selectedUsersList;
            set
            {
                if (value != _selectedUsersList)
                {
                    _selectedUsersList = value;
                    base.RaisePropertyChanged();
                }
            }
        }

        private ObservableCollection<User> _usersList;
        public ObservableCollection<User> UsersList
        {
            get => _usersList;
            private set
            {
                if (value != _usersList)
                {
                    _usersList = value;
                    base.RaisePropertyChanged();
                }
            }
        }

        public Appointment Appointment { get; set; }

        public List<string> BeginningTime { get; }
        public List<string> EndingTime { get; }

        private DateTime _startDate = DateTime.Today;
        private DateTime _endingDate = DateTime.Today.AddHours(1);

        public DateTime StartBeginningDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                base.RaisePropertyChanged();
            }
        }
        public DateTime EndBeginningDate
        {
            get => _endingDate;
            set
            {
                _endingDate = value;
                base.RaisePropertyChanged();
            }
        }

        public List<Location> LocationList { get; }
        private Location _selectedLocation;
        public Location SelectedLocation
        {
            get => _selectedLocation;
            set
            {
                _selectedLocation = value;
                base.RaisePropertyChanged();
            }
        }


        public DateTime SelectedBeginningTime { get; set; }
        public DateTime SelectedEndingTime { get; set; }

        public AddAppWindowViewModel(IBLLService service)
        {
            _service = service;
            AddUsersToListCommand = new RelayCommand<User>(AddUsersToList);
            RemoveUsersFromListCommand = new RelayCommand<User>(RemoveUsersFromList);
            CreateAppCommand = new RelayCommand(CreateAppointment);

            UsersList = new ObservableCollection<User>(_service.GetUsers());
            SelectedUsersList = new ObservableCollection<User>();
            Appointment = new Appointment();

            LocationList = _service.GetLocations().ToList();

            BeginningTime = new List<string>() { DateTime.Now.ToString("h:mm tt") };
            EndingTime = new List<string>() { DateTime.Now.ToString("h:mm tt") };
        }

        public void CreateAppointment()
        {
            Appointment.BeginningDate = DateTime.Parse(_startDate.ToString("d") + " " + SelectedBeginningTime.ToString("h:mm tt"));
            Appointment.EndingDate = DateTime.Parse(_endingDate.ToString("d") + " " + SelectedEndingTime.ToString("h:mm tt"));
            Appointment.LocationId = SelectedLocation.LocationId;
            if (SelectedUsersList.Count > 0)
            {
                Appointment.Users = SelectedUsersList;
            }
            _service.AddAppointment(Appointment);
        }

        public void AddUsersToList(User user)
        {
            if (user != null)
            {
                SelectedUsersList.Add(user);
                UsersList.Remove(user);
                base.RaisePropertyChanged();
            }
        }

        public void RemoveUsersFromList(User user)
        {
            if (user != null)
            {
                UsersList.Add(user);
                SelectedUsersList.Remove(user);
                base.RaisePropertyChanged();
            }
        }
    }
}
