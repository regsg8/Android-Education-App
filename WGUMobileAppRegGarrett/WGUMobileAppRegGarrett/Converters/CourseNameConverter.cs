using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using WGUMobileAppRegGarrett.Services;
using Xamarin.Forms;

namespace WGUMobileAppRegGarrett.Converters
{
    class CourseNameConverter : IValueConverter
    {
        public static DataTable courseNames;
        public static void populateCourseNames()
        {
            courseNames = new DataTable();
            courseNames.Clear();
            DB.getCourseNames();
    }
        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            string courseName = "";
            populateCourseNames();
            for(int i = 0; i < courseNames.Rows.Count; i++)
            {
                if (courseNames.Rows[i][0].ToString() == value.ToString())
                {
                    courseName = courseNames.Rows[i][1].ToString();
                }
            }
            return courseName;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            int courseId = -1;
            populateCourseNames();
            for (int i = 0; i < courseNames.Rows.Count; i++)
            {
                if (courseNames.Rows[i][1].ToString() == value.ToString())
                {
                    courseId = int.Parse(courseNames.Rows[i][0].ToString());
                }
            }
            return courseId;
        }
    }
}
