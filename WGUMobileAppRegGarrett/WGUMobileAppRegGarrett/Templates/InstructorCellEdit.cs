using System;
using System.Collections.Generic;
using System.Text;
using WGUMobileAppRegGarrett.Converters;
using WGUMobileAppRegGarrett.ViewModels;
using Xamarin.Forms;

namespace WGUMobileAppRegGarrett.Templates
{
    class InstructorCellEdit : ViewCell
    {
        public InstructorCellEdit()
        {
            Entry name = new Entry()
            {
                Style = (Style)Application.Current.Resources["title"],
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            name.SetBinding(Entry.TextProperty, "InstructorName", BindingMode.TwoWay);
            //Instructor
            Label email = Generics.label("right", "Email: ");
            Entry boundEmail = new Entry();
            boundEmail.SetBinding(Entry.TextProperty, "Email", BindingMode.TwoWay);
            InstructorNameConverter.populateInstructors();
            Label phone = Generics.label("right", "Phone: ");
            Entry boundPhone = new Entry();
            boundPhone.SetBinding(Entry.TextProperty, "Phone", BindingMode.TwoWay);
            Grid instructorGrid = Generics.twoByTwoGrid();
            instructorGrid.Children.Add(email, 0, 0);
            instructorGrid.Children.Add(boundEmail, 1, 0);
            instructorGrid.Children.Add(phone, 0, 1);
            instructorGrid.Children.Add(boundPhone, 1, 1);
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
                    instructorGrid
                }
            };

            View = stack;
        }
    }
}
