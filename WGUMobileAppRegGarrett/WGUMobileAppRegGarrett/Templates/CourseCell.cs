using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace WGUMobileAppRegGarrett.Templates
{
    public class CourseCell : ViewCell
    {
        public CourseCell()
        {
            Label name = new Label()
            {
                Style = (Style)Application.Current.Resources["centerLabel"]
            };
            Label start = new Label()
            {
                Style = (Style)Application.Current.Resources["centerLabel"]
            };
            Label end = new Label()
            {
                Style = (Style)Application.Current.Resources["centerLabel"]
            };

            name.SetBinding(Label.TextProperty, "CourseName");
            start.SetBinding(Label.TextProperty, "CourseStart");
            end.SetBinding(Label.TextProperty, "CourseEnd");

            StackLayout stack = new StackLayout()
            {
                Padding = new Thickness(5),
                Children =
                {
                    name,
                    start,
                    end
                }
            };

            View = stack;
        }
    }
}
