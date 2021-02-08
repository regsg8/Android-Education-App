using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WGUMobileAppRegGarrett.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DegreePage : ContentPage
    {
        public DegreePage()
        {
            InitializeComponent();
            checkForLogin();
            //ToolbarItem logout = new ToolbarItem
            //{
            //    Text = "Logout"
            //};
            //logout.Clicked += logoutClicked;
            //void logoutClicked(object sender, EventArgs args)
            //{
            //    Navigation.PopAsync();
            //    Navigation.PushAsync(new LoginPage());
            //}
            //this.ToolbarItems.Add(logout);
        }
        public void checkForLogin()
        {
            if (!App.loggedIn)
            {
                this.Content = new Label 
                { 
                    Text = "Please log in",
                    Style = (Style)Application.Current.Resources["centerLabel"]
                };
                ToolbarItem login = new ToolbarItem
                {
                    Text = "Login"
                    
                };
                login.Clicked += loginClicked;
                void loginClicked(object sender, EventArgs args)
                {
                    //DisplayAlert("Alert", "You have been alerted", "OK");
                    App.loggedIn = true;
                    Application.Current.MainPage = new AppShell();
                }
                this.ToolbarItems.Add(login);
            }
        }
    }
}