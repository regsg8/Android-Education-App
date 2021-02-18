using System;
using System.Collections.Generic;
using System.Text;
using WGUMobileAppRegGarrett.Converters;
using Xamarin.Forms;

namespace WGUMobileAppRegGarrett.Templates
{
    public class TermCellEdit : ViewCell
    {
        public TermCellEdit()
        {
            Label name = new Label()
            {
                Style = (Style)Application.Current.Resources["rightLabel"],
                Text = "Term Name: "
            };
            Entry nameEntry = new Entry();
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

            nameEntry.SetBinding(Entry.TextProperty, "TermName", BindingMode.TwoWay);
            startDate.SetBinding(DatePicker.DateProperty, "TermStart", BindingMode.TwoWay, new DateConverter());
            endDate.SetBinding(DatePicker.DateProperty, "TermEnd", BindingMode.TwoWay, new DateConverter());

            Grid grid = Generics.twoByThreeGrid();
            grid.Children.Add(name, 0, 0);
            grid.Children.Add(nameEntry, 1, 0);
            grid.Children.Add(start, 0, 1);
            grid.Children.Add(startDate, 1, 1);
            grid.Children.Add(end, 0, 2);
            grid.Children.Add(endDate, 1, 2);
            BoxView line = Generics.horizontalLine();
            line.Color = (Color)Application.Current.Resources["Neutral"];
            line.HeightRequest = 1;
            StackLayout stack = new StackLayout()
            {
                Padding = new Thickness(5),
                Margin = new Thickness(5),
                Children =
                {
                    line,
                    grid
                }
            };

            View = stack;
        }
    }
}
