﻿using GalaSoft.MvvmLight.Messaging;
using MVVM.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window, IView
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        //[Dependency]
        public IViewModel ViewModel
        {
            get { return DataContext as IViewModel; }
            set { DataContext = value;  }
        }

        public void NotificationMessageReceived(NotificationMessage obj)
        {
            throw new NotImplementedException();
        }
    }
}
