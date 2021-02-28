using System;
using System.Collections.Generic;
using System.Text;
using WGUMobileAppRegGarrett.Converters;
using WGUMobileAppRegGarrett.ViewModels;
using Xamarin.Forms;

namespace WGUMobileAppRegGarrett.Templates
{
    class CourseCellEdit : ViewCell
    {
        public CourseCellEdit()
        {
             Entry name = new Entry()
             {
                 Style = (Style)Application.Current.Resources["title"],
                 HorizontalOptions = LayoutOptions.FillAndExpand
             };
            name.SetBinding(Entry.TextProperty, "CourseName", BindingMode.TwoWay);
            //Instructor
            Label instructor = Generics.label("right", "Instructor: ");
            Label boundName = Generics.label("left");
            boundName.SetBinding(Label.TextProperty, "InstructorId", BindingMode.TwoWay, new InstructorNameConverter());
            InstructorNameConverter.populateInstructors();
            Label changeInstructor = Generics.label("right", "New Instructor: ");
            Picker instructorChange = new Picker()
            {
                Title = "New Instructor"
            };
            instructorChange.ItemsSource = InstructorNameConverter.instructorNames;
            instructorChange.SetBinding(Picker.SelectedItemProperty, "InstructorId", BindingMode.TwoWay, new InstructorNameConverter());
            Grid instructorGrid = Generics.twoByTwoGrid();
            instructorGrid.Children.Add(instructor, 0, 0);
            instructorGrid.Children.Add(boundName, 1, 0);
            instructorGrid.Children.Add(changeInstructor, 0, 1);
            instructorGrid.Children.Add(instructorChange, 1, 1);
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
