﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using WGUMobileAppRegGarrett.Services;
using Xamarin.Forms;

namespace WGUMobileAppRegGarrett.Converters
{
    class TimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime date = DateTime.Parse(value.ToString());
            return date.ToShortTimeString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<DateTime> datetime = new List<DateTime> { DateTime.Parse(value.ToString()) };
            List<string> date = Validation.convertDates(datetime);
            return date[0];
        }
    }
}
