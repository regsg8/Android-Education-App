using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace WGUMobileAppRegGarrett.Templates
{
    public static class Generics
    {
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
    }
}
