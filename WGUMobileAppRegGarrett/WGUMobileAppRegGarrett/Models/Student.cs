using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using SQLite;

namespace WGUMobileAppRegGarrett.Models
{
    [Table("Students")]
    public class Student : INotifyPropertyChanged
    {
        [PrimaryKey, AutoIncrement]
        public int StudentId 
        { 
            get => _id;
            set 
            {
                _id = value;
                RaisePropertyChanged(nameof(StudentId));
            }
        }
        public string Name 
        { 
            get => _name;
            set 
            {
                _name = value;
                RaisePropertyChanged(nameof(Name));
            }
        }
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                RaisePropertyChanged(nameof(Username));
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                RaisePropertyChanged(nameof(Password));
            }
        }
        private int _id;
        private string _name;
        private string _username;
        private string _password;
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
