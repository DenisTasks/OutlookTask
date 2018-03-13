using AutoMapper;
using BLL.EntitesDTO;
using BLL.Interfaces;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Models;

namespace ViewModel.ViewModels.Administration
{
    public class ShowAllLogsViewModel: ViewModelBase
    {
        private ObservableCollection<LogModel> _logs;
        private IAdministrationService _administrationService;

        public ObservableCollection<LogModel> Logs
        {
            get => _logs;
            set
            {
                _logs = value;
                base.RaisePropertyChanged();
            }
        }


        public ShowAllLogsViewModel(IAdministrationService administrationService)
        {
            _administrationService = administrationService;
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<LogDTO, LogModel>()
                    .ForMember(d => d.LogId, opt => opt.MapFrom(s => s.LogId))
                    .ForMember(d => d.ActionName, opt => opt.MapFrom(s => s.Action))
                    .ForMember(d => d.AppointmentName, opt => opt.MapFrom(s => s.AppointmentName))
                    .ForMember(d => d.ActionAuthorName, opt => opt.MapFrom(s => _administrationService.GetUserById(s.ActionAuthorId).Name))
                    .ForMember(d => d.CreatorName, opt => opt.MapFrom(s => _administrationService.GetUserById(s.CreatorId).Name))
                    .ForMember(d => d.EventTime, opt => opt.MapFrom(s => s.EventTime));

            }).CreateMapper();
            Logs = new ObservableCollection<LogModel>(mapper.Map<IEnumerable<LogDTO>, ICollection<LogModel>>(_administrationService.GetLogs()));
        }
    }
}
