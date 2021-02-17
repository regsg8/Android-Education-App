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
            DB.getDegreeTerms(DegreeViewModel.degree.DegreeId);
            DB.getActiveDegree(Auth.user.StudentId);
            if (editing)
            {
                editingPage();
            }
            else standardPage();
        }

        // ↓↓↓  Standard Page  ↓↓↓ 
        private void standardPage()
        {
            Label name = new Label()
            {
                Style = (Style)Application.Current.Resources["title"]
            };
            name.BindingContext = DegreeViewModel.degree;
            name.SetBinding(Label.TextProperty, "DegreeName");
            ListView listview = new ListView()
            {
                RowHeight = 120,
                Margin = new Thickness(5)
            };
            listview.ItemTemplate = new DataTemplate(typeof(TermCell));
            listview.ItemsSource = DegreeViewModel.terms;

            Button edit = new Button
            {
                Text = "Edit",
                CornerRadius = 10,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Margin = new Thickness(10)
            };
            edit.Clicked += Edit_Clicked;
            StackLayout stack = new StackLayout()
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
                Content = stack
            };
            this.Content = scrollview;
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
            Entry name = new Entry()
            {
                Style = (Style)Application.Current.Resources["title"]
            };
            name.BindingContext = DegreeViewModel.degree;
            name.SetBinding(Entry.TextProperty, "DegreeName");
            ListView listview = new ListView()
            {
                RowHeight = 160,
                Margin = new Thickness(5) 
            };
            listview.ItemTapped += Listview_ItemTapped;
            listview.ItemTemplate = new DataTemplate(typeof(TermCellEdit));
            listview.ItemsSource = DegreeViewModel.terms;
            Button removeTerm = new Button
            {
                Text = "Remove Term",
                CornerRadius = 10,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.StartAndExpand,
                Margin = new Thickness(10)
            };
            removeTerm.Clicked += removeTerm_Clicked;
            Button add = new Button
            {
                Text = "Add Term",
                CornerRadius = 10,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.StartAndExpand,
                Margin = new Thickness(10)
            };
            add.Clicked += Add_Clicked;
            Button save = new Button
            {
                Text = "Save",
                CornerRadius = 10,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(10)
            };
            save.Clicked += Save_Clicked;
            Button cancel = new Button
            {
                Text = "Cancel",
                CornerRadius = 10,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(10)
            };
            cancel.Clicked += Cancel_Clicked;
            StackLayout buttons = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.End,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Children =
                {

                    removeTerm,
                    add,
                    cancel,
                    save
                }
            };
            StackLayout stack = new StackLayout()
            {
                Children =
                {
                    name,
                    listview,
                    buttons
                }
            };
            ScrollView scrollview = new ScrollView()
            {
                Content = stack
            };
            this.Content = scrollview;
        }
        private async void removeTerm_Clicked(object sender, EventArgs e)
        {
            if (degreeVM.selectedTerm.TermId < 0)
            {
                await DisplayAlert("Error", "Please select a term to remove.", "Okay");
            }
            else
            {
                var action = await DisplayAlert("Confirm Deletion", $"Are you sure you would like to delete {degreeVM.selectedTerm.TermName}?", "Yes", "No");
                if (action)
                {
                    DB.deleteTerm(degreeVM.selectedTerm.TermId);
                    populatePage();
                }
            }
        }
        private void Save_Clicked(object sender, EventArgs e)
        {
            //Loop through degreeviewmodel.terms and send an update call for each one
            editing = false;
            populatePage();
        }
        private void Cancel_Clicked(object sender, EventArgs e)
        {
            editing = false;
            populatePage();
        }
        // ↑↑↑  Edit Page  ↑↑↑


        // ↓↓↓  Add Term Page  ↓↓↓
        private void Add_Clicked(object sender, EventArgs e)
        {
            DegreeViewModel.newTerm = new Term();
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
            nameEntry.BindingContext = DegreeViewModel.newTerm;
            DateTime now = DateTime.Now;
            DatePicker startDate = new DatePicker();
            startDate.SetBinding(DatePicker.DateProperty, "start", BindingMode.TwoWay);
            startDate.BindingContext = degreeVM;
            degreeVM.start = new DateTime(now.Year, now.Month, 1);
            DatePicker endDate = new DatePicker();
            endDate.SetBinding(DatePicker.DateProperty, "end", BindingMode.TwoWay);
            endDate.BindingContext = degreeVM;
            degreeVM.end = new DateTime(now.Year, (now.Month + 5), DateTime.DaysInMonth(now.Year, (now.Month + 5)));
            grid.Children.Add(name, 0, 0);
            grid.Children.Add(nameEntry, 1, 0);
            grid.Children.Add(start, 0, 1);
            grid.Children.Add(startDate, 1, 1);
            grid.Children.Add(end, 0, 2);
            grid.Children.Add(endDate, 1, 2);
            Button addTerm = new Button
            {
                Text = "Add",
                CornerRadius = 10,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(10)
            };
            addTerm.Clicked += AddTerm_Clicked;
            Button cancelTerm = new Button
            {
                Text = "Cancel",
                CornerRadius = 10,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(10)
            };
            cancelTerm.Clicked += CancelTerm_Clicked;
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
            if (DegreeViewModel.checkOverlapping(degreeVM))
            {
                await DisplayAlert("Overlapping Terms", "Start and End dates cannot overlap existing terms.", "OK");
            }
            else
            {
                degreeVM.addNewTerm();
                populatePage();
            }
        }
        // ↑↑↑  Add Term Page  ↑↑↑

        private void Listview_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            degreeVM.selectedTerm = (Term)e.Item;
        }
    }
}