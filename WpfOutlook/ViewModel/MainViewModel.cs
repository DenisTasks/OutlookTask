﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using Model.Entities;
using Model.Interfaces;
using Model.ModelVIewElements;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using ViewModel.Helpers;

namespace ViewModel
{
    public class MainViewModel : ViewModelBase
        //, INotifyPropertyChanged
    {
        //#region INotifyPropertyChanged

        //public event PropertyChangedEventHandler PropertyChanged;

        //public void NotifyPropertyChanged(string propertyName)
        //{
        //    if (PropertyChanged != null)
        //    {
        //        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //    }
        //}
        //#endregion

        private IUnitOfWork Database { get; set; }
        private ObservableCollection<Appointment> _appointments;
        public RelayCommand AddAppCommand { get; }
        public RelayCommand<Appointment> RemoveAppCommand { get; }
        public RelayCommand<Appointment> SortByAppIdCommand { get; }
        public RelayCommand<Appointment> GroupBySubjectCommand { get; }
        public RelayCommand<Appointment> FilterBySubjectCommand { get; }

        public MainViewModel()
        {
            AddAppCommand = new RelayCommand(
                () => 
                Messenger.Default.Send<OpenWindowMessage>(
                    new OpenWindowMessage() {Type = WindowType.kModal, Argument = "1"}));
            RemoveAppCommand = new RelayCommand<Appointment>(RemoveAppointment);
            SortByAppIdCommand = new RelayCommand<Appointment>(SortByAppId);
            GroupBySubjectCommand = new RelayCommand<Appointment>(GroupBySubject);
            FilterBySubjectCommand = new RelayCommand<Appointment>(FilterBySubject);
            LoadData();
        }

        public void GroupBySubject(Appointment appointment)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(this.Appointments);
            view.GroupDescriptions.Clear();
            view.GroupDescriptions.Add(new PropertyGroupDescription("Subject"));
        }
        public void SortByAppId(Appointment appointment)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(this.Appointments);
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
            get
            {
                return _appointments;
            }
            private set
            {
                if (value != _appointments)
                {
                    _appointments = value;
                    base.RaisePropertyChanged("Appointments");
                }
            }
        }


        public void LoadData()
        {
            using (Database = new UnitOfWork())
            {
                //for (int i = 1; i < 20; i++)
                //{
                //    Appointment x = new Appointment() { AppointmentId = i, BeginningDate = DateTime.Now, EndingDate = DateTime.Now, Location = new Location(), LocationId = i*i, Subject = $"Meeting {i}" };
                //    Database.Appointments.Create(x);
                //    Database.Save();
                //}
                Appointments = new ObservableCollection<Appointment>(Database.Appointments.Get());
            }
        }

        public void FilterBySubject(Appointment appointment)
        {
            if (appointment != null)
            {
                ICollectionView view = CollectionViewSource.GetDefaultView(this.Appointments);
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
                using (Database = new UnitOfWork())
                {
                    Database.Appointments.Remove(appointment);
                    Database.Save();
                    Appointments.Remove(appointment);
                    base.RaisePropertyChanged("Appointments");
                }
            }
            
        }

    }
}
