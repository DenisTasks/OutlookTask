using System;
using System.Collections.ObjectModel;
using System.Windows;
using BLL.DTO;
using BLL.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace ViewModel.ViewModels
{
    public class CalendarWindowViewModel : ViewModelBase
    {
        private readonly IBLLService _service;
        private ObservableCollection<AppointmentDTO> _appointments;
        private ObservableCollection<AppointmentDTO> _appointmentsOther;
        private ObservableCollection<UserDTO> _users;
        private UserDTO _selectedSyncUser;

        private Visibility _enabled;
        public Visibility Enabled
        {
            get => _enabled;
            set
            {
                if (value != _enabled)
                {
                    _enabled = value;
                    base.RaisePropertyChanged();
                }
            }
        }
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
        public CalendarWindowViewModel(IBLLService service)
        {
            //Enabled = Visibility.Collapsed;
            _service = service;
            LoadData();
            SyncCommand = new RelayCommand(SyncWithUser);
        }

        private void SyncWithUser()
        {
            if (_selectedSyncUser != null)
            {
                //if (Enabled == Visibility.Collapsed)
                //{
                //AppointmentsSync = new ObservableCollection<AppointmentDTO>(_service.GetCalendar().Where(s => s.Users.Any(x => x.UserId == _selectedSyncUser.UserId)).ToList());


                //Enabled = Visibility.Visible;
                //}
                //else
                //{
                //AppointmentsSync.Clear();
                //view.Refresh();
                //Enabled = Visibility.Collapsed;
                //}
            }
        }
        private void LoadData()
        {
            try
            {
                Appointments = new ObservableCollection<AppointmentDTO>(_service.GetCalendar());
                AppointmentsSync = new ObservableCollection<AppointmentDTO>(_service.GetCalendar());

                Users = new ObservableCollection<UserDTO>(_service.GetUsers());
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

    }
}
