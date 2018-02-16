using GalaSoft.MvvmLight;
using Model.Entities;
using Model.ModelVIewElements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ViewModel.Authentication;
using ViewModel.Interfaces;

namespace ViewModel.Print
{
    public class PrintViewModel : ViewModelBase, IViewModel
    {
        private ObservableCollection<Appointment> _appointments;

        public PrintViewModel()
        {
            FillTable();
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

        public void FillTable()
        {
           using(var uow = new UnitOfWork())
           {
                Appointments = new ObservableCollection<Appointment>(uow.Appointments.Get());
           }
        }

        
    }
}
