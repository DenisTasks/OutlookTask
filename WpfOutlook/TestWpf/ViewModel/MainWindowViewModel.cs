using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Model.Entities;
using Model.Interfaces;
using Model.ModelVIewElements;
using TestWpf.Helpers;
using ViewModel.Interfaces;

namespace TestWpf.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IBLLService _service;
        private ObservableCollection<Appointment> _appointments;
        public RelayCommand AddAppCommand { get; }
        public RelayCommand<Appointment> RemoveAppCommand { get; }
        public RelayCommand SortByAppIdCommand { get; }
        public RelayCommand GroupBySubjectCommand { get; }
        public RelayCommand<Appointment> FilterBySubjectCommand { get; }

        public MainWindowViewModel(IBLLService service)
        {
            _service = service;
            AddAppCommand = new RelayCommand(
                () =>
                Messenger.Default.Send(
                    new OpenWindowMessage() { Type = WindowType.AddAppWindow, Argument = "1" }));
            RemoveAppCommand = new RelayCommand<Appointment>(RemoveAppointment);
            SortByAppIdCommand = new RelayCommand(SortByAppId);
            GroupBySubjectCommand = new RelayCommand(GroupBySubject);
            FilterBySubjectCommand = new RelayCommand<Appointment>(FilterBySubject);
            LoadData();
        }

        public void GroupBySubject()
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(Appointments);
            view.GroupDescriptions.Clear();
            view.GroupDescriptions.Add(new PropertyGroupDescription("Subject"));
        }
        public void SortByAppId()
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

        public ObservableCollection<Appointment> Appointments
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

        public void LoadData()
        {
            try
            {
                //for (int i = 1; i < 20; i++)
                //{
                //    Appointment x = new Appointment() { AppointmentId = i, BeginningDate = DateTime.Now, EndingDate = DateTime.Now, Location = new Location(), LocationId = i * i, Subject = $"Meeting {i}" };
                //    Database.Appointments.Create(x);
                //    Database.Save();
                //}
                Appointments = new ObservableCollection<Appointment>(_service.GetAppointments());
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public void FilterBySubject(Appointment appointment)
        {
            if (appointment != null)
            {
                ICollectionView view = CollectionViewSource.GetDefaultView(Appointments);
                view.GroupDescriptions.Clear();
                view.Filter = o => ((o as Appointment)?.Subject) == appointment.Subject;
            }
        }

        //public void AddAppointment(Appointment appointment)
        //{
        //    if (appointment != null)
        //    {
        //        using (Database = new UnitOfWork())
        //        {
        //            Appointment x9 = new Appointment() { BeginningDate = DateTime.Now, EndingDate = DateTime.Now, Location = new Location(), LocationId = DateTime.Now.Second, Subject = "Meeting random" };
        //            Database.Appointments.Create(x9);
        //            Database.Save();
        //            Appointments.Add(x9);
        //            base.RaisePropertyChanged("Appointments");
        //        }
        //    }
        //}

        public void RemoveAppointment(Appointment appointment)
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
