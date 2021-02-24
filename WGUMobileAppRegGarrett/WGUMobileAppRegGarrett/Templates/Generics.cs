using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace WGUMobileAppRegGarrett.Templates
{
    public static class Generics
    {
        public static Grid twoByOneGrid()
        {
            Grid grid = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition()
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition(),
                    new ColumnDefinition()
                }
            };
            return grid;
        }

        public static Grid twoByTwoGrid()
        {
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
            return grid;
        }

        public static Grid twoByThreeGrid()
        {
            Grid grid = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition(),
                    new RowDefinition(),
                    new RowDefinition()
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition(),
                    new ColumnDefinition()
                }
            };
            return grid;
        }
        public static Grid twoByFourGrid()
        {
            Grid grid = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition(),
                    new RowDefinition(),
                    new RowDefinition(),
                    new RowDefinition()
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition(),
                    new ColumnDefinition()
                }
            };
            return grid;
        }

        public static Label label(string style)
        {
            Label label = new Label()
            {
                Style = (Style)Application.Current.Resources[$"{style}Label"]
            };
            return label;
        }
        public static Label label(string style, string text)
        {
            Label label = new Label()
            {
                Style = (Style)Application.Current.Resources[$"{style}Label"],
                Text = text
            };
            return label;
        }

        public static Button button(string style, string text)
        {
            Button button = new Button
            {
                Text = text,
                Style = (Style)Application.Current.Resources[$"{style}Button"]
            };
            return button;
        }

        public static BoxView horizontalLine()
        {
            BoxView line = new BoxView()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HeightRequest = 5,
                Color = (Color)Application.Current.Resources["Secondary"]
            };
            return line;
        }
    }
}
