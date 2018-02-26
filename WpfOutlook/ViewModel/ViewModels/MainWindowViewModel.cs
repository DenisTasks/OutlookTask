using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using BLL.DTO;
using BLL.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using ViewModel.Helpers;

namespace ViewModel.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IBLLService _service;
        private ObservableCollection<AppointmentDTO> _appointments;

        public ObservableCollection<AppointmentDTO> Appointments
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

        #region Commands
        public RelayCommand<AppointmentDTO> AboutAppointmentCommand { get; }
        public RelayCommand<AppointmentDTO> AllAppByLocationCommand { get; }
        public RelayCommand AddAppWindowCommand { get; }
        public RelayCommand<AppointmentDTO> RemoveAppCommand { get; }
        public RelayCommand<object> SortCommand { get; }
        public RelayCommand GroupBySubjectCommand { get; }
        public RelayCommand<AppointmentDTO> FilterBySubjectCommand { get; }
        public RelayCommand CalendarWindowCommand { get; }
        public RelayCommand ToastCommand { get; }
        #endregion

        public MainWindowViewModel(IBLLService service)
        {
            _service = service;
            LoadData();

            AddAppWindowCommand = new RelayCommand(AddAppointment);
            AboutAppointmentCommand = new RelayCommand<AppointmentDTO>(AboutAppointment);
            AllAppByLocationCommand = new RelayCommand<AppointmentDTO>(GetAllAppsByRoom);
            RemoveAppCommand = new RelayCommand<AppointmentDTO>(RemoveAppointment);
            SortCommand = new RelayCommand<object>(SortBy);
            GroupBySubjectCommand = new RelayCommand(GroupBySubject);
            FilterBySubjectCommand = new RelayCommand<AppointmentDTO>(FilterBySubject);
            CalendarWindowCommand = new RelayCommand(GetCalendar);
            ToastCommand = new RelayCommand(ToastWindow);

            Messenger.Default.Register<NotificationMessage>(this, message =>
            {
                if (message.Notification == "Refresh")
                {
                    RefreshingAppointments();
                }
            });
        }

        private void ToastWindow()
        {
            Messenger.Default.Send(new OpenWindowMessage { Type = WindowType.Toast, Argument = "You have a new\r\nappointment! Check\r\nyour calendar, please!", SecondsToShow = 5 });
        }

        private void AddAppointment()
        {
            Messenger.Default.Send( new OpenWindowMessage() { Type = WindowType.AddAppWindow });
        }
        private void AboutAppointment(AppointmentDTO appointment)
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
            Appointments = new ObservableCollection<AppointmentDTO>(_service.GetAppointments());
        }
        private void GetAllAppsByRoom(AppointmentDTO appointment)
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
                Appointments = new ObservableCollection<AppointmentDTO>(_service.GetAppointments());
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
        private void FilterBySubject(AppointmentDTO appointment)
        {
            if (appointment != null)
            {
                ICollectionView view = CollectionViewSource.GetDefaultView(Appointments);
                view.GroupDescriptions.Clear();
                view.Filter = s => ((s as AppointmentDTO)?.Subject) == appointment.Subject;
            }
        }
        private void RemoveAppointment(AppointmentDTO appointment)
        {
            if (appointment != null)
            {
                try
                {
                    _service.RemoveAppointment(appointment);
                    Appointments.Remove(appointment);
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
