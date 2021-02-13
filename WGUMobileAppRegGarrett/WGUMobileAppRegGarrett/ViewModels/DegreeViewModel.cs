using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using WGUMobileAppRegGarrett.Models;
using WGUMobileAppRegGarrett.Services;

namespace WGUMobileAppRegGarrett.ViewModels
{
    public class DegreeViewModel : INotifyPropertyChanged
    {
        public static Degree degree { get; set; }
        public static ObservableCollection<Term> terms;

        public DegreeViewModel()
        {
            terms = new ObservableCollection<Term>();
            DB.getActiveDegree(Auth.user.StudentId);
            DB.getDegreeTerms(degree.DegreeId);
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
