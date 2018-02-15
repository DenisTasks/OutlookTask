using System.Windows;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using TestWpf.Helpers;
using TestWpf.ViewModel;

namespace TestWpf
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Messenger.Default.Register<OpenWindowMessage>(
                this,
                message => {
                    if (message.Type == WindowType.AddAppWindow)
                    {
                        var addAppWindowVM = SimpleIoc.Default.GetInstance<AddAppWindowViewModel>();
                        var addAppWindow = new AddAppWindow()
                        {
                            DataContext = addAppWindowVM
                        };
                        var result = addAppWindow.ShowDialog() ?? false;
                    }
                });
        }
    }
}
