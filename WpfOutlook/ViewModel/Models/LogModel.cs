using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Models
{
    public class LogModel : INotifyPropertyChanged
    {
        public int LogId { get; set; }
        private string _actionName { get; set; }
        private string _appointmentName { get; set; }
        private string _actionAuthorName { get; set; }
        private string _creatorName { get; set; }
        private DateTime _eventTime { get; set; }

        public string ActionName
        {
            get => _actionName;
            set
            {
                _actionName = value;
                NotifyPropertyChanged("ActionName");
            }
        }

        public string AppointmentName
        {
            get => _appointmentName;
            set
            {
                _appointmentName = value;
                NotifyPropertyChanged("AppointmentName");
            }
        }

        public string ActionAuthorName
        {
            get => _actionAuthorName;
            set
            {
                _actionAuthorName = value;
                NotifyPropertyChanged("ActionAuthorName");
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

        public DateTime EventTime
        {
            get => _eventTime;
            set
            {
                _eventTime = value;
                NotifyPropertyChanged("EventTime");
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
