using System;
using WGUMobileAppRegGarrett.Services;
using WGUMobileAppRegGarrett.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WGUMobileAppRegGarrett
{
    public partial class App : Application
    {
        public static bool loggedIn = false;
        public App()
        {
            InitializeComponent();
            DB.initializeDB();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
