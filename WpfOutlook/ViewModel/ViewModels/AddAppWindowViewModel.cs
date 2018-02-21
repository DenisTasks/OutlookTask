using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using BLL.DTO;
using BLL.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

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
        private LocationDTO _selectedLocation;
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

        private void ChekingTime()
        {
            _isAvailible = 0;
            ChekingBetweenDates();
            if (_selectedLocation != null && _isAvailible == 0)
            {
                #region Only beginning time after start and previous ending
                var parseDateBegin = DateTime.Parse(_startDate.ToString("d") + " " + _selectedBeginningTime.ToString("h:mm tt"));
                var parseDateEnd = DateTime.Parse(_endingDate.ToString("d") + " " + _selectedEndingTime.ToString("h:mm tt"));

                var bySameDayBegin = _service.GetAppsByLocation(_selectedLocation.LocationId)
                    .Where(s => s.BeginningDate.DayOfYear == StartBeginningDate.DayOfYear).ToList();
                foreach (var s in bySameDayBegin.ToList())
                {
                    // если есть такие аппы, где начало моего аппа попадает в их "с начала до конца" ИЛИ совпадает с их началом (с концом можно)
                    int resultStartFirst = DateTime.Compare(s.BeginningDate, parseDateBegin); // -1 or 0
                    int resultStartSecond = DateTime.Compare(s.EndingDate, parseDateBegin); // -1
                    if ((resultStartFirst == -1 || resultStartFirst == 0) && resultStartSecond == -1)
                    {
                        _isAvailible++;
                    }
                }
                #endregion

                #region Only ending time after start and previous ending
                var bySameDayEnd = _service.GetAppsByLocation(_selectedLocation.LocationId)
                    .Where(s => s.EndingDate.DayOfYear == EndBeginningDate.DayOfYear).ToList();
                foreach (var s in bySameDayEnd.ToList())
                {
                    // если есть такие аппы, где конец моего аппа совпадает с их концом ИЛИ попадает в их "с начала до конца"
                    int resultEndFirst = DateTime.Compare(parseDateEnd, s.BeginningDate); // -1
                    int resultEndSecond = DateTime.Compare(s.EndingDate, parseDateEnd); // -1 or 0
                    if (resultEndFirst == -1 && (resultEndSecond == -1 || resultEndSecond == 0))
                    {
                        _isAvailible++;
                    }
                }
                #endregion

                #region If inside or outside
                foreach (var s in bySameDayBegin.ToList())
                {
                    // если есть такие аппы, которые попадают В мои сроки полностью или их сроки РАВНЫ моим
                    // если есть такие аппы, которые шире мои сроков в обе стороны
                    int resultInStart = DateTime.Compare(s.BeginningDate, parseDateBegin); // -1 or 0     or 1/0 если шире
                    int resultInEnd = DateTime.Compare(parseDateEnd, s.EndingDate); // -1 or 0            or 1/0 если шире
                    if (((resultInStart == 0 || resultInStart == -1) && (resultInEnd == 0 || resultInEnd == -1))
                        || ((resultInStart == 1 || resultInStart == 0) && (resultInEnd == 1 || resultInEnd == 0)))
                    {
                        _isAvailible++;
                    }
                }
                #endregion
            }
        }

        private void ChekingBetweenDates()
        {
            if (_selectedLocation != null)
            {
                if (DateTime.Compare(_endingDate, _startDate) != 0)
                {
                    // concat?
                    var byDayIn = _service.GetAppsByLocation(_selectedLocation.LocationId)
                        .Where(s => s.BeginningDate.DayOfYear >= _startDate.DayOfYear && s.EndingDate.DayOfYear <= _endingDate.DayOfYear).ToList();
                    var byDayAgoAfter = _service.GetAppsByLocation(_selectedLocation.LocationId)
                        .Where(s => (s.BeginningDate.DayOfYear >= _startDate.DayOfYear && s.BeginningDate.DayOfYear <= _endingDate.DayOfYear)
                                    || (s.EndingDate.DayOfYear >= _startDate.DayOfYear && s.EndingDate.DayOfYear <= _endingDate.DayOfYear)).ToList();

                    if (byDayIn.Count > 0 || byDayAgoAfter.Count > 0)
                    {
                        _isAvailible++;
                        MessageBox.Show($"In your dates created appointments!");
                    }
                }
            }
            else
            {
                MessageBox.Show("Select location!");
            }
        }

        private void CreateAppointment(Window window)
        {
            Appointment.BeginningDate = DateTime.Parse(_startDate.ToString("d") + " " + _selectedBeginningTime.ToString("h:mm tt"));
            Appointment.EndingDate = DateTime.Parse(_endingDate.ToString("d") + " " + _selectedEndingTime.ToString("h:mm tt"));
            ChekingTime();

            if (SelectedUserList.Count > 0 && _isAvailible == 0 && SelectedLocation.LocationId > 0)
            {
                Appointment.LocationId = SelectedLocation.LocationId;
                Appointment.Users = SelectedUserList;
                _service.AddAppointment(Appointment);
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
