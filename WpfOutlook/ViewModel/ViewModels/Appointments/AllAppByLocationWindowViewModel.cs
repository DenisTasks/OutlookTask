using System;
using System.Collections.ObjectModel;
using BLL.DTO;
using BLL.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using ViewModel.Helpers;
using ViewModel.Models;

namespace ViewModel.ViewModels.Appointments
{
    public class AllAppByLocationWindowViewModel : ViewModelBase
    {
        private ObservableCollection<AppointmentModel> _appointments;

        public ObservableCollection<AppointmentModel> Appointments
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

        public AllAppByLocationWindowViewModel(IBLLServiceMain service)
        {
            Messenger.Default.Register<OpenWindowMessage>(this, message =>
            {
                if (message.Type == WindowType.LoadLocations && message.Argument != null)
                {
                    Appointments = new ObservableCollection<AppointmentModel>(service.GetAppsByLocation(Int32.Parse(message.Argument)));
                }
            });
        }
    }

}
