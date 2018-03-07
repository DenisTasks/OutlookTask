﻿using System;
using System.Collections.ObjectModel;
using System.Windows;
using BLL.EntitesDTO;
using BLL.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using ViewModel.Helpers;

namespace ViewModel.ViewModels.Calendar
{
    public class SyncWindowViewModel: ViewModelBase
    {
        public int MyStartDay { get; set; }
        public int MyFinishDay { get; set; }
        public int SyncStartDay { get; set; }
        public int SyncFinishDay { get; set; }

        private readonly IBLLServiceMain _service;
        private ObservableCollection<AppointmentDTO> _appointments;
        private ObservableCollection<AppointmentDTO> _appointmentsOther;

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

        public SyncWindowViewModel(IBLLServiceMain service)
        {
            MyStartDay = 0;
            MyFinishDay = 7;
            SyncStartDay = 0;
            SyncFinishDay = 7;
            _service = service;

            Messenger.Default.Register<OpenWindowMessage>(this, message =>
            {
                if (message.Type == WindowType.None && message.User != null)
                {
                    LoadData(message.User.UserId);
                }
            });
        }

        private void LoadData(int id)
        {
            try
            {
                Appointments = new ObservableCollection<AppointmentDTO>(_service.GetCalendar());
                AppointmentsSync = new ObservableCollection<AppointmentDTO>(_service.GetCalendarByUserId(id));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

    }
}
