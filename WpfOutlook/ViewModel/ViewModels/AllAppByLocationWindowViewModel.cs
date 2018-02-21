using System;
using System.Collections.ObjectModel;
using BLL.DTO;
using BLL.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using ViewModel.Helpers;

namespace ViewModel.ViewModels
{
    public class AllAppByLocationWindowViewModel : ViewModelBase
    {
        private ObservableCollection<AppointmentDTO> _appointments;

        public ObservableCollection<AppointmentDTO> Appointments
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

        public AllAppByLocationWindowViewModel(IBLLService service)
        {
            Messenger.Default.Register<OpenWindowMessage>(this, message =>
            {
                if (message.Type == WindowType.LoadLocations && message.Argument != null)
                {
                    Appointments = new ObservableCollection<AppointmentDTO>(service.GetAppsByLocation(Int32.Parse(message.Argument)));
                }
            });
        }
    }

}
