using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using BLL.DTO;
using BLL.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using ViewModel.Helpers;

namespace ViewModel.ViewModels
{
    public class AddAppWindowViewModel : ViewModelBase
    {
        private readonly IBLLService _service;

        private ObservableCollection<UserDTO> _usersList;
        private ObservableCollection<UserDTO> _selectedUsersList;

        private DateTime _selectedBeginningTime;
        private DateTime _selectedEndingTime;
        private DateTime _startDate = DateTime.Today;
        private DateTime _endingDate = DateTime.Today.AddHours(1);
        private LocationDTO _selectedLocation;
        private int _isAvailible;

        public RelayCommand<Window> CreateAppCommand { get; }
        public RelayCommand<UserDTO> AddUsersToListCommand { get; }
        public RelayCommand<UserDTO> RemoveUsersFromListCommand { get; }

        public ObservableCollection<UserDTO> SelectedUsersList
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
        public ObservableCollection<UserDTO> UsersList
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
        public AppointmentDTO Appointment { get; set; }

        public List<string> BeginningTime { get; }
        public List<string> EndingTime { get; }
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
        public List<LocationDTO> LocationList { get; }
        public LocationDTO SelectedLocation
        {
            get => _selectedLocation;
            set
            {
                _selectedLocation = value;
                base.RaisePropertyChanged();
            }
        }
        public DateTime SelectedBeginningTime
        {
            get => _selectedBeginningTime;
            set
            {
                if (value != _selectedBeginningTime)
                {
                    _selectedBeginningTime = value;
                    ChekingTimeBegin();
                    base.RaisePropertyChanged();
                }
            }
        }
        public DateTime SelectedEndingTime
        {
            get => _selectedEndingTime;
            set
            {
                if (value != _selectedEndingTime)
                {
                    _selectedEndingTime = value;
                    ChekingTimeEnd();
                    base.RaisePropertyChanged();
                }
            }
        }

        public AddAppWindowViewModel(IBLLService service)
        {
            _service = service;
            AddUsersToListCommand = new RelayCommand<UserDTO>(AddUsersToList);
            RemoveUsersFromListCommand = new RelayCommand<UserDTO>(RemoveUsersFromList);
            CreateAppCommand = new RelayCommand<Window>(CreateAppointment);

            UsersList = new ObservableCollection<UserDTO>(_service.GetUsers());
            SelectedUsersList = new ObservableCollection<UserDTO>();
            LocationList = _service.GetLocations().ToList();

            Appointment = new AppointmentDTO();

            BeginningTime = new List<string>() { DateTime.Now.ToString("h:mm tt"), DateTime.Now.AddHours(1).ToString("h:mm tt"), DateTime.Now.AddHours(2).ToString("h:mm tt") };
            EndingTime = new List<string>() { DateTime.Now.AddHours(1).ToString("h:mm tt"), DateTime.Now.AddHours(2).ToString("h:mm tt"), DateTime.Now.AddHours(3).ToString("h:mm tt") };
        }

        private void ChekingTimeBegin()
        {
            if (_selectedLocation != null)
            {
                _isAvailible = 0;
                var parseDateBegin = DateTime.Parse(_startDate.ToString("d") + " " + _selectedBeginningTime.ToString("h:mm tt"));

                var byDay = _service.GetAppsByLocation(_selectedLocation.LocationId)
                    .Where(s => s.BeginningDate.DayOfYear == StartBeginningDate.DayOfYear).ToList();

                foreach (var s in byDay.ToList())
                {
                    int resultStartFirst = DateTime.Compare(s.BeginningDate, parseDateBegin); // -1
                    int resultStartSecond = DateTime.Compare(parseDateBegin, s.EndingDate); // -1
                    if (resultStartFirst == -1 && resultStartSecond == -1)
                    {
                        _isAvailible++;
                    }
                }
                if (_isAvailible > 0)
                {
                    MessageBox.Show("You are have same time with " + _isAvailible + " locations!");
                }
                if (_isAvailible == 0)
                {
                    MessageBox.Show("This room is availible for selected begin time period!");
                }
            }
            else
            {
                MessageBox.Show("Please, select location room!");
            }
        }

        private void ChekingTimeEnd()
        {
            if (_selectedLocation != null)
            {
                _isAvailible = 0;
                var parseDateEnd = DateTime.Parse(_endingDate.ToString("d") + " " + _selectedEndingTime.ToString("h:mm tt"));

                var byDay = _service.GetAppsByLocation(_selectedLocation.LocationId)
                    .Where(s => s.EndingDate.DayOfYear == EndBeginningDate.DayOfYear).ToList();

                foreach (var s in byDay.ToList())
                {
                    int resultEndFirst = DateTime.Compare(s.BeginningDate, parseDateEnd); // -1
                    int resultEndSecond = DateTime.Compare(parseDateEnd, s.EndingDate); // -1
                    if (resultEndFirst == -1 && resultEndSecond == -1)
                    {
                        _isAvailible++;
                    }
                }
                if (_isAvailible > 0)
                {
                    MessageBox.Show("You are have same time with " + _isAvailible + " locations!");
                }
                if (_isAvailible == 0)
                {
                    MessageBox.Show("This room is availible for selected end time period!");
                }
            }
            else
            {
                MessageBox.Show("Please, select location room!");
            }
        }

        private void CreateAppointment(Window window)
        {
            Appointment.BeginningDate = DateTime.Parse(_startDate.ToString("d") + " " + _selectedBeginningTime.ToString("h:mm tt"));
            Appointment.EndingDate = DateTime.Parse(_endingDate.ToString("d") + " " + _selectedEndingTime.ToString("h:mm tt"));
            if (SelectedUsersList.Count > 0 && SelectedLocation.LocationId > 0 && _isAvailible == 0)
            {
                Appointment.LocationId = SelectedLocation.LocationId;
                Appointment.Users = SelectedUsersList;
                _service.AddAppointment(Appointment);
                Messenger.Default.Send(new OpenWindowMessage() { Type = WindowType.None });
                window?.Close();
            }
            else
            {
                MessageBox.Show("Please, check your choice!");
            }
        }

        private void AddUsersToList(UserDTO user)
        {
            if (user != null)
            {
                SelectedUsersList.Add(user);
                UsersList.Remove(user);
                base.RaisePropertyChanged();
            }
        }

        private void RemoveUsersFromList(UserDTO user)
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
