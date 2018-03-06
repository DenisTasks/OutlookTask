﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
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

namespace ViewModel.ViewModels.Appointments
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IBLLServiceMain _service;
        private ObservableCollection<AppointmentModel> _appointments;
        private ObservableCollection<FileInfo> _files;
        private FileInfo _selectTheme;

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
        
        #endregion

        public MainWindowViewModel(IBLLServiceMain service)
        {
            _service = service;
            LoadData();
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
            #endregion

            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                var localthemes = new DirectoryInfo("Themes").GetFiles();
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
            Messenger.Default.Send( new OpenWindowMessage() { Type = WindowType.AddAppWindow });
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
            Appointments = new ObservableCollection<AppointmentModel>(GetMapper().Map<IEnumerable<AppointmentDTO>, ICollection<AppointmentModel>>(_service.GetAppointments()));
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
        private void LoadData()
        {
            try
            {
                Appointments = new ObservableCollection<AppointmentModel>(GetMapper().Map<IEnumerable<AppointmentDTO>, ICollection<AppointmentModel>>(_service.GetAppointments()));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
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
                    _service.RemoveAppointment(appointment.AppointmentId);
                    Appointments.Remove(appointment);

                    var myJob = NotifyScheduler.WpfScheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup())
                        .Where(x => x.Name == appointment.AppointmentId.ToString()).ToList();
                    var triggerKeyList = NotifyScheduler.WpfScheduler.GetTriggersOfJob(myJob[0]);
                    var triggerKey = triggerKeyList[0].Key;
                    NotifyScheduler.WpfScheduler.UnscheduleJob(NotifyScheduler.WpfScheduler.GetTrigger(triggerKey).Key);

                    base.RaisePropertyChanged();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }
    }
}