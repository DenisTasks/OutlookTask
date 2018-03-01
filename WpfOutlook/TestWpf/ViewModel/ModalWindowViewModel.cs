using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace TestWpf.ViewModel
{
    public class ModalWindowViewModel : ViewModelBase
    {
        private string _myText;

        public string MyText
        {
            get
            {
                return _myText;
            }

            set
            {
                if (_myText == value)
                {
                    return;
                }

                _myText = value;
                RaisePropertyChanged(() => MyText);
            }
        }
        public string Name { get; set; }
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public ModalWindowViewModel()
        {
            //OpenModalDialog
            Name = "test";
        }

    }
}
