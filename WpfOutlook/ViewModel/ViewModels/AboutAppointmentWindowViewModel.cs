using BLL.DTO;
using BLL.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using ViewModel.Helpers;

namespace ViewModel.ViewModels
{
    public class AboutAppointmentWindowViewModel : ViewModelBase
    {
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

        public AboutAppointmentWindowViewModel(IBLLServiceMain service)
        {
            Messenger.Default.Register<OpenWindowMessage>(this, message =>
            {
                if (message.Argument == "Load this appointment")
                {
                    Appointment = message.Appointment;
                    Location = service.GetLocationById(message.Appointment.LocationId);
                }
            });
        }
    }

}
