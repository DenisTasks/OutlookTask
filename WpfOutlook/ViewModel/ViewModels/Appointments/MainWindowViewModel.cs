using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;
using AutoMapper;
using BLL.EntitesDTO;
using BLL.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Quartz;
using Quartz.Impl.Matchers;
using ViewModel.Helpers;
using ViewModel.Jobs;
using ViewModel.Models;
using ViewModel.ViewModels.Authenication;

namespace ViewModel.ViewModels.Appointments
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IBLLServiceMain _service;
        private ObservableCollection<AppointmentModel> _appointments;
        private ObservableCollection<FileInfo> _files;
        private FileInfo _selectTheme;
        private int Id { get; }

        public FileInfo SelectedTheme
        {
            get { return _selectTheme; }
            set
            {
                _selectTheme = value;
                base.RaisePropertyChanged();
                ChangeTheme(_selectTheme);
            }
        }
        public ObservableCollection<FileInfo> Files
        {
            get => _files;
            set
            {
                _files = value;
                base.RaisePropertyChanged();
            }
        }
        public ObservableCollection<AppointmentModel> Appointments
        {
            get => _appointments;
            set
            {
                if (value != _appointments)
                {
                    _appointments = value;
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

        #region Commands
        public RelayCommand<AppointmentModel> AboutAppointmentCommand { get; }
        public RelayCommand<AppointmentModel> AllAppByLocationCommand { get; }
        public RelayCommand AddAppWindowCommand { get; }
        public RelayCommand<AppointmentModel> RemoveAppCommand { get; }
        public RelayCommand<object> SortCommand { get; }
        public RelayCommand GroupBySubjectCommand { get; }
        public RelayCommand<AppointmentModel> FilterBySubjectCommand { get; }
        public RelayCommand CalendarWindowCommand { get; }
        public RelayCommand<object> PrintTable { get; }
        public RelayCommand LogoutCommand { get; }
        public RelayCommand CalendarFrameCommand { get; }
        #endregion

        public MainWindowViewModel(IBLLServiceMain service)
        {
            CustomPrincipal cp = Thread.CurrentPrincipal as CustomPrincipal;
            if (cp != null) Id = cp.Identity.UserId;
            _service = service;
            LoadData(Id);
            #region Commands
            AddAppWindowCommand = new RelayCommand(AddAppointment);
            AboutAppointmentCommand = new RelayCommand<AppointmentModel>(AboutAppointment);
            AllAppByLocationCommand = new RelayCommand<AppointmentModel>(GetAllAppsByRoom);
            RemoveAppCommand = new RelayCommand<AppointmentModel>(RemoveAppointment);
            SortCommand = new RelayCommand<object>(SortBy);
            GroupBySubjectCommand = new RelayCommand(GroupBySubject);
            FilterBySubjectCommand = new RelayCommand<AppointmentModel>(FilterBySubject);
            CalendarWindowCommand = new RelayCommand(GetCalendar);
            PrintTable = new RelayCommand<object>(PrintListView);
            LogoutCommand = new RelayCommand(Logout, CanLogout);
            CalendarFrameCommand = new RelayCommand(CalendarFrame);
            #endregion

            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "Resources");
                var localthemes = new DirectoryInfo(path).GetFiles();
                if (Files == null)
                    Files = new ObservableCollection<FileInfo>();
                foreach (var item in localthemes)
                {
                    Files.Add(item);
                }
                SelectedTheme = Files[1];
            }));

            Messenger.Default.Register<NotificationMessage>(this, message =>
            {
                if (message.Notification == "Refresh")
                {
                    RefreshingAppointments();
                }
            });
        }

        private bool IsAuthenticated => Thread.CurrentPrincipal.Identity.IsAuthenticated;
        private void Logout()
        {
            CustomPrincipal customPrincipal = Thread.CurrentPrincipal as CustomPrincipal;
            if (customPrincipal != null)
            {
                customPrincipal.Identity = new AnonymousIdentity();
            }
        }
        private bool CanLogout()
        {
            return IsAuthenticated;
        }
        private void PrintListView(object parameter)
        {
            PrintHelper.PrintViewList(parameter as ListView);
        }
        private void ChangeTheme(FileInfo selectTheme)
        {
            Application.Current.Resources.Clear();
            Application.Current.Resources.Source = new Uri(selectTheme.FullName, UriKind.Absolute);
        }
        private void AddAppointment()
        {
            Messenger.Default.Send( new OpenWindowMessage { Type = WindowType.AddAppWindow, Argument = "AddAppWindow"});
        }
        private void CalendarFrame()
        {
            Messenger.Default.Send(new OpenWindowMessage { Type = WindowType.CalendarFrame });
        }
        private void AboutAppointment(AppointmentModel appointment)
        {
            if (appointment != null)
            {
                Messenger.Default.Send(new OpenWindowMessage()
                { Type = WindowType.AddAboutAppointmentWindow, Appointment = appointment, Argument = "Load this appointment" });
            }
        }
        private void GetCalendar()
        {
            Messenger.Default.Send(new OpenWindowMessage() { Type = WindowType.Calendar });
        }
        private void RefreshingAppointments()
        {
            Appointments.Clear();
            Appointments = new ObservableCollection<AppointmentModel>(GetMapper().Map<IEnumerable<AppointmentDTO>, ICollection<AppointmentModel>>(_service.GetAppointmentsByUserId(Id)));
            Messenger.Default.Send(new OpenWindowMessage { Type = WindowType.Toast, Argument = "You added a new\r\nappointment! Check\r\nyour calendar, please!", SecondsToShow = 5 });
        }
        private void GetAllAppsByRoom(AppointmentModel appointment)
        {
            if (appointment != null)
            {
                try
                {
                    Messenger.Default.Send(new OpenWindowMessage()
                    { Type = WindowType.AddAllAppByLocationWindow, Argument = appointment.LocationId.ToString() });
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }
        private void GroupBySubject()
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(Appointments);
            view.GroupDescriptions.Clear();
            view.GroupDescriptions.Add(new PropertyGroupDescription("Subject"));
        }
        private void SortBy(object parameter)
        {
            string column = parameter as string;
            if (column != null)
            {
                ICollectionView view = CollectionViewSource.GetDefaultView(Appointments);
                if (view.SortDescriptions.Count > 0
                    && view.SortDescriptions[0].PropertyName == column
                    && view.SortDescriptions[0].Direction == ListSortDirection.Ascending)
                {
                    view.GroupDescriptions.Clear();
                    view.Filter = null;
                    view.SortDescriptions.Clear();
                    view.SortDescriptions.Add(new SortDescription(column, ListSortDirection.Descending));
                }
                else
                {
                    view.GroupDescriptions.Clear();
                    view.Filter = null;
                    view.SortDescriptions.Clear();
                    view.SortDescriptions.Add(new SortDescription(column, ListSortDirection.Ascending));
                }
            }
        }
        private void LoadData(int id)
        {
            try
            {
                Appointments = new ObservableCollection<AppointmentModel>(GetMapper().Map<IEnumerable<AppointmentDTO>, ICollection<AppointmentModel>>(_service.GetAppointmentsByUserId(id)));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
        //private void LoadData()
        //{
        //    try
        //    {
        //        Appointments = new ObservableCollection<AppointmentModel>(GetMapper().Map<IEnumerable<AppointmentDTO>, ICollection<AppointmentModel>>(_service.GetAppointments()));
        //    }
        //    catch (Exception e)
        //    {
        //        MessageBox.Show(e.ToString());
        //    }
        //}
        private void FilterBySubject(AppointmentModel appointment)
        {
            if (appointment != null)
            {
                ICollectionView view = CollectionViewSource.GetDefaultView(Appointments);
                view.GroupDescriptions.Clear();
                view.Filter = s => ((s as AppointmentModel)?.Subject) == appointment.Subject;
            }
        }
        private void RemoveAppointment(AppointmentModel appointment)
        {
            if (appointment != null)
            {
                try
                {
                    CustomPrincipal customPrincipal = Thread.CurrentPrincipal as CustomPrincipal;
                    _service.RemoveAppointment(appointment.AppointmentId, customPrincipal.Identity.UserId);

                    var myJob = NotifyScheduler.WpfScheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup())
                        .Where(x => x.Name == appointment.AppointmentId.ToString()).ToList();
                    if (myJob.Count > 0)
                    {
                        var triggerKeyList = NotifyScheduler.WpfScheduler.GetTriggersOfJob(myJob[0]);
                        var triggerKey = triggerKeyList[0].Key;
                        NotifyScheduler.WpfScheduler.UnscheduleJob(NotifyScheduler.WpfScheduler.GetTrigger(triggerKey).Key);
                    }
                    
                    Appointments.Remove(appointment);

                    base.RaisePropertyChanged();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }
    }
}