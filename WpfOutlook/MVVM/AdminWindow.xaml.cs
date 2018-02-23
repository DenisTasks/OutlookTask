using System.Windows;

namespace MVVM
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    [PrincipalPermission(SecurityAction.Demand, Role = "admin")]
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
        }
    }
}
