using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WGUMobileAppRegGarrett.Services;
using WGUMobileAppRegGarrett.Templates;
using WGUMobileAppRegGarrett.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WGUMobileAppRegGarrett.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TermPage : ContentPage
    {
        public TermViewModel termVM;
        public TermPage()
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
            termVM = new TermViewModel();
        }
        //Populate page
        private void populatePage()
        {
            this.BindingContext = termVM;
            ListView listview = new ListView()
            {
                RowHeight = 120,
                Margin = new Thickness(5)
            };
            listview.ItemTemplate = new DataTemplate(typeof(EnrollmentCell));
            listview.ItemsSource = TermViewModel.enrollments;

            ScrollView scrollview = new ScrollView()
            {
                Content = listview
            };
            this.Content = scrollview;
        }
    }
}