using System;
using System.Collections.Generic;
using System.Text;
using WGUMobileAppRegGarrett.Converters;
using Xamarin.Forms;

namespace WGUMobileAppRegGarrett.Templates
{
    class AssessmentCell : ViewCell
    {
        public AssessmentCell()
        {
            Label name = Generics.label("center");
            Label startDate = Generics.label("left");
            Label startTime = Generics.label("left");
            Label endDate = Generics.label("left");
            Label endTime = Generics.label("left");
            Label start = Generics.label("right", "Start: ");
            Label end = Generics.label("right", "End: ");
            name.SetBinding(Label.TextProperty, new Binding("AssessmentId", BindingMode.OneWay, new AssessmentNameConverter(), null, null));
            startDate.SetBinding(Label.TextProperty, "AssessmentStart", BindingMode.OneWay, new DateConverter());
            endDate.SetBinding(Label.TextProperty, "AssessmentEnd", BindingMode.OneWay, new DateConverter());
            startTime.SetBinding(Label.TextProperty, "AssessmentStart", BindingMode.OneWay, new TimeConverter());
            endTime.SetBinding(Label.TextProperty, "AssessmentEnd", BindingMode.OneWay, new TimeConverter());

            Grid dateGrid = Generics.twoByFourGrid();
            dateGrid.Children.Add(start, 0, 0);
            dateGrid.Children.Add(startDate, 1, 0);
            dateGrid.Children.Add(startTime, 1, 1);
            dateGrid.Children.Add(end, 0, 2);
            dateGrid.Children.Add(endDate, 1, 2);
            dateGrid.Children.Add(endTime, 1, 3);
            BoxView line = Generics.horizontalLine();
            line.Color = (Color)Application.Current.Resources["Neutral"];
            line.HeightRequest = 1;
            StackLayout stack = new StackLayout()
            {
                Padding = new Thickness(5),
                Children =
                {
                    line,
                    name,
                    dateGrid
                }
            };

            View = stack;
        }
    }
}
