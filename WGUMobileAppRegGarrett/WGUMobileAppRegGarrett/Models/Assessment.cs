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
        public string AssessmentStart
        {
            get => _assessmentStart;
            set
            {
                _assessmentStart = value;
                RaisePropertyChanged(nameof(AssessmentStart));
            }
        }
        public string AssessmentEnd
        {
            get => _assessmentEnd;
            set
            {
                _assessmentEnd = value;
                RaisePropertyChanged(nameof(AssessmentEnd));
            }
        }
        //SQLite stores bool as int, 0 = false, 1 = true
        public int AssessmentStartNotify
        {
            get => _assessmentStartNotify;
            set
            {
                _assessmentStartNotify = value;
                RaisePropertyChanged(nameof(AssessmentStartNotify));
            }
        }
        public int AssessmentEndNotify
        {
            get => _assessmentEndNotify;
            set
            {
                _assessmentEndNotify = value;
                RaisePropertyChanged(nameof(AssessmentEndNotify));
            }
        }

        private int _assessmentId;
        private int _enrollmentId;
        private string _name;
        private string _type;
        private string _assessmentStart;
        private string _assessmentEnd;
        private int _assessmentStartNotify;
        private int _assessmentEndNotify;
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
