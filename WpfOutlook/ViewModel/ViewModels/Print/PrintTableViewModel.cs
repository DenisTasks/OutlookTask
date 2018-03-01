using BLL.EntitesDTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ViewModel.ViewModel.Print
{
    public class PrintTable
    {
        private ObservableCollection<AppointmentDTO> _appointments;

        public PrintTable()
        {
            FillTable();
        }

        public ObservableCollection<AppointmentDTO> Appointments
        {
            get => _appointments;
            private set
            {
                if (value != _appointments)
                {
                    _appointments = value;
                    //base.RaisePropertyChanged();
                }
            }
        }

        public void FillTable()
        {
            //using (var uow = new UnitOfWork())
            //{
            //    //Appointments = new ObservableCollection<AppointmentDTO>(uow.Appointments.Get());
            //}
        }



        private void PrintBtn_Click(object sender, RoutedEventArgs e)

        {

            PrintDialog printDialog = new PrintDialog();


            if (printDialog.ShowDialog() == true)

            {

                //printDialog.PrintVisual(grid, "My First Print Job");

            }
        }
    }
}
