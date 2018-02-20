using BLL.EntitesDTO;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MVVM.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.ViewModels.Administration
{
    public class EditRolesViewModel: ViewModelBase, INotifyPropertyChanged
    {
        private ObservableCollection<RoleDTO> _roles;
        private RelayCommand _addRoleCommand;

        public EditRolesViewModel()
        {

        }

        public RelayCommand AddRoleCommand { get { return _addRoleCommand; } }


        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
