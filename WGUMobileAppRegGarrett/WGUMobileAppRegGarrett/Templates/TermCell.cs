using System;
using System.Collections.Generic;
using System.Text;
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
            Label start = new Label()
            {
                Style = (Style)Application.Current.Resources["centerLabel"]
            };
            Label end = new Label()
            {
                Style = (Style)Application.Current.Resources["centerLabel"]
            };

            name.SetBinding(Label.TextProperty, "TermName");
            start.SetBinding(Label.TextProperty, "TermStart");
            end.SetBinding(Label.TextProperty, "TermEnd");

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
