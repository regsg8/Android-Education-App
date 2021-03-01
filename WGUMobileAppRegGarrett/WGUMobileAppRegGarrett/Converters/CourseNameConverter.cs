using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Text;
using WGUMobileAppRegGarrett.Models;
using WGUMobileAppRegGarrett.Services;
using Xamarin.Forms;

namespace WGUMobileAppRegGarrett.Converters
{
    class CourseNameConverter : IValueConverter
    {
        public static ObservableCollection<Course> courses;
        public static void populateCourseNames()
        {
            courses = new ObservableCollection<Course>();
            courses.Clear();
            DB.getCourses();
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            string courseName = "";
            populateCourseNames();
            for (int i = 0; i < courses.Count; i++)
            {
                if (courses[i].CourseId.ToString() == value.ToString())
                {
                    courseName = courses[i].CourseName.ToString();
                }
            }
            return courseName;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            int courseId = -1;
            populateCourseNames();
            for (int i = 0; i < courses.Count; i++)
            {
                if (courses[i].CourseName.ToString() == value.ToString())
                {
                    courseId = courses[i].CourseId;
                }
            }
            return courseId;
        }
    }
}
