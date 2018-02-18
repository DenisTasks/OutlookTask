using System;
using System.Windows;
using BLL.DTO;
using BLL.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using TestWpf.Helpers;

namespace TestWpf.ViewModel
{
    public class AboutAppointmentWindowViewModel : ViewModelBase
    {
        private readonly IBLLService _service;
        private AppointmentDTO _appointment;
        private LocationDTO _location;
        public LocationDTO Location
        {
            get => _location;
            private set
            {
                if (value != _location)
                {
                    _location = value;
                    base.RaisePropertyChanged();
                }
            }
        }

        public AppointmentDTO Appointment
        {
            get => _appointment;
            private set
            {
                if (value != _appointment)
                {
                    _appointment = value;
                    base.RaisePropertyChanged();
                }
            }
        }

        public RelayCommand<AppointmentDTO> RemoveAppCommand { get; set; }
        public AboutAppointmentWindowViewModel(IBLLService service)
        {
            _service = service;
            Messenger.Default.Register<OpenWindowMessage>(this, message =>
            {
                if (message.Argument == "Load this appointment")
                {
                    Appointment = message.Appointment;
                    Location = _service.GetLocationById(message.Appointment.LocationId);
                }
            });
            RemoveAppCommand = new RelayCommand<AppointmentDTO>(RemoveAppointment);
        }

        private void RemoveAppointment(AppointmentDTO appointment)
        {
            if (appointment != null)
            {
                try
                {
                    _service.RemoveAppointment(appointment);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }

    }
}
