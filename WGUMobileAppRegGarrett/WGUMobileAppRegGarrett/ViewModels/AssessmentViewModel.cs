using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using WGUMobileAppRegGarrett.Models;
using WGUMobileAppRegGarrett.Services;

namespace WGUMobileAppRegGarrett.ViewModels
{
    public class AssessmentViewModel : INotifyPropertyChanged
    {
        public static Assessment currentAssessment;
        public static Assessment newAssessment;
        
        public AssessmentViewModel(int assessmentId)
        {
            currentAssessment = DB.getAssessment(assessmentId);
        }

        
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
