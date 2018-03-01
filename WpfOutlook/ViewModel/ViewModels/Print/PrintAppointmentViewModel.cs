using BLL.EntitesDTO;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ViewModel.ViewModel.Print
{
    public class PrintAppointmentViewModel
    {
        private AppointmentDTO _appointment;
        private RelayCommand<object> _print;

        public PrintAppointmentViewModel()
        {
            _print = new RelayCommand<object>(ButtonPrintPressed);
            //using (var uow = new UnitOfWork())
            //{
            //    //_appointment = uow.Appointments.FindById(9);
            //}
        }

        public RelayCommand<object> PrintCommand { get { return _print; } }

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

        private void ButtonPrintPressed(object parametr)
        {
            Grid grid = parametr as Grid;
            PrintDialog myPrintDialog = new PrintDialog();
            if (myPrintDialog.ShowDialog() == true)
            {
                myPrintDialog.PrintVisual(grid, "Form All Controls Print");
            }
        }
    }
}
