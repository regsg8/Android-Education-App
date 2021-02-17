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
        private bool editing;
        public TermPage()
        {
            InitializeComponent();
            Auth.loginCheck(this);
            if (Auth.loggedIn)
            {
                editing = false;
                linkViewModel();
                populatePage();
            }
        }
        private void linkViewModel()
        {
            termVM = new TermViewModel();
            this.BindingContext = termVM;
        }

        private void populatePage()
        {
            if (TermViewModel.currentTerm.TermId != -1) DB.getTermEnrollments(TermViewModel.currentTerm.TermId);
            if (editing)
            {
                editingPage();
            }
            else standardPage();
        }

        // ↓↓↓  Standard Page  ↓↓↓
        private void standardPage()
        {
            //Need to add term name, start and end dates at top of page
            Label name = new Label()
            {
                Style = (Style)Application.Current.Resources["title"]
            };
            name.BindingContext = TermViewModel.currentTerm;
            name.SetBinding(Label.TextProperty, "TermName");
            Button edit = new Button
            {
                Text = "Edit Term",
                Style = (Style)Application.Current.Resources["centerButton"]
            };
            edit.Clicked += Edit_Clicked;
            //No available term
            if (TermViewModel.currentTerm.TermId == -1)
            {
                StackLayout noTerm = new StackLayout()
                {
                    Children =
                    {
                        name
                    }
                };
                ScrollView scrollview = new ScrollView()
                {
                    Content = noTerm
                };
                this.Content = scrollview;
            }
            //No available enrollments
            else if (TermViewModel.enrollments.Count == 0)
            {
                Label noEnrollments = new Label()
                {
                    Style = (Style)Application.Current.Resources["centerLabel"],
                    Text = "Term currently has no added classes."
                };
                StackLayout noList = new StackLayout()
                {
                    Children =
                    {
                        name,
                        noEnrollments,
                        edit
                    }
                };
                ScrollView scrollview = new ScrollView()
                {
                    Content = noList
                };
                this.Content = scrollview;
            }
            else
            {
                ListView listview = new ListView()
                {
                    RowHeight = 120,
                    Margin = new Thickness(5)
                };
                listview.ItemTemplate = new DataTemplate(typeof(EnrollmentCell));
                listview.ItemsSource = TermViewModel.enrollments;

                StackLayout withList = new StackLayout()
                {
                    Children =
                    {
                        name,
                        listview,
                        edit
                    }
                };
                ScrollView scrollview = new ScrollView()
                {
                    Content = withList
                };
                this.Content = scrollview;
            }
        }
        private void Edit_Clicked(object sender, EventArgs e)
        {
            editing = true;
            populatePage();
        }
        // ↑↑↑  Standard Page  ↑↑↑


        // ↓↓↓  Edit Page  ↓↓↓
        private void editingPage()
        {

        }
        // ↑↑↑  Edit Page  ↑↑↑


        // ↓↓↓  Add Enrollment Page  ↓↓↓

        // ↑↑↑  Add Enrollment Page  ↑↑↑
    }
}