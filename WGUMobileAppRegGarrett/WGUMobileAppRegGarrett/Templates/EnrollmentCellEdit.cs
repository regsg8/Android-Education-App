﻿using System;
using System.Collections.Generic;
using System.Text;
using WGUMobileAppRegGarrett.Converters;
using Xamarin.Forms;

namespace WGUMobileAppRegGarrett.Templates
{
    public class EnrollmentCellEdit : ViewCell
    {
        public EnrollmentCellEdit()
        {
            Label name = new Label()
            {
                Style = (Style)Application.Current.Resources["centerLabel"]
            };
            DatePicker startDate = new DatePicker();
            DatePicker endDate = new DatePicker();
            Label start = new Label()
            {
                Style = (Style)Application.Current.Resources["rightLabel"],
                Text = "Start: "
            };
            Label end = new Label()
            {
                Style = (Style)Application.Current.Resources["rightLabel"],
                Text = "End: "
            };
            name.SetBinding(Label.TextProperty, new Binding("CourseId", BindingMode.OneWay, new CourseNameConverter(), null, null));
            startDate.SetBinding(DatePicker.DateProperty, "EnrollmentStart", BindingMode.TwoWay, new DateConverter());
            endDate.SetBinding(DatePicker.DateProperty, "EnrollmentEnd", BindingMode.TwoWay, new DateConverter());

            Grid dateGrid = Generics.twoByTwoGrid();
            dateGrid.Children.Add(start, 0, 0);
            dateGrid.Children.Add(startDate, 1, 0);
            dateGrid.Children.Add(end, 0, 1);
            dateGrid.Children.Add(endDate, 1, 1);

            StackLayout stack = new StackLayout()
            {
                Padding = new Thickness(5),
                Children =
                {
                    name,
                    dateGrid
                }
            };

            View = stack;
        }
    }
}
