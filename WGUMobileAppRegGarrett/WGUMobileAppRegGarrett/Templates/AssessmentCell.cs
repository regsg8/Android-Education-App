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
            Label type = Generics.label("center");
            Label endDate = Generics.label("left");
            Label end = Generics.label("right", "Due: ");
            name.SetBinding(Label.TextProperty, new Binding("AssessmentId", BindingMode.OneWay, new AssessmentNameConverter(), null, null));
            type.SetBinding(Label.TextProperty, new Binding("Type", BindingMode.OneWay));
            endDate.SetBinding(Label.TextProperty, "AssessmentDue", BindingMode.OneWay, new DateConverter());

            Grid dateGrid = Generics.twoByOneGrid();
            dateGrid.Children.Add(end, 0, 0);
            dateGrid.Children.Add(endDate, 1, 0);
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
                    type,
                    dateGrid
                }
            };

            View = stack;
        }
    }
}
