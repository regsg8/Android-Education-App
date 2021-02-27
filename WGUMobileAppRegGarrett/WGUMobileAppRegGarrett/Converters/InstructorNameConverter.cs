using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using WGUMobileAppRegGarrett.Models;
using WGUMobileAppRegGarrett.Services;
using Xamarin.Forms;

namespace WGUMobileAppRegGarrett.Converters
{
    class InstructorNameConverter : IValueConverter
    {
        public static ObservableCollection<Instructor> instructors;
        public static ObservableCollection<string> instructorNames;
        public static void populateInstructors()
        {
            instructors = new ObservableCollection<Instructor>();
            instructorNames = new ObservableCollection<string>();
            instructors.Clear();
            instructorNames.Clear();
            DB.getInstructors();

        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            string instructorName = "";
            populateInstructors();
            for (int i = 0; i < instructors.Count; i++)
            {
                if (instructors[i].InstructorId.ToString() == value.ToString())
                {
                    instructorName = instructors[i].InstructorName.ToString();
                }
            }
            return instructorName;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            int instructorId = -1;
            populateInstructors();
            for (int i = 0; i < instructors.Count; i++)
            {
                if (instructors[i].InstructorName.ToString() == value.ToString())
                {
                    instructorId = instructors[i].InstructorId;
                }
            }
            return instructorId;
        }
    }
}
