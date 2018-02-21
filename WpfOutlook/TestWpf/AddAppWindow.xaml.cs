using System.Windows;
using ViewModel;

namespace TestWpf
{
    /// <summary>
    /// Interaction logic for ModalWindow.xaml
    /// </summary>
    public partial class AddAppWindow : Window
    {
        public AddAppWindow()
        {
            InitializeComponent();
            Closing += (s, e) =>
            {
                DialogResult = true;
                ViewModelLocator.CleanUp();
            };
        }
    }
}
