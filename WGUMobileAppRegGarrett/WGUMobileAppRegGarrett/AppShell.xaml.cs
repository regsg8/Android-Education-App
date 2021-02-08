using System;
using System.Collections.Generic;
using WGUMobileAppRegGarrett.Views;
using Xamarin.Forms;

namespace WGUMobileAppRegGarrett
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public static bool loggedIn = false;
        public AppShell()
        {
            InitializeComponent();
        }

    }
}
