using System.Windows;

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
            };
        }
    }
}
