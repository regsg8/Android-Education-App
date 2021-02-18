﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using WGUMobileAppRegGarrett.Models;
using WGUMobileAppRegGarrett.Services;
using Xamarin.Forms;

namespace WGUMobileAppRegGarrett.Converters
{
    class AssessmentNameConverter : IValueConverter
    {
        public static ObservableCollection<Assessment> assessments;
        public static void populateCourseNames()
        {
            assessments = new ObservableCollection<Assessment>();
            assessments.Clear();
            DB.getAssessments();
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            string assessmentName = "";
            populateCourseNames();
            for (int i = 0; i < assessments.Count; i++)
            {
                if (assessments[i].AssessmentId.ToString() == value.ToString())
                {
                    assessmentName = assessments[i].AssessmentName.ToString();
                }
            }
            return assessmentName;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            int assessmentId = -1;
            populateCourseNames();
            for (int i = 0; i < assessments.Count; i++)
            {
                if (assessments[i].AssessmentName.ToString() == value.ToString())
                {
                    assessmentId = assessments[i].AssessmentId;
                }
            }
            return assessmentId;
        }
    }
}