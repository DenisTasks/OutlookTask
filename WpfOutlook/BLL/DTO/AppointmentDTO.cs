using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BLL.DTO
{
    public class AppointmentDTO : IDataErrorInfo, INotifyPropertyChanged
    {
        private string _subject;

        public int AppointmentId { get; set; }

        public string Subject
        {
            get { return _subject; }
            set
            {
                _subject = value;
                NotifyPropertyChanged("Subject");
            }
        }

        public DateTime BeginningDate { get; set; }
        public DateTime EndingDate { get; set; }
        public int LocationId { get; set; }
        public string Room { get; set; }
        public ICollection<UserDTO> Users { get; set; }

        public AppointmentDTO()
        {
            
        }

        #region IDataErrorInfo
        string IDataErrorInfo.Error
        {
            get
            {
                return null;
            }
        }

        string IDataErrorInfo.this[string propertyName]
        {
            get { return GetValidationError(propertyName); }
        }
        #endregion


        #region Validation
        static readonly string[] ValidatedProperties =
        {
            "Subject"
        };

        string GetValidationError(string propertyName)
        {
            string error = null;

            switch (propertyName)
            {
                case "Subject":
                    error = ValidateSubject();
                    break;
            }

            return error;
        }

        private string ValidateSubject()
        {
            if (String.IsNullOrWhiteSpace(Subject))
            {
                return "Subject can not be empty!";
            }

            return null;
        }
        #endregion


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
