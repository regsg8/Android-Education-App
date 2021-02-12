using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using SQLite;

namespace WGUMobileAppRegGarrett.Models
{
    [Table("Terms")]
    class Term : INotifyPropertyChanged
    {
        [PrimaryKey, AutoIncrement]
        public int TermId
        {
            get => _termId;
            set
            {
                _termId = value;
                RaisePropertyChanged(nameof(TermId));
            }
        }
        public int DegreeId
        {
            get => _degreeId;
            set
            {
                _degreeId = value;
                RaisePropertyChanged(nameof(DegreeId));
            }
        }
        public string TermName
        {
            get => _termName;
            set
            {
                _termName = value;
                RaisePropertyChanged(nameof(TermName));
            }
        }
        //Storing Dates as ISO8601 strings ("YYYY-MM-DD HH:MM:SS.SSS")
        public string TermStart
        {
            get => _termStart;
            set
            {
                _termStart = value;
                RaisePropertyChanged(nameof(TermStart));
            }
        }
        public string TermEnd
        {
            get => _termEnd;
            set
            {
                _termEnd = value;
                RaisePropertyChanged(nameof(TermEnd));
            }
        }

        private int _termId;
        private int _degreeId;
        private string _termName;
        private string _termStart;
        private string _termEnd;
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
