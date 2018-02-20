using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BLL.DTO;
using BLL.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using ViewModel.Helpers;

namespace ViewModel.ViewModels
{
    public interface IRange<T>
    {
        T Start { get; }
        T End { get; }
        bool Includes(T value);
        bool Includes(IRange<T> range);
    }

    public class DateRange : IRange<DateTime>
    {
        public DateRange(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }

        public bool Includes(DateTime value)
        {
            return (Start <= value) && (value <= End);
        }

        public bool Includes(IRange<DateTime> range)
        {
            return (Start <= range.Start) && (range.End <= End);
        }
    }

    public class AllAppByLocationWindowViewModel : ViewModelBase
    {
        private readonly IBLLService _service;
        private ObservableCollection<AppointmentDTO> _appointments;
        private List<AppointmentDTO> _checking;
        private string _isAvailible;

        public string IsAvailible
        {
            get => _isAvailible;
            private set
            {
                if (value != _isAvailible)
                {
                    _isAvailible = value;
                    base.RaisePropertyChanged();
                }
            }
        }

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
            _service = service;
            Messenger.Default.Register<OpenWindowMessage>(this, message =>
            {
                if (message.Type == WindowType.LoadLocations && message.Argument != null)
                {
                    Appointments = new ObservableCollection<AppointmentDTO>(_service.GetAppsByLocation(Int32.Parse(message.Argument)));
                    _checking = new List<AppointmentDTO>();
                    RoomIsAvailible(_appointments);
                }
            });
        }

        private void RoomIsAvailible(ObservableCollection<AppointmentDTO> items)
        {
            foreach (var item in items)
            {
                _checking = items.Where(s => new DateRange(s.BeginningDate, s.EndingDate).Includes(item.BeginningDate)).ToList();
                if (_checking.Count > 0)
                {
                    IsAvailible = _checking.Count.ToString();
                }
            }
        }
    }

}
