using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using SQLite;

namespace WGUMobileAppRegGarrett.Models
{
    [Table("Degrees")]
    class Degree : INotifyPropertyChanged
    {
        [PrimaryKey, AutoIncrement]
        public int DegreeId
        {
            get => _degreeId;
            set
            {
                _degreeId = value;
                RaisePropertyChanged(nameof(DegreeId));
            }
        }
        public int StudentId
        {
            get => _studentId;
            set
            {
                _studentId = value;
                RaisePropertyChanged(nameof(StudentId));
            }
        }
        public string DegreeName
        {
            get => _degreeName;
            set
            {
                _degreeName = value;
                RaisePropertyChanged(nameof(DegreeName));
            }
        }
        //SQLite stores bool as int, 0 = false, 1 = true
        public int DegreeActive
        {
            get => _degreeActive;
            set
            {
                _degreeActive = value;
                RaisePropertyChanged(nameof(DegreeActive));
            }
        }

        private int _degreeId;
        private int _studentId;
        private string _degreeName;
        private int _degreeActive;
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
