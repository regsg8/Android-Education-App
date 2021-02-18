using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using WGUMobileAppRegGarrett.Models;
using WGUMobileAppRegGarrett.Services;

namespace WGUMobileAppRegGarrett.ViewModels
{
    public class EnrollmentViewModel : INotifyPropertyChanged
    {
        public static Enrollment currentEnrollment;
        public EnrollmentViewModel()
        {
            currentEnrollment = DB.getEnrollment(TermViewModel.selectedEnrollmentId);
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
