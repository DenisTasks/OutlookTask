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

namespace ViewModel.ViewModels
{
    public class AddAppWindowViewModel : ViewModelBase
    {
        private readonly IBLLService _service;

        private ObservableCollection<UserDTO> _userList;
        private ObservableCollection<UserDTO> _selectedUserList;

        private DateTime _selectedBeginningTime;
        private DateTime _selectedEndingTime;
        private DateTime _startDate = DateTime.Today;
        private DateTime _endingDate = DateTime.Today;
        private LocationDTO _selectedLocation = new LocationDTO(){LocationId = 0};
        private int _isAvailible;

        public RelayCommand<Window> CreateAppCommand { get; }
        public RelayCommand<UserDTO> AddUserToListCommand { get; }
        public RelayCommand<UserDTO> RemoveUserFromListCommand { get; }

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
        public ObservableCollection<UserDTO> UserList
        {
            get => _userList;
            private set
            {
                if (value != _userList)
                {
                    _userList = value;
                    base.RaisePropertyChanged();
                }
            }
        }
        public AppointmentDTO Appointment { get; set; }

        // combo boxes
        public List<DateTime> BeginningTime { get; }
        public List<DateTime> EndingTime { get; }
        public List<LocationDTO> LocationList { get; }

        // selected
        public LocationDTO SelectedLocation
        {
            get => _selectedLocation;
            set
            {
                _selectedLocation = value;
                base.RaisePropertyChanged();
            }
        }
        public DateTime StartBeginningDate
        {
            get => _startDate;
            set
            {
                if (EndBeginningDate != value)
                {
                    EndBeginningDate = value;
                }
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
        public DateTime SelectedBeginningTime
        {
            get => _selectedBeginningTime;
            set
            {
                if (value != _selectedBeginningTime)
                {
                    _selectedBeginningTime = value;
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
                    base.RaisePropertyChanged();
                }
            }
        }

        public AddAppWindowViewModel(IBLLService service)
        {
            _service = service;
            AddUserToListCommand = new RelayCommand<UserDTO>(AddUsersToList);
            RemoveUserFromListCommand = new RelayCommand<UserDTO>(RemoveUsersFromList);
            CreateAppCommand = new RelayCommand<Window>(CreateAppointment);

            UserList = new ObservableCollection<UserDTO>(_service.GetUsers());
            SelectedUserList = new ObservableCollection<UserDTO>();
            LocationList = _service.GetLocations().ToList();
            
            Appointment = new AppointmentDTO();

            BeginningTime = LoadTimeRange();
            EndingTime = LoadTimeRange();
        }

        private List<DateTime> LoadTimeRange()
        {
            var timeList = new List<DateTime>();
            DateTime day = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 00, 00, 00);
            DateTime day2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 00);
            for (TimeSpan i = day.TimeOfDay; i < day2.TimeOfDay; i += TimeSpan.FromMinutes(30))
            {
                timeList.Add(DateTime.Parse(i.ToString()));
            }
            return timeList;
        }

        private void CheckDates()
        {
            _isAvailible = 0;
            if (_selectedLocation != null)
            {
                var startA = DateTime.Parse(_startDate.ToString("d") + " " + _selectedBeginningTime.ToString("h:mm tt"));
                var endA = DateTime.Parse(_endingDate.ToString("d") + " " + _selectedEndingTime.ToString("h:mm tt"));

                var bySameDay = _service.GetAppsByLocation(_selectedLocation.LocationId)
                    .Where(s => s.BeginningDate.DayOfYear == startA.DayOfYear).ToList();

                foreach (var b in bySameDay)
                {
                    bool overlap = startA < b.EndingDate && b.BeginningDate < endA;
                    if (overlap)
                    {
                        _isAvailible++;
                    }
                }
            }
        }

        private void CreateAppointment(Window window)
        {
            Appointment.BeginningDate = DateTime.Parse(_startDate.ToString("d") + " " + _selectedBeginningTime.ToString("h:mm tt"));
            Appointment.EndingDate = DateTime.Parse(_endingDate.ToString("d") + " " + _selectedEndingTime.ToString("h:mm tt"));
            Appointment.LocationId = SelectedLocation.LocationId;
            Appointment.Users = SelectedUserList;
            CheckDates();

            if (Appointment.IsValid && _isAvailible == 0)
            {
                _service.AddAppointment(Appointment);
                Messenger.Default.Send<NotificationMessage, MainWindowViewModel>(new NotificationMessage("Refresh"));
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
                SelectedUserList.Add(user);
                UserList.Remove(user);
                base.RaisePropertyChanged();
            }
        }

        private void RemoveUsersFromList(UserDTO user)
        {
            if (user != null)
            {
                UserList.Add(user);
                SelectedUserList.Remove(user);
                base.RaisePropertyChanged();
            }
        }
    }

}
