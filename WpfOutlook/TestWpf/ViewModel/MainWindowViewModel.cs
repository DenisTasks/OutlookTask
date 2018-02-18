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
using TestWpf.Helpers;

namespace TestWpf.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IBLLService _service;
        private ObservableCollection<AppointmentDTO> _appointments;

        public ObservableCollection<AppointmentDTO> Appointments
        {
            get => _appointments;
            private set
            {
                if (value != _appointments)
                {
                    _appointments = value;
                    base.RaisePropertyChanged();
                }
            }
        }

        public RelayCommand<AppointmentDTO> AboutAppointmentCommand { get; }
        public RelayCommand AddAppWindowCommand { get; }
        public RelayCommand<AppointmentDTO> RemoveAppCommand { get; }
        public RelayCommand SortByAppIdCommand { get; }
        public RelayCommand GroupBySubjectCommand { get; }
        public RelayCommand<AppointmentDTO> FilterBySubjectCommand { get; }

        public MainWindowViewModel(IBLLService service)
        {
            _service = service;
            AddAppWindowCommand = new RelayCommand(
                () =>
                Messenger.Default.Send(
                    new OpenWindowMessage() { Type = WindowType.AddAppWindow }));
            AboutAppointmentCommand = new RelayCommand<AppointmentDTO>(AboutAppointment);
            RemoveAppCommand = new RelayCommand<AppointmentDTO>(RemoveAppointment);
            SortByAppIdCommand = new RelayCommand(SortByAppId);
            GroupBySubjectCommand = new RelayCommand(GroupBySubject);
            FilterBySubjectCommand = new RelayCommand<AppointmentDTO>(FilterBySubject);

            Messenger.Default.Register<OpenWindowMessage>(this, message =>
            {
                if (message.Type == WindowType.Refresh)
                {
                    RefreshingAppointments();
                }
            });

            LoadData();
        }

        public void AboutAppointment(AppointmentDTO appointment)
        {
            if (appointment != null)
            {
                Messenger.Default.Send(new OpenWindowMessage()
                { Type = WindowType.AddAboutAppointmentWindow, Appointment = appointment, Argument = "Load this appointment" });
            }
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
          //          var allApps = _service.GetAppsByLocation(appointment).ToList();
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
        private void SortByAppId()
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(Appointments);
            if (view.SortDescriptions.Count > 0
                && view.SortDescriptions[0].PropertyName == "AppointmentId"
                && view.SortDescriptions[0].Direction == ListSortDirection.Ascending)
            {
                view.GroupDescriptions.Clear();
                view.Filter = null;
                view.SortDescriptions.Clear();
                view.SortDescriptions.Add(new SortDescription("AppointmentId", ListSortDirection.Descending));
            }
            else
            {
                view.GroupDescriptions.Clear();
                view.Filter = null;
                view.SortDescriptions.Clear();
                view.SortDescriptions.Add(new SortDescription("AppointmentId", ListSortDirection.Ascending));
            }
        }

        private void LoadData()
        {
            try
            {
                //for (int i = 1; i < 5; i++)
                //{
                //    Location location = new Location();
                //    location.Room = "Room number " + i;
                //    _service.AddLocation(location);
                //}
                //for (int i = 1; i < 5; i++)
                //{
                //    User user = new User();
                //    user.Name = "MyName " + i;
                //    _service.AddUser(user);
                //}
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
                view.Filter = o => ((o as AppointmentDTO)?.Subject) == appointment.Subject;
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
