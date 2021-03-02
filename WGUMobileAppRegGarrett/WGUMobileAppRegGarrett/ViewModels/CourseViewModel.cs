using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using WGUMobileAppRegGarrett.Models;
using WGUMobileAppRegGarrett.Services;

namespace WGUMobileAppRegGarrett.ViewModels
{
    public class CourseViewModel : INotifyPropertyChanged
    {
        public static Course newCourse;
        public static ObservableCollection<Course> CVMCourses;
        public CourseViewModel()
        {
            
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
