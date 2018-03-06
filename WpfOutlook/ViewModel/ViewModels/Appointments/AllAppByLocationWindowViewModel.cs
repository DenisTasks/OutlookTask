using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AutoMapper;
using BLL.EntitesDTO;
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
                    var mapper = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<AppointmentDTO, AppointmentModel>()
                            .ForMember(d => d.AppointmentId, opt => opt.MapFrom(s => s.AppointmentId))
                            .ForMember(d => d.Subject, opt => opt.MapFrom(s => s.Subject))
                            .ForMember(d => d.BeginningDate, opt => opt.MapFrom(s => s.BeginningDate))
                            .ForMember(d => d.EndingDate, opt => opt.MapFrom(s => s.EndingDate))
                            .ForMember(d => d.LocationId, opt => opt.MapFrom(s => s.LocationId))
                            .ForMember(d => d.Room, opt => opt.MapFrom(s => service.GetLocationById(s.LocationId).Room))
                            .ForMember(d => d.Users, opt => opt.MapFrom(s => new ObservableCollection<UserDTO>(service.GetAppointmentUsers(s.AppointmentId))));

                    }).CreateMapper();
                    Appointments = new ObservableCollection<AppointmentModel>(mapper.Map<IEnumerable<AppointmentDTO>, ICollection<AppointmentModel>>(service.GetAppsByLocation(Int32.Parse(message.Argument))));
                }
            });
        }
    }

}
