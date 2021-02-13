using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WGUMobileAppRegGarrett.Services;
using WGUMobileAppRegGarrett.ViewModels;
using WGUMobileAppRegGarrett.Templates;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WGUMobileAppRegGarrett.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DegreePage : ContentPage
    {
        public DegreeViewModel degreeVM;
        public DegreePage()
        {
            InitializeComponent();
            Auth.loginCheck(this);
            if (Auth.loggedIn)
            {
                linkViewModel();
                populatePage();
            }
        }
        private void linkViewModel()
        {
            degreeVM = new DegreeViewModel();
        }
        //Populate page
        private void populatePage()
        {
            this.BindingContext = degreeVM;
            ListView listview = new ListView()
                {
                    RowHeight = 120, 
                    Margin = new Thickness(5)
                };
            listview.ItemTemplate = new DataTemplate(typeof(TermCell));
            listview.ItemsSource = DegreeViewModel.terms;

            ScrollView scrollview = new ScrollView()
                {
                    Content = listview
                };
            this.Content = scrollview;
        }
    }
}