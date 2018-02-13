using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Entities;
using Model.Interfaces;
using Model.ModelVIewElements;
using ViewModel.Common;

namespace ViewModel
{
    //public class Appointment
    //{
    //    public Appointment(int id, string subject)
    //    {
    //        AppointmentId = id;
    //        Subject = subject;
    //    }
    //    public int AppointmentId { get; set; }
    //    public string Subject { get; set; }
    //    public DateTime BeginningDate { get; set; }
    //    public DateTime EndingDate { get; set; }
    //}

    // test repo
    //public class Appointments
    //{
    //    public ObservableCollection<Appointment> Read()
    //    {
    //        return new ObservableCollection<Appointment>();
    //    }
    //}

    //public class IUnitOfWork
    //{
    //    public Appointments Appointments { get; set; }
    //}

    public class VM : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        private IUnitOfWork Database { get; set; }
        private ObservableCollection<Appointment> _appointments;
        public RelayCommand<Appointment> AddAppCommand { get; }
        public RelayCommand<Appointment> RemoveAppCommand { get; }

        public VM()
        {
            Database = new UnitOfWork();
            AddAppCommand = new RelayCommand<Appointment>(AddAppointment);
            RemoveAppCommand = new RelayCommand<Appointment>(RemoveAppointment);
        }
        //public VM(IUnitOfWork uOw)
        //{
        //    Database = uOw;
        //}
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
                    NotifyPropertyChanged("Appointments");
                }
            }
        }

        public void LoadData()
        {
            Appointments = new ObservableCollection<Appointment>(Database.Appointments.Get());
            //Appointment x1 = new Appointment(5, "Meeting now 5");
            //Appointment x2 = new Appointment(6, "Meeting now 6");
            //Appointment x3 = new Appointment(8, "Meeting now 8");
            //Appointment x4 = new Appointment(15, "Meeting now 15");
            //Appointment x5 = new Appointment(55, "Meeting now 55");
            //Appointments = new ObservableCollection<Appointment>();
            //Appointments.Add(x1);
            //Appointments.Add(x2);
            //Appointments.Add(x3);
            //Appointments.Add(x4);
            //Appointments.Add(x5);
        }

        public void AddAppointment(Appointment appointment)
        {
            if (appointment != null)
            {
                if (Appointments.Contains(appointment))
                {
                    //Appointments.Add(new Appointment(appointment.AppointmentId + 1, appointment.Subject + " + 1"));
                }
            }
        }

        public void RemoveAppointment(Appointment appointment)
        {
            Appointments.Remove(appointment);
        }

    }
}
