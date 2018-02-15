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
            Closing += (s, e) => ViewModelLocator.Cleanup();
            Messenger.Default.Register<OpenWindowMessage>(
                this,
                message => {
                    if (message.Type == WindowType.kModal)
                    {
                        var modalWindowVM = SimpleIoc.Default.GetInstance<ModalWindowViewModel>();
                        modalWindowVM.MyText = message.Argument;
                        var modalWindow = new ModalWindow()
                        {
                            DataContext = modalWindowVM
                        };
                        var result = modalWindow.ShowDialog() ?? false;
                        Messenger.Default.Send(result ? "Accepted" : "Rejected");
                    }
                    else
                    {
                        // if nonModal
                    }
                });
        }
    }
}
