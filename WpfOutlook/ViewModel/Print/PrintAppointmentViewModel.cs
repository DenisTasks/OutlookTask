using GalaSoft.MvvmLight;
using Model.Entities;
using Model.ModelVIewElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Interfaces;

namespace ViewModel.Print
{
    public class PrintAppointmentViewModel: ViewModelBase, IViewModel
    {
        private Appointment _appointment;

        public PrintAppointmentViewModel()
        {
            using (var uow = new UnitOfWork())
            {
                _appointment = uow.Appointments.FindById(9);
            }
        }

        //public PrintAppointmentViewModel(int id)
        //{
        //    using (var uow = new UnitOfWork())
        //    {
        //        _appointment = uow.Appointments.FindById(id);
        //    }
        //}

        public string AppointmentName
        {
            get { return _appointment.Subject; }
        }

        public DateTime AppointmentBeginDate
        {
            get { return _appointment.BeginningDate; }
        }

        public int AppointmentId
        {
            get { return _appointment.AppointmentId; }
        }
        
    }
}
