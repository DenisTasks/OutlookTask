using BLL.EntitesDTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Models
{
    public class UserModel : INotifyPropertyChanged
    {
        public int UserId { get; set; }
        private bool _isActive;
        private string _name;
        private string _userName;
        private string _password;

        private ICollection<GroupDTO> _groups;
        private ICollection<RoleDTO> _roles;
        private ICollection<AppointmentDTO> _appointments;

        public UserModel()
        {
            _groups = new List<GroupDTO>();
            _roles = new List<RoleDTO>();
            _appointments = new List<AppointmentDTO>();
        }

        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                NotifyPropertyChanged("IsActive");
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                NotifyPropertyChanged("Name");
            }
        }

        public string UserName
        {
            get => _userName;
            set
            {
                if (value != string.Empty)
                {
                    _userName = value;
                    NotifyPropertyChanged("UserName");
                }
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if(value != string.Empty)
                {
                    _password = value;
                    NotifyPropertyChanged("Password");
                }
            }
        }

        public ICollection<GroupDTO> Groups
        {
            get => _groups;
            set
            {
                _groups = value;
                NotifyPropertyChanged("Groups");
            }
        }

        public ICollection<RoleDTO> Roles
        {
            get => _roles;
            set
            {
                _roles = value;
                NotifyPropertyChanged("Roles");
            }
        }

        public ICollection<AppointmentDTO> Appointments
        {
            get => _appointments;
            set
            {
                _appointments = value;
                NotifyPropertyChanged("Appointments");
            }
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
