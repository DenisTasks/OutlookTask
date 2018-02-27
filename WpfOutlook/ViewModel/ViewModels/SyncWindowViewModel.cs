using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BLL.DTO;
using BLL.Interfaces;
using GalaSoft.MvvmLight;

namespace ViewModel.ViewModels
{
    public class SyncWindowViewModel: ViewModelBase
    {
        private readonly IBLLService _service;
        private ObservableCollection<AppointmentDTO> _appointmentsOther;


        public ObservableCollection<AppointmentDTO> AppointmentsSync
        {
            get => _appointmentsOther;
            set
            {
                if (value != _appointmentsOther)
                {
                    _appointmentsOther = value;
                    base.RaisePropertyChanged();
                }
            }
        }

        public SyncWindowViewModel(IBLLService service)
        {
            _service = service;
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                AppointmentsSync = new ObservableCollection<AppointmentDTO>(_service.GetCalendar());
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

    }
}
