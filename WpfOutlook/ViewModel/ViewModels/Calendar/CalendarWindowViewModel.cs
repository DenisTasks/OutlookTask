using System;
using System.Collections.ObjectModel;
using System.Windows;
using BLL.EntitesDTO;
using BLL.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using ViewModel.Helpers;

namespace ViewModel.ViewModels.Calendar
{
    public class CalendarWindowViewModel : ViewModelBase
    {

        private int _text;
        public int Text => _text;

        private int _text2;
        public int Text2 => _text2;

        private readonly IBLLServiceMain _service;
        private ObservableCollection<AppointmentDTO> _appointments;
        private ObservableCollection<UserDTO> _users;
        private UserDTO _selectedSyncUser;

        public ObservableCollection<UserDTO> Users
        {
            get => _users;
            set
            {
                if (value != _users)
                {
                    _users = value;
                    base.RaisePropertyChanged();
                }
            }
        }
        public ObservableCollection<AppointmentDTO> Appointments
        {
            get => _appointments;
            set
            {
                if (value != _appointments)
                {
                    _appointments = value;
                    base.RaisePropertyChanged();
                }
            }
        }

        public UserDTO SelectedSyncUser
        {
            get => _selectedSyncUser;
            set
            {
                if (value != _selectedSyncUser)
                {
                    _selectedSyncUser = value;
                    base.RaisePropertyChanged();
                }
            }
        }
        public RelayCommand SyncCommand { get; }
        public CalendarWindowViewModel(IBLLServiceMain service)
        {
            _service = service;
            LoadData();
            SyncCommand = new RelayCommand(SyncWithUser);
        }

        private void SyncWithUser()
        {
            if (_selectedSyncUser != null)
            {
                Messenger.Default.Send(new OpenWindowMessage { Type = WindowType.Sync, User = new UserDTO() { UserId = _selectedSyncUser.UserId } });
            }
        }
        private void LoadData()
        {
            try
            {
                _text = 0;
                _text2 = 7;
                Appointments = new ObservableCollection<AppointmentDTO>(_service.GetCalendar());
                Users = new ObservableCollection<UserDTO>(_service.GetUsers());
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

    }
}
