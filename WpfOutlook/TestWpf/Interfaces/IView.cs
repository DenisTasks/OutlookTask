using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Interfaces;

namespace TestWpf.Interfaces
{
    interface IView
    {
        IViewModel IViewModel { get; set; }
        void Show();
    }
}
