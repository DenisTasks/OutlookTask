using BLL.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.ComponentModel;

namespace ViewModel.ViewModels.Administration
{
    public class AdministrationViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private readonly IAdministrationService _administrationService;
        private readonly RelayCommand _showEditUsersView;
        private readonly RelayCommand _showEditGroupsView;
        private readonly RelayCommand _showEditRolesView;

        public RelayCommand ShowEditUsersView { get { return _showEditUsersView; } }
        public RelayCommand ShowEditGroupsView { get { return _showEditGroupsView; } }
        public RelayCommand ShowEditRoleswView { get { return _showEditRolesView; } }



        public AdministrationViewModel(IAdministrationService administrationService)
        {
            _administrationService = administrationService;
            _showEditUsersView = new RelayCommand(ShowUsersWindow);
            _showEditGroupsView = new RelayCommand(ShowGroupsWindow);
            _showEditRolesView = new RelayCommand(ShowRolesWindow);
        }




        private void ShowRolesWindow()
        {
            Messenger.Default.Send(new NotificationMessage("ShowAllRolesPage"));
        }

        private void ShowGroupsWindow()
        {
            Messenger.Default.Send(new NotificationMessage("ShowAllGroupsPage"));
        }

        private void ShowUsersWindow()
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
