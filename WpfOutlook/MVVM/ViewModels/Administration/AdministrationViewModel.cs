using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using MVVM.Helpers;
using MVVM.Interfaces;
using System.ComponentModel;

namespace MVVM.ViewModels.Administration
{
    public class AdministrationViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private readonly IAdministrationService _administrationService;
        private readonly DelegateCommand _showEditUsersView;
        private readonly DelegateCommand _showEditGroupsView;
        private readonly DelegateCommand _showEditRolesView;

        public DelegateCommand ShowEditUsersView { get { return _showEditUsersView; } }
        public DelegateCommand ShowEditGroupsView { get { return _showEditGroupsView; } }
        public DelegateCommand ShowEditRoleswView { get { return _showEditRolesView; } }



        public AdministrationViewModel(IAdministrationService administrationService)
        {
            _administrationService = administrationService;
            _showEditUsersView = new DelegateCommand(ShowUsersWindow, null);
            _showEditGroupsView = new DelegateCommand(ShowGroupsWindow, null);
            _showEditRolesView = new DelegateCommand(ShowRolesWindow, null);
        }




        private void ShowRolesWindow(object parameter)
        {
            Messenger.Default.Send(new NotificationMessage("ShowAllRolesPage"));
        }

        private void ShowGroupsWindow(object parameter)
        {
            Messenger.Default.Send(new NotificationMessage("ShowAllUsersPage"));
        }

        private void ShowUsersWindow(object parameter)
        {
            Messenger.Default.Send(new NotificationMessage("ShowAllUsersPage"));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
