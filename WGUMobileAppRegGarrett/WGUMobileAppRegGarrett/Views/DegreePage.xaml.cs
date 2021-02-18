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
            Button edit = new Button
            {
                Text = "Edit Degree",
                Style = (Style)Application.Current.Resources["centerButton"]
            };
            edit.Clicked += Edit_Clicked;
            if (DegreeViewModel.terms.Count == 0)
            {
                Label noTerms = new Label()
                {
                    Style = (Style)Application.Current.Resources["centerLabel"],
                    Text = "Degree currently has no added terms."
                };
                StackLayout noList = new StackLayout()
                {
                    Children =
                    {
                        name,
                        noTerms,
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
                listview.ItemTemplate = new DataTemplate(typeof(TermCell));
                listview.ItemsSource = DegreeViewModel.terms;
                listview.ItemTapped += Listview_ItemTapped;

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
        private async void Listview_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            //TermViewModel.currentTerm = (Term)e.Item;
            degreeVM.SelectedTerm = (Term)e.Item;
            DegreeViewModel.selectedTermId = degreeVM.SelectedTerm.TermId;
            await Navigation.PushAsync(new TermPage());
        }
        // ↑↑↑  Standard Page  ↑↑↑ 


        // ↓↓↓  Edit Page  ↓↓↓ 
        private void editingPage()
        {
            DegreeViewModel.deselectTerm(degreeVM);
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
            listview.ItemTapped += EditListview_ItemTapped;
            listview.ItemTemplate = new DataTemplate(typeof(TermCellEdit));
            listview.ItemsSource = DegreeViewModel.terms;
            Button removeTerm = new Button
            {
                Text = "Remove Term",
                Style = (Style)Application.Current.Resources["rightButton"]
            };
            removeTerm.Clicked += removeTerm_Clicked;
            Button add = new Button
            {
                Text = "Add Term",
                Style = (Style)Application.Current.Resources["leftButton"]
            };
            add.Clicked += Add_Clicked;
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
            Grid grid = Generics.twoByTwoGrid();
            grid.Children.Add(removeTerm, 0, 0);
            grid.Children.Add(add, 1, 0);
            grid.Children.Add(cancel, 0, 1);
            grid.Children.Add(save, 1, 1);
            StackLayout stack = new StackLayout()
            {
                Children =
                {
                    name,
                    listview,
                    grid
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
            if (degreeVM.SelectedTerm.TermId < 0)
            {
                await DisplayAlert("Error", "Please select a term to remove.", "Okay");
            }
            else
            {
                var action = await DisplayAlert("Confirm Deletion", $"Are you sure you would like to delete {degreeVM.SelectedTerm.TermName}?", "Yes", "No");
                if (action)
                {
                    DB.deleteTerm(degreeVM.SelectedTerm.TermId);
                    populatePage();
                }
            }
        }
        private void Save_Clicked(object sender, EventArgs e)
        {
            for (int i = 0; i < DegreeViewModel.terms.Count; i++)
            {
                DB.updateTerm(DegreeViewModel.terms[i]);
            }
            editing = false;
            populatePage();
        }
        private void Cancel_Clicked(object sender, EventArgs e)
        {
            editing = false;
            populatePage();
        }
        private void EditListview_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            degreeVM.SelectedTerm = (Term)e.Item;
        }
        // ↑↑↑  Edit Page  ↑↑↑


        // ↓↓↓  Add Term Page  ↓↓↓
        private void Add_Clicked(object sender, EventArgs e)
        {
            DegreeViewModel.NewTerm = new Term();
            Grid grid = Generics.twoByThreeGrid();
            grid.Margin = new Thickness(0, 40, 0, 0);
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
            nameEntry.BindingContext = DegreeViewModel.NewTerm;
            DegreeViewModel.NewTerm.TermName = "";
            DateTime now = DateTime.Now;
            DatePicker startDate = new DatePicker();
            startDate.SetBinding(DatePicker.DateProperty, "start", BindingMode.TwoWay);
            startDate.BindingContext = degreeVM;
            degreeVM.Start = new DateTime(now.Year, now.Month, 1);
            DatePicker endDate = new DatePicker();
            endDate.SetBinding(DatePicker.DateProperty, "end", BindingMode.TwoWay);
            endDate.BindingContext = degreeVM;
            degreeVM.End = new DateTime(now.Year, (now.Month + 5), DateTime.DaysInMonth(now.Year, (now.Month + 5)));
            grid.Children.Add(name, 0, 0);
            grid.Children.Add(nameEntry, 1, 0);
            grid.Children.Add(start, 0, 1);
            grid.Children.Add(startDate, 1, 1);
            grid.Children.Add(end, 0, 2);
            grid.Children.Add(endDate, 1, 2);
            Button addTerm = new Button
            {
                Text = "Add",
                Style = (Style)Application.Current.Resources["centerButton"]
            };
            addTerm.Clicked += AddTerm_Clicked;
            Button cancelTerm = new Button
            {
                Text = "Cancel",
                Style = (Style)Application.Current.Resources["centerButton"]
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
            int checkDate = DateTime.Compare(degreeVM.Start, degreeVM.End);
            if (DegreeViewModel.checkOverlapping(degreeVM))
            {
                await DisplayAlert("Overlapping Terms", "Start and End dates cannot overlap existing terms.", "OK");
            }
            else if (DegreeViewModel.NewTerm.TermName == "")
            {
                await DisplayAlert("No Term Name", "Please enter a name for the new term.", "OK");
            }
            else if (checkDate >= 0)
            {
                await DisplayAlert("Incorrect Dates", "Start date must be before End date", "OK");
            }
            else
            {
                degreeVM.addNewTerm();
                populatePage();
            }
        }
        // ↑↑↑  Add Term Page  ↑↑↑
    }
}