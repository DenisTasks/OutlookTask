using BLL.EntitesDTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Models
{
    public class GroupModel : INotifyPropertyChanged
    {
        public int GroupId { get; set; }
        private string _groupName;
        private string _parentName;
        private string _creatorName;

        private ICollection<UserDTO> _users;

        public string GroupName
        {
            get => _groupName;
            set
            {
                if(value!= string.Empty)
                {
                    _groupName = value;
                    NotifyPropertyChanged("GroupName");
                }
            }
        }

        public string ParentName
        {
            get => _parentName;
            set
            {
                _parentName = value;
                NotifyPropertyChanged("ParentName");
            }
        }

        public string CreatorName
        {
            get => _creatorName;
            set
            {
                _creatorName = value;
                NotifyPropertyChanged("CreatorName");
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
