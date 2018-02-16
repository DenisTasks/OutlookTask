using GalaSoft.MvvmLight;
using Model.Entities;
using Model.ModelVIewElements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using ViewModel.Authentication;
using ViewModel.Interfaces;

namespace ViewModel.Print
{
    public class PrintAppointmentViewModel: ViewModelBase, IViewModel
    {
        private Appointment _appointment;
        private DelegateCommand _print;

        public PrintAppointmentViewModel()
        {
            _print = new DelegateCommand(ButtonPrintPressed, null);
            using (var uow = new UnitOfWork())
            {
                _appointment = uow.Appointments.FindById(9);
            }
        }

        public DelegateCommand PrintCommand { get { return _print; } }

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

        private void ButtonPrintPressed(object parametr)
        {
            Grid grid = parametr as Grid;
            PrintDialog myPrintDialog = new PrintDialog();
            if (myPrintDialog.ShowDialog() == true)
            {
                myPrintDialog.PrintVisual(grid, "Form All Controls Print");
            }
        }


        //public void PrintBtn_Click(object sender, RoutedEventArgs e)
        //{
        //    PrintDialog printDialog = new PrintDialog();
        //    if (printDialog.ShowDialog() == true)
        //    {
        //        printDialog.PrintVisual(grid, "My First Print Job");
        //    }
        //}


    }
       
}
