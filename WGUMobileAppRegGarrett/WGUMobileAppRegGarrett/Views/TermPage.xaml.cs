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
    public partial class TermPage : ContentPage
    {
        public TermPage()
        {
            InitializeComponent();
            ToolbarItem logout = new ToolbarItem
            {
                Text = "Logout"
            };
            logout.Clicked += logoutClicked;
            void logoutClicked(object sender, EventArgs args)
            {
                Application.Current.MainPage = new LoginPage();
            }
            this.ToolbarItems.Add(logout);
        }
    }
}