using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MVVM.ViewModels.CommonViewModels.Groups
{
    /// <summary>
    /// Interaction logic for ShowAllGroupsPage.xaml
    /// </summary>
    [PrincipalPermission(SecurityAction.Demand, Role = "admin")]
    public partial class ShowAllGroupsPage : Page
    {
        public ShowAllGroupsPage()
        {
            InitializeComponent();
        }
    }
}
