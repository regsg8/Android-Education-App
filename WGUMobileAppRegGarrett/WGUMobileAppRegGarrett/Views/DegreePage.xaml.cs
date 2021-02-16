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
using WGUMobileAppRegGarrett.Models;

namespace WGUMobileAppRegGarrett.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DegreePage : ContentPage
    {
        public DegreeViewModel degreeVM;
        private bool editing;
        public DegreePage()
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
            degreeVM = new DegreeViewModel();
            this.BindingContext = degreeVM;
        }

        //Populate page
        private void populatePage()
        {
            if (editing)
            {
                editingPage();
            }
            else standardPage();
        }

        //Creates standard page view
        private void standardPage()
        {
            //Create page title
            Label name = new Label()
            {
                Style = (Style)Application.Current.Resources["title"]
            };
            name.BindingContext = DegreeViewModel.degree;
            name.SetBinding(Label.TextProperty, "DegreeName");

            //Create listview of terms
            //Need to add item tapped functionality
            //Set degreeviewmodel.selectedterm to the tapped term
            //And navigate to the term page
            ListView listview = new ListView()
            {
                RowHeight = 120,
                Margin = new Thickness(5)
            };
            listview.ItemTemplate = new DataTemplate(typeof(TermCell));
            listview.ItemsSource = DegreeViewModel.terms;

            //Create edit button
            Button edit = new Button
            {
                Text = "Edit",
                CornerRadius = 10,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Margin = new Thickness(10)
            };
            edit.Clicked += Edit_Clicked;

            //Add everything to a stacklayout
            StackLayout stack = new StackLayout()
            {
                Children =
                {
                    name,
                    listview,
                    edit
                }
            };
            //Set stacklayout in a scrollview
            ScrollView scrollview = new ScrollView()
            {
                Content = stack
            };
            this.Content = scrollview;
        }

        //Creates editing page
        private void editingPage()
        {
            //Use entries and two way bindings
            //Create page title
            Entry name = new Entry()
            {
                Style = (Style)Application.Current.Resources["title"]
            };
            name.BindingContext = DegreeViewModel.degree;
            name.SetBinding(Entry.TextProperty, "DegreeName");

            //Create listview of terms
            //Need to add item tapped functionality
            //Set degreeviewmodel.selectedterm to the tapped term
            //And navigate to the term page
            ListView listview = new ListView()
            {
                RowHeight = 120,
                Margin = new Thickness(5)
            };
            listview.ItemTemplate = new DataTemplate(typeof(TermCell));
            listview.ItemsSource = DegreeViewModel.terms;

            //Create add button
            Button add = new Button
            {
                Text = "Add Term",
                CornerRadius = 10,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.StartAndExpand,
                Margin = new Thickness(10)
            };
            add.Clicked += Add_Clicked;

            //Create save button
            Button save = new Button
            {
                Text = "Save",
                CornerRadius = 10,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(10)
            };
            save.Clicked += Save_Clicked;
            //Create cancel button
            Button cancel = new Button
            {
                Text = "Cancel",
                CornerRadius = 10,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(10)
            };
            cancel.Clicked += Cancel_Clicked;
            //Put cancel and save side by side
            StackLayout buttons = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.End,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Children =
                {
                    cancel,
                    save
                }
            };

            //Add everything to a stacklayout
            StackLayout stack = new StackLayout()
            {
                Children =
                {
                    name,
                    listview,
                    add,
                    buttons
                }
            };
            //Set stacklayout in a scrollview
            ScrollView scrollview = new ScrollView()
            {
                Content = stack
            };
            this.Content = scrollview;
        }

        private void Edit_Clicked(object sender, EventArgs e)
        {
            editing = true;
            populatePage();
        }

        private async void Save_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("ah", "saved", "shweet");
            editing = false;
            populatePage();
        }

        private void Cancel_Clicked(object sender, EventArgs e)
        {
            editing = false;
            //Undo any changes made by two-way bindings
            DB.getDegreeTerms(DegreeViewModel.degree.DegreeId);
            DB.getActiveDegree(Auth.user.StudentId);
            populatePage();
        }

        private void Add_Clicked(object sender, EventArgs e)
        {
            //Populate page with adding a new term
            DegreeViewModel.newTerm = new Term
            {
                TermName = "",
                TermStart = DateTime.Now.ToString(),
                TermEnd = DateTime.Now.AddMonths(6).ToString()
            };
            Grid grid = new Grid 
            {
                RowDefinitions =
                {
                    new RowDefinition(),
                    new RowDefinition(),
                    new RowDefinition()
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition(),
                    new ColumnDefinition()
                },
                Margin = new Thickness(0, 40, 0, 0)
            };
            Label name = new Label
            {
                Text = "Term Name:",
                Style = (Style)Application.Current.Resources["rightLabel"]
            };
            Label start = new Label
            {
                Text = "Start:",
                Style = (Style)Application.Current.Resources["rightLabel"]
            };
            Label end = new Label
            {
                Text = "End:",
                Style = (Style)Application.Current.Resources["rightLabel"]
            };
            Entry nameEntry = new Entry();
            nameEntry.SetBinding(Entry.TextProperty, "TermName", BindingMode.TwoWay);
            DatePicker startDate = new DatePicker();
            DatePicker endDate = new DatePicker();
            grid.Children.Add(name, 0, 0);
            grid.Children.Add(nameEntry, 1, 0);
            grid.Children.Add(start, 0, 1);
            grid.Children.Add(startDate, 1, 1);
            grid.Children.Add(end, 0, 2);
            grid.Children.Add(endDate, 1, 2);

            //Create save button
            Button addTerm = new Button
            {
                Text = "Add",
                CornerRadius = 10,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(10)
            };
            addTerm.Clicked += AddTerm_Clicked;
            //Create cancel button
            Button cancelTerm = new Button
            {
                Text = "Cancel",
                CornerRadius = 10,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(10)
            };
            cancelTerm.Clicked += CancelTerm_Clicked;
            //Put cancel and save side by side
            StackLayout buttons = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.End,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Children =
                {
                    cancelTerm,
                    addTerm
                }
            };
            StackLayout stack = new StackLayout
            {
                Children =
                {
                    grid,
                    buttons
                }
            };
            ScrollView scrollview = new ScrollView
            {
                Content = stack
            };

            this.Content = scrollview;
        }

        private void CancelTerm_Clicked(object sender, EventArgs e)
        {
            populatePage();
        }

        private async void AddTerm_Clicked(object sender, EventArgs e)
        {
            //add term to viewmodel terms
            await DisplayAlert("hey friendo", "addterm clicked", "mmmkay");
        }
    }
}