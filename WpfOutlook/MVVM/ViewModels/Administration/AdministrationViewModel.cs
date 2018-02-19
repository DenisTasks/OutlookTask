using BLL.EntitesDTO;
using MVVM.Helpers;
using MVVM.Interfaces;
using MVVM.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.ViewModels.Administration
{
    public class AdministrationViewModel : IViewModel, INotifyPropertyChanged
    {
        private readonly IAdministrationService _administrationService;
        private readonly DelegateCommand _showEditUsersView;
        private readonly DelegateCommand _showEditGroupsView;
        private readonly DelegateCommand _showEditRolesView;

        public DelegateCommand ShowEditUsersView { get { return _showEditUsersView; } }
        public DelegateCommand ShowEditgroupsView { get { return _showEditGroupsView; } }
        public DelegateCommand ShowEditRoleswView { get { return _showEditRolesView; } }



        public AdministrationViewModel()
        {
            _administrationService = new AdministrationService();
            _showEditUsersView = new DelegateCommand(ShowUsersWindow, null);
            _showEditGroupsView = new DelegateCommand(ShowGroupsWindow, null);
            _showEditRolesView = new DelegateCommand(ShowRolesWindow, null);
        }




        private void ShowRolesWindow(object parameter)
        {
            IView view = new EditRolesWindow();
            view.Show();
        }

        private void ShowGroupsWindow(object parameter)
        {
            IView view = new EditGroupsWindow();
            view.Show();
        }

        private void ShowUsersWindow(object parameter)
        {
            IView view = new EditUsersWindow();
            view.Show();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
