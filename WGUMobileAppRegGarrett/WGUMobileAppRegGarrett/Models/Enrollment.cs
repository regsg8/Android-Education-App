using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using SQLite;

namespace WGUMobileAppRegGarrett.Models
{
    [Table("Enrollments")]
    class Enrollment : INotifyPropertyChanged
    {
        [PrimaryKey, AutoIncrement]
        public int EnrollmentId
        {
            get => _enrollmentId;
            set
            {
                _enrollmentId = value;
                RaisePropertyChanged(nameof(EnrollmentId));
            }
        }
        public int TermId
        {
            get => _termId;
            set
            {
                _termId = value;
                RaisePropertyChanged(nameof(TermId));
            }
        }
        public int CourseId
        {
            get => _courseId;
            set
            {
                _courseId = value;
                RaisePropertyChanged(nameof(CourseId));
            }
        }
        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                RaisePropertyChanged(nameof(Status));
            }
        }
        public string Notes
        {
            get => _notes;
            set
            {
                _notes = value;
                RaisePropertyChanged(nameof(Notes));
            }
        }
        //Storing Dates as ISO8601 strings ("YYYY-MM-DD HH:MM:SS.SSS")
        public string EnrollmentStart
        {
            get => _enrollmentStart;
            set
            {
                _enrollmentStart = value;
                RaisePropertyChanged(nameof(EnrollmentStart));
            }
        }
        public string EnrollmentEnd
        {
            get => _enrollmentEnd;
            set
            {
                _enrollmentEnd = value;
                RaisePropertyChanged(nameof(EnrollmentEnd));
            }
        }
        //SQLite stores bool as int, 0 = false, 1 = true
        public int EnrollmentStartNotify
        {
            get => _enrollmentStartNotify;
            set
            {
                _enrollmentStartNotify = value;
                RaisePropertyChanged(nameof(EnrollmentStartNotify));
            }
        }
        public int EntrollmentEndNotify
        {
            get => _enrollmentEndNotify;
            set
            {
                _enrollmentEndNotify = value;
                RaisePropertyChanged(nameof(EntrollmentEndNotify));
            }
        }

        private int _enrollmentId;
        private int _termId;
        private int _courseId;
        private string _status;
        private string _notes;
        private string _enrollmentStart;
        private string _enrollmentEnd;
        private int _enrollmentStartNotify;
        private int _enrollmentEndNotify;
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
