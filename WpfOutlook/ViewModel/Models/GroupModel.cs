﻿using BLL.EntitesDTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private int? _parentId;
        private int _creatorId;

        private ObservableCollection<GroupDTO> _groups;
        private ObservableCollection<UserDTO> _users;

        public GroupModel()
        {
            _users = new ObservableCollection<UserDTO>();
        }

        public int? ParentId
        {
            get => _parentId;
            set
            {
                _parentId = value;
                NotifyPropertyChanged("ParentId");
            }
        }

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

        public int CreatorId
        {
            get => _creatorId;
            set
            {
                _creatorId = value;
                NotifyPropertyChanged("CreatorId"); 
            }
        }

        public ObservableCollection<UserDTO> Users
        {
            get => _users;
            set
            {
                if (value != null)
                {
                    _users = value;
                    NotifyPropertyChanged("Users");
                }
            }
        }

        public ObservableCollection<GroupDTO> Groups
        {
            get => _groups;
            set
            {
                _groups = value;
                NotifyPropertyChanged("Groups");
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
