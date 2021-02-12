using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using SQLite;

namespace WGUMobileAppRegGarrett.Models
{
    [Table("Instructors")]
    class Instructor : INotifyPropertyChanged
    {
        [PrimaryKey, AutoIncrement]
        public int InstructorId
        {
            get => _instructorId;
            set
            {
                _instructorId = value;
                RaisePropertyChanged(nameof(InstructorId));
            }
        }
        public string InstructorName
        {
            get => _instructorName;
            set
            {
                _instructorName = value;
                RaisePropertyChanged(nameof(InstructorName));
            }
        }
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                RaisePropertyChanged(nameof(Email));
            }
        }
        public string Phone
        {
            get => _phone;
            set
            {
                _phone = value;
                RaisePropertyChanged(nameof(Phone));
            }
        }

        private int _instructorId;
        private string _instructorName;
        private string _email;
        private string _phone;
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
