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
        public static Term currentTerm;
        public static ObservableCollection<Enrollment> enrollments;
        public TermViewModel()
        {
            enrollments = new ObservableCollection<Enrollment>();
            currentTerm = new Term();
            checkTerms();
        }
        private void checkTerms()
        {
            if (DegreeViewModel.terms.Count == 0)
            {
                currentTerm = new Term()
                {
                    TermName = "No terms available",
                    TermId = -1
                };
            }
            else if (DegreeViewModel.selectedTermId == -1)
            {
                currentTerm = DegreeViewModel.terms[DegreeViewModel.terms.Count - 1];
            }
            else 
            {
                currentTerm = DB.getTerm(DegreeViewModel.selectedTermId);
            }  
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
