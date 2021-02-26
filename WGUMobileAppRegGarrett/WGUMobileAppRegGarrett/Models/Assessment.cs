using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using SQLite;

namespace WGUMobileAppRegGarrett.Models
{
    [Table("Assessments")]
    public class Assessment : INotifyPropertyChanged
    {
        [PrimaryKey, AutoIncrement]
        public int AssessmentId
        {
            get => _assessmentId;
            set
            {
                _assessmentId = value;
                RaisePropertyChanged(nameof(AssessmentId));
            }
        }
        public int EnrollmentId
        {
            get => _enrollmentId;
            set
            {
                _enrollmentId = value;
                RaisePropertyChanged(nameof(EnrollmentId));
            }
        }
        public string AssessmentName
        {
            get => _name;
            set
            {
                _name = value;
                RaisePropertyChanged(nameof(AssessmentName));
            }
        }
        public string Type
        {
            get => _type;
            set
            {
                _type = value;
                RaisePropertyChanged(nameof(Type));
            }
        }
        //Storing Dates as ISO8601 strings ("YYYY-MM-DD HH:MM:SS.SSS")
        public string AssessmentDue
        {
            get => _assessmentDue;
            set
            {
                _assessmentDue = value;
                RaisePropertyChanged(nameof(AssessmentDue));
            }
        }
        //SQLite stores bool as int, 0 = false, 1 = true
        public int AssessmentDueNotify
        {
            get => _assessmentDueNotify;
            set
            {
                _assessmentDueNotify = value;
                RaisePropertyChanged(nameof(AssessmentDueNotify));
            }
        }

        private int _assessmentId;
        private int _enrollmentId;
        private string _name;
        private string _type;
        private string _assessmentDue;
        private int _assessmentDueNotify;
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
