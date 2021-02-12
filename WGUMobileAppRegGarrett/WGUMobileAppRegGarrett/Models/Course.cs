using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using SQLite;

namespace WGUMobileAppRegGarrett.Models
{
    [Table("Courses")]
    class Course : INotifyPropertyChanged
    {
        [PrimaryKey, AutoIncrement]
        public int CourseId
        {
            get => _courseId;
            set
            {
                _courseId = value;
                RaisePropertyChanged(nameof(CourseId));
            }
        }
        public int InstructorId
        {
            get => _instructorId;
            set
            {
                _instructorId = value;
                RaisePropertyChanged(nameof(InstructorId));
            }
        }
        public string CourseName
        {
            get => _courseName;
            set
            {
                _courseName = value;
                RaisePropertyChanged(nameof(CourseName));
            }
        }

        private int _courseId;
        private int _instructorId;
        private string _courseName;
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
