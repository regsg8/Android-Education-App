using System;
using System.Collections.Generic;
using System.Text;
using WGUMobileAppRegGarrett.Models;
using Xamarin.Forms;

namespace WGUMobileAppRegGarrett.Services
{
    class Auth
    {
        public static Student login = new Student
        {
            StudentId = -2,
            Name = "login",
            Username = "",
            Password = ""
        };
        public static bool loggedIn = false;
        public static Student user;

        //Checks if user is logged in
        public static void loginCheck(ContentPage page)
        {
            if (!loggedIn)
            {
                //Sets binding context to the Student login
                page.BindingContext = login;
                //Adds login view to page
                page.Content = new ScrollView
                {
                    Content = createLoginView()
                };
                //Adds login toolbar button to page
                ToolbarItem loginToolbar = new ToolbarItem
                {
                    Text = "Login"
                };
                loginToolbar.Clicked += loginClicked;
                page.ToolbarItems.Add(loginToolbar);
            }
            else
            {
                //Adds logout toolbar button to page
                ToolbarItem logoutButton = new ToolbarItem
                {
                    Text = "Logout"
                };
                logoutButton.Clicked += logoutClicked;
                page.ToolbarItems.Add(logoutButton);
            }
        }

        //Creates login UI content
        private static StackLayout createLoginView()
        {
            Label usernameLabel = new Label
            {
                Text = "Username",
                Style = (Style)Application.Current.Resources["centerLabel"]
            };
            Entry usernameEntry = new Entry
            {
                StyleId = "usernameEntry",
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            StackLayout username = new StackLayout
            {
                Children =
                    {
                        usernameLabel,
                        usernameEntry
                    }
            };
            usernameEntry.SetBinding(Entry.TextProperty, "Username", mode: BindingMode.TwoWay);
            Label passwordLabel = new Label
            {
                Text = "Password",
                Style = (Style)Application.Current.Resources["centerLabel"],
                Margin = new Thickness(60, 0)
            };
            Entry passwordEntry = new Entry
            {
                IsPassword = true,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            passwordEntry.SetBinding(Entry.TextProperty, "Password", mode: BindingMode.TwoWay);
            StackLayout password = new StackLayout
            {
                Children =
                        {
                            passwordLabel,
                            passwordEntry
                        },
                Margin = new Thickness(0, 20, 0, 0)
            };
            StackLayout entries = new StackLayout
            {
                Children =
                        {
                            username,
                            password
                        },
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Margin = new Thickness(80)
            };
            Button loginButton = new Button
            {
                VerticalOptions = LayoutOptions.StartAndExpand,
                Text = "Login"
            };
            loginButton.Clicked += loginClicked;

            StackLayout stackLayout = new StackLayout
            {
                Children =
                {
                    entries,
                    loginButton
                }
            };

            return stackLayout;
        }

        //Logs user in
        public async static void loginClicked(object sender, EventArgs args)
        {
            try
            {
                //Check for empty entries
                if (login.Username == "") throw new Exception("Please enter a Username.");
                if (login.Password == "") throw new Exception("Please enter a Password.");
                //Use Student login binding info
                DB.studentLogin(login.Username, login.Password);
            }
            catch (Exception x)
            {
                Console.WriteLine(x.Message);
                await Application.Current.MainPage.DisplayAlert("Error", x.Message, "OK");
            }
        }

        //Logs user out
        public static void logoutClicked(object sender, EventArgs args)
        {
            loggedIn = false;
            login.Username = "";
            login.Password = "";
            Application.Current.MainPage = new AppShell();
        }
    }
}
