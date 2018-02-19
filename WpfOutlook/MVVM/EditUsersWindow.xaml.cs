using GalaSoft.MvvmLight.Messaging;
using MVVM.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MVVM
{
    /// <summary>
    /// Interaction logic for EditUsersWindow.xaml
    /// </summary>
    public partial class EditUsersWindow : Window, IView
    {
        public EditUsersWindow()
        {
            InitializeComponent();
        }

        public IViewModel ViewModel { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void NotificationMessageReceived(NotificationMessage obj)
        {
            throw new NotImplementedException();
        }
    }
}
