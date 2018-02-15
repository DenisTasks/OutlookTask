using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Model.Entities;
using ViewModel.Interfaces;

namespace TestWpf.ViewModel
{
    public class AddAppWindowViewModel : ViewModelBase
    {
        private readonly IBLLService _service;
        public RelayCommand CreateAppCommand { get; }

        public List<string> BeginningTime { get; }
        public List<string> EndingTime { get; }
        public List<Location> LocationList { get; }
        public string selectedBeginningTime { get; set; }
        public string selectedEndingTime { get; set; }
        public string inputSubject { get; set; }
        public Location selectedLocation { get; set; }

        public AddAppWindowViewModel(IBLLService service)
        {
            _service = service;
            CreateAppCommand = new RelayCommand(CreateAppointment);

            Location loc = new Location() {LocationId = 1, Room = "Shagal"};
            LocationList = new List<Location>() {loc};
            BeginningTime = new List<string>() { DateTime.Now.ToString("h:mm tt") };
            EndingTime = new List<string>() { DateTime.Now.ToString("h:mm tt") };

        }

        public void CreateAppointment()
        {
            MessageBox.Show(selectedBeginningTime + " " + selectedEndingTime + " " + selectedLocation.Room + " " + inputSubject);
        }
    }
}
