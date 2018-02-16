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
using TestWpf.Helpers;

namespace TestWpf.ViewModel
{
    public class AddAppWindowViewModel : ViewModelBase
    {
        private readonly IBLLService _service;
        public RelayCommand<Window> CreateAppCommand { get; }
        public RelayCommand<UserDTO> AddUsersToListCommand { get; }
        public RelayCommand<UserDTO> RemoveUsersFromListCommand { get; }

        private ObservableCollection<UserDTO> _selectedUsersList;
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

        private ObservableCollection<UserDTO> _usersList;
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

        public List<LocationDTO> LocationList { get; }
        private LocationDTO _selectedLocation;
        public LocationDTO SelectedLocation
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
            AddUsersToListCommand = new RelayCommand<UserDTO>(AddUsersToList);
            RemoveUsersFromListCommand = new RelayCommand<UserDTO>(RemoveUsersFromList);
            CreateAppCommand = new RelayCommand<Window>(CreateAppointment);

            UsersList = new ObservableCollection<UserDTO>(_service.GetUsers());
            SelectedUsersList = new ObservableCollection<UserDTO>();
            Appointment = new AppointmentDTO();

            LocationList = _service.GetLocations().ToList();

            BeginningTime = new List<string>() { DateTime.Now.ToString("h:mm tt") };
            EndingTime = new List<string>() { DateTime.Now.ToString("h:mm tt") };
        }

        public void CreateAppointment(Window window)
        {
            Appointment.BeginningDate = DateTime.Parse(_startDate.ToString("d") + " " + SelectedBeginningTime.ToString("h:mm tt"));
            Appointment.EndingDate = DateTime.Parse(_endingDate.ToString("d") + " " + SelectedEndingTime.ToString("h:mm tt"));
            Appointment.LocationId = SelectedLocation.LocationId;
            if (SelectedUsersList.Count > 0)
            {
                Appointment.Users = SelectedUsersList;
                _service.AddAppointment(Appointment);
                Messenger.Default.Send(new OpenWindowMessage() { Argument = "AddAppDone" });
                window?.Close();
            }
            else
            {
                MessageBox.Show("Please, add users to appointment list!");
            }
        }

        public void AddUsersToList(UserDTO user)
        {
            if (user != null)
            {
                SelectedUsersList.Add(user);
                UsersList.Remove(user);
                base.RaisePropertyChanged();
            }
        }

        public void RemoveUsersFromList(UserDTO user)
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
