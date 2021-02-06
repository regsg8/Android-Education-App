using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WGUMobileAppRegGarrett.Services;
using WGUMobileAppRegGarrett.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WGUMobileAppRegGarrett.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            Label titleLabel = new Label
            {
                Text = "WGU Login",
                FontSize = Device.GetNamedSize(NamedSize.Title, typeof(Label)),
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.White,
                HorizontalTextAlignment = TextAlignment.Center,
                Padding = new Thickness (80),
                BackgroundColor = (Color)Application.Current.Resources["Primary"]
            };
            Label usernameLabel = new Label
            {
                Text = "Username",
                Style = (Style)Application.Current.Resources["centerLabel"]
            };
            Entry usernameEntry = new Entry
            {
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
            Label passwordLabel = new Label
            {
                Text = "Password",
                Style = (Style)Application.Current.Resources["centerLabel"],
                Margin = new Thickness (60, 0)
            };
            Entry passwordEntry = new Entry
            {
                IsPassword = true,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            StackLayout password = new StackLayout
            {
                Children =
                {
                    passwordLabel,
                    passwordEntry
                }
            };
            StackLayout entries = new StackLayout
            {
                Children =
                {
                    username,
                    password
                },
                VerticalOptions = LayoutOptions.StartAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Margin = new Thickness (80)
            };
            Button login = new Button
            {
                Text = "Login",
                Margin = new Thickness (0, 0, 0, 80)
            };
            login.Clicked += LoginViewModel.loginClicked;
            
            StackLayout stackLayout = new StackLayout 
            {
                Children =
                {
                    titleLabel,
                    entries,
                    login
                }
            };
            Content = new ScrollView
            {
                Content = stackLayout
            };
        }
    }
}