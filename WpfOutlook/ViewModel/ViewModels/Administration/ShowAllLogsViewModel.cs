using BLL.EntitesDTO;
using BLL.Interfaces;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.ViewModels.Administration
{
    public class ShowAllLogsViewModel: ViewModelBase
    {
        private ObservableCollection<LogDTO> _logs;
        private IAdministrationService _administrationService;

        public ObservableCollection<LogDTO> Logs
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
            Logs = new ObservableCollection<LogDTO>(_administrationService.GetLogs());
        }
    }
}
