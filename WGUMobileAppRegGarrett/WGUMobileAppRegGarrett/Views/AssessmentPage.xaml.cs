using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WGUMobileAppRegGarrett.Converters;
using WGUMobileAppRegGarrett.Services;
using WGUMobileAppRegGarrett.Templates;
using WGUMobileAppRegGarrett.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WGUMobileAppRegGarrett.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AssessmentPage : ContentPage
    {
        private bool editing;
        public AssessmentViewModel aVM;
        private int aID;
        public AssessmentPage(int assessmentId)
        {
            InitializeComponent();
            Auth.loginCheck(this);
            aID = assessmentId;
            if (Auth.loggedIn)
            {
                editing = false;
                populatePage();
            }
        }
        private void linkViewModel()
        {
            aVM = new AssessmentViewModel(aID);
            this.BindingContext = aVM;
        }

        private void populatePage()
        {
            linkViewModel();
            if (editing) editingPage();
            else standardPage();
        }

        // ↓↓↓  Standard Page  ↓↓↓
        private void standardPage()
        {
            this.BindingContext = AssessmentViewModel.currentAssessment;
            //course name
            Label name = new Label()
            {
                Style = (Style)Application.Current.Resources["title"]
            };
            Label type = Generics.label("center");
            Label endDate = Generics.label("left");
            Label end = Generics.label("right", "Due: ");
            name.SetBinding(Label.TextProperty, new Binding("AssessmentId", BindingMode.OneWay, new AssessmentNameConverter(), null, null));
            type.SetBinding(Label.TextProperty, new Binding("Type", BindingMode.OneWay));
            endDate.SetBinding(Label.TextProperty, "AssessmentDue", BindingMode.OneWay, new DateConverter());

            Grid dateGrid = Generics.twoByOneGrid();
            dateGrid.Children.Add(end, 0, 0);
            dateGrid.Children.Add(endDate, 1, 0);
            Button edit = Generics.button("center", "Edit");
            edit.Clicked += Edit_Clicked;
            Button remove = Generics.button("center", "Remove");
            remove.Clicked += Remove_Clicked;
            Grid btnGrid = Generics.twoByOneGrid();
            btnGrid.Children.Add(remove, 0, 0);
            btnGrid.Children.Add(edit, 1, 0);
            BoxView line1 = Generics.horizontalLine();
            BoxView line2 = Generics.horizontalLine();
            BoxView line3 = Generics.horizontalLine();
            StackLayout stack = new StackLayout()
            {
                Padding = new Thickness(5),
                Children =
                {
                    name,
                    line1,
                    type,
                    line2,
                    dateGrid,
                    line3,
                    btnGrid
                }
            };
            ScrollView scrollview = new ScrollView
            {
                Content = stack
            };

            this.Content = scrollview;
        }

        private async void Remove_Clicked(object sender, EventArgs e)
        {
            var action = await DisplayAlert("Confirm Deletion", "Are you sure you would like to remove this assessment from your class?  This cannot be undone.", "Yes", "No");
            if (action)
            {
                DB.deleteAssessment(AssessmentViewModel.currentAssessment.AssessmentId);
                await Navigation.PushAsync(new EnrollmentPage());
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
            this.BindingContext = AssessmentViewModel.currentAssessment;
            //course name
            Entry name = new Entry()
            {
                Style = (Style)Application.Current.Resources["title"],
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            name.SetBinding(Entry.TextProperty, new Binding("AssessmentName", BindingMode.TwoWay));
            Label due = Generics.label("right", "Due: ");
            DatePicker dueDate = new DatePicker();
            dueDate.SetBinding(DatePicker.DateProperty, "AssessmentDue", BindingMode.TwoWay, new DateConverter());
            Label dNotify = Generics.label("right", "Notification: ");
            Switch dueNotify = new Switch()
            {
                HorizontalOptions = LayoutOptions.Start
            };
            dueNotify.SetBinding(Switch.IsToggledProperty, "AssessmentDueNotify", BindingMode.TwoWay, new BooleanConverter());
            

            Grid dateGrid = Generics.twoByTwoGrid();
            dateGrid.Children.Add(due, 0, 0);
            dateGrid.Children.Add(dueDate, 1, 0);
            dateGrid.Children.Add(dNotify, 0, 1);
            dateGrid.Children.Add(dueNotify, 1, 1);
            dateGrid.Margin = new Thickness(10);

            //Buttons
            Button save = new Button
            {
                Text = "Save",
                Style = (Style)Application.Current.Resources["leftButton"]
            };
            save.Clicked += Save_Clicked;
            Button cancel = new Button
            {
                Text = "Cancel",
                Style = (Style)Application.Current.Resources["rightButton"]
            };
            cancel.Clicked += Cancel_Clicked;
            Grid btnGrid = Generics.twoByOneGrid();
            btnGrid.Children.Add(cancel, 0, 0);
            btnGrid.Children.Add(save, 1, 0);
            BoxView line1 = Generics.horizontalLine();
            BoxView line2 = Generics.horizontalLine();
            StackLayout stack = new StackLayout()
            {
                Padding = new Thickness(5),
                Children =
                    {
                        name,
                        line1,
                        dateGrid,
                        line2,
                        btnGrid
                    }
            };
            ScrollView scrollview = new ScrollView
            {
                Content = stack
            };

            this.Content = scrollview;
        }

        private void Cancel_Clicked(object sender, EventArgs e)
        {
            editing = false;
            populatePage();
        }

        private void Save_Clicked(object sender, EventArgs e)
        {
            DB.updateAssessment(AssessmentViewModel.currentAssessment);
            editing = false;
            populatePage();
        }
        // ↑↑↑  Edit Page  ↑↑↑
    }
}