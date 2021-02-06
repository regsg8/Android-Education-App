using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WGUMobileAppRegGarrett.ViewModels
{
    class LoginViewModel
    {
        public static void loginClicked(object sender, EventArgs args)
        {
            //check for blank entries

            Application.Current.MainPage = new AppShell();
        }
    }
}
