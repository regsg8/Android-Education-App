using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using WGUMobileAppRegGarrett.Models;
using WGUMobileAppRegGarrett.Services;

namespace WGUMobileAppRegGarrett.ViewModels
{
    public class TermViewModel : INotifyPropertyChanged
    {
        public static Term currentTerm { get; set; }
        public static ObservableCollection<Enrollment> enrollments;
        public TermViewModel()
        {
            enrollments = new ObservableCollection<Enrollment>();
            getCurrentTerm();
            DB.getTermEnrollments(currentTerm.TermId);
        }
        private void getCurrentTerm()
        {
            //For now just get most recent term, need to add ability to populate selected term
            currentTerm = DegreeViewModel.terms[DegreeViewModel.terms.Count - 1];
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
