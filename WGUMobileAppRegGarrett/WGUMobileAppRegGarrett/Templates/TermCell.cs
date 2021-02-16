using System;
using System.Collections.Generic;
using System.Text;
using WGUMobileAppRegGarrett.Converters;
using Xamarin.Forms;

namespace WGUMobileAppRegGarrett.Templates
{
    public class TermCell : ViewCell
    {
        public TermCell()
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

            name.SetBinding(Label.TextProperty, "TermName");
            startDate.SetBinding(Label.TextProperty, "TermStart", BindingMode.OneWay, new DateConverter());
            endDate.SetBinding(Label.TextProperty, "TermEnd", BindingMode.OneWay, new DateConverter());

            Grid dateGrid = new Grid
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
