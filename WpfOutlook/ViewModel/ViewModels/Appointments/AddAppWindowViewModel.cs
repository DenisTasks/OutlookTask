using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using AutoMapper;
using BLL.EntitesDTO;
using BLL.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Quartz;
using ViewModel.Jobs;
using ViewModel.Models;
using ViewModel.ViewModels.Authenication;

namespace ViewModel.ViewModels.Appointments
{
    public class AddAppWindowViewModel : ViewModelBase
    {
        private readonly IBLLServiceMain _service;

        private ObservableCollection<UserDTO> _userList;
        private ObservableCollection<UserDTO> _selectedUserList;
        private ObservableCollection<AppointmentModel> _templateApps;

        private DateTime _selectedBeginningTime;
        private DateTime _selectedEndingTime;
        private DateTime _startDate = DateTime.Today;
        private DateTime _endingDate = DateTime.Today;
        private LocationDTO _selectedLocation = new LocationDTO(){LocationId = 0};
        private AppointmentModel _selectedTemplateItem;
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
        public ObservableCollection<AppointmentModel> TemplateApps
        {
            get => _templateApps;
            private set
            {
                if (value != _templateApps)
                {
                    _templateApps = value;
                    base.RaisePropertyChanged();
                }
            }
        }

        public AppointmentModel Appointment { get; set; }

        // combo boxes
        public List<DateTime> BeginningTime { get; }
        public List<DateTime> EndingTime { get; }
        public List<LocationDTO> LocationList { get; }

        // selected
        public AppointmentModel SelectedTemplateItem
        {
            get => _selectedTemplateItem;
            set
            {
                _selectedTemplateItem = value;
                Appointment.Subject = value.Subject;
                SelectedLocation = LocationList.First(x => x.LocationId == value.LocationId);
                StartBeginningDate = value.BeginningDate;
                EndBeginningDate = value.EndingDate;
                SelectedBeginningTime = BeginningTime.First(x =>
                    x.Hour == value.BeginningDate.Hour && x.Minute == value.BeginningDate.Minute);
                SelectedEndingTime = BeginningTime.First(x =>
                    x.Hour == value.EndingDate.Hour && x.Minute == value.EndingDate.Minute);
                base.RaisePropertyChanged();
            }
        }
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

        private IMapper GetMapper()
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AppointmentDTO, AppointmentModel>()
                    .ForMember(d => d.AppointmentId, opt => opt.MapFrom(s => s.AppointmentId))
                    .ForMember(d => d.Subject, opt => opt.MapFrom(s => s.Subject))
                    .ForMember(d => d.BeginningDate, opt => opt.MapFrom(s => s.BeginningDate))
                    .ForMember(d => d.EndingDate, opt => opt.MapFrom(s => s.EndingDate))
                    .ForMember(d => d.LocationId, opt => opt.MapFrom(s => s.LocationId))
                    .ForMember(d => d.Room, opt => opt.MapFrom(s => _service.GetLocationById(s.LocationId).Room))
                    .ForMember(d => d.Users, opt => opt.MapFrom(s => new ObservableCollection<UserDTO>(_service.GetAppointmentUsers(s.AppointmentId))));

            }).CreateMapper();
            return mapper;
        }

        public AddAppWindowViewModel(IBLLServiceMain service)
        {
            _service = service;
            AddUserToListCommand = new RelayCommand<UserDTO>(AddUsersToList);
            RemoveUserFromListCommand = new RelayCommand<UserDTO>(RemoveUsersFromList);
            CreateAppCommand = new RelayCommand<Window>(CreateAppointment);

            UserList = new ObservableCollection<UserDTO>(_service.GetUsers());
            SelectedUserList = new ObservableCollection<UserDTO>();
            LocationList = _service.GetLocations().ToList();
            TemplateApps = new ObservableCollection<AppointmentModel>(GetMapper().Map<IEnumerable<AppointmentDTO>, ICollection<AppointmentModel>>(_service.GetAppointments()));

            Appointment = new AppointmentModel();

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
                var mapper = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<AppointmentModel, AppointmentDTO>().ForMember(s => s.LocationId,
                        opt => opt.MapFrom(loc => loc.LocationId));
                }).CreateMapper();
                CustomPrincipal customPrincipal = Thread.CurrentPrincipal as CustomPrincipal;
                _service.AddAppointment(mapper.Map<AppointmentModel,AppointmentDTO>(Appointment), _selectedUserList, customPrincipal.Identity.UserId );
                Appointment.Room = _service.GetLocationById(Appointment.LocationId).Room;

                string id = _service.GetAppointments().LastOrDefault().AppointmentId.ToString();
                IJobDetail job;
                ITrigger trigger;
                if (Appointment.BeginningDate < DateTime.Now)
                {
                    job = JobBuilder.Create<MissedNotifyCreater>()
                        .WithIdentity(id, "OutlookGroup")
                        .Build();
                    job.JobDataMap.Put("myApp", new List<AppointmentModel>{Appointment});

                    trigger = TriggerBuilder.Create()
                        .WithIdentity(id, "OutlookGroup")
                        .StartNow()
                        .Build();
                }
                else
                {
                    job = JobBuilder.Create<NotifyCreater>()
                        .WithIdentity(id, "OutlookGroup")
                        .Build();
                    job.JobDataMap.Put("myApp", Appointment);

                    trigger = TriggerBuilder.Create()
                        .WithIdentity(id, "OutlookGroup")
                        .StartAt(Appointment.BeginningDate.AddMinutes(-15))
                        .Build();
                }

                NotifyScheduler.WpfScheduler.ScheduleJob(job, trigger);

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
