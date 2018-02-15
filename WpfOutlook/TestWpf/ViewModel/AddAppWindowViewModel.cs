using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using ViewModel.Interfaces;

namespace TestWpf.ViewModel
{
    public class AddAppWindowViewModel : ViewModelBase
    {
        private readonly IBLLService _service;
        public string Name { get; set; }

        public AddAppWindowViewModel(IBLLService service)
        {
            _service = service;
            Name = "test";
        }
    }
}
