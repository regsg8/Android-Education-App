﻿using System;
using System.Collections.Generic;
using System.Text;
using WGUMobileAppRegGarrett.Converters;
using Xamarin.Forms;

namespace WGUMobileAppRegGarrett.Templates
{
    public class EnrollmentCell : ViewCell
    {
        public EnrollmentCell()
        {
            Label name = new Label()
            {
                Style = (Style)Application.Current.Resources["centerLabel"]
            };
            Label startDate = new Label()
            {
                Style = (Style)Application.Current.Resources["leftLabel"]
            };
            Label endDate = new Label()
            {
                Style = (Style)Application.Current.Resources["leftLabel"]
            };
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
            startDate.SetBinding(Label.TextProperty, "EnrollmentStart", BindingMode.OneWay, new DateConverter());
            endDate.SetBinding(Label.TextProperty, "EnrollmentEnd", BindingMode.OneWay, new DateConverter());

            Grid grid = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition(),
                    new RowDefinition()
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition(),
                    new ColumnDefinition()
                }
            };
            grid.Children.Add(start, 0, 0);
            grid.Children.Add(startDate, 1, 0);
            grid.Children.Add(end, 0, 1);
            grid.Children.Add(endDate, 1, 1);

            StackLayout stack = new StackLayout()
            {
                Padding = new Thickness(5),
                Children =
                {
                    name,
                    grid
                }
            };

            View = stack;
        }
    }
}
