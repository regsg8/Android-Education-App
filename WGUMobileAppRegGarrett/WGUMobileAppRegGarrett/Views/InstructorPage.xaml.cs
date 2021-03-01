using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WGUMobileAppRegGarrett.Converters;
using WGUMobileAppRegGarrett.Models;
using WGUMobileAppRegGarrett.Services;
using WGUMobileAppRegGarrett.Templates;
using WGUMobileAppRegGarrett.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WGUMobileAppRegGarrett.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InstructorPage : ContentPage
    {
        public InstructorViewModel iVM;
        public Picker currentPicker;
        public ListView currentListView;
        public InstructorPage(Picker picker)
        {
            InitializeComponent();
            Authentication.loginCheck(this);
            currentPicker = picker;
            currentListView = new ListView()
            {
                StyleId = "null"
            };
            if (Authentication.loggedIn)
            {
                populatePage();
            }
        }

        public InstructorPage(ListView listview)
        {
            InitializeComponent();
            Authentication.loginCheck(this);
            currentListView = listview;
            currentPicker = new Picker()
            {
                Title = "null"
            };
            if (Authentication.loggedIn)
            {
                populatePage();
            }
        }

        private void linkViewModel()
        {
            iVM = new InstructorViewModel();
            this.BindingContext = iVM;
        }

        private void populatePage()
        {
            linkViewModel();
            standardPage();
        }

        // ↓↓↓  Standard Page  ↓↓↓
        private void standardPage()
        {
            Label name = new Label()
            {
                Style = (Style)Application.Current.Resources["title"],
                Text = "Instructors"
            };
            ListView listview = new ListView()
            {
                RowHeight = 200,
                Margin = new Thickness(5)
            };
            listview.ItemTemplate = new DataTemplate(typeof(InstructorCellEdit));
            InstructorNameConverter.populateInstructors();
            InstructorViewModel.IVMInstructors = InstructorNameConverter.instructors;
            listview.ItemsSource = InstructorViewModel.IVMInstructors;

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
            Button add = Generics.button("center", "Add Instructor");
            add.Clicked += Add_Clicked;
            Grid btnGrid = Generics.twoByOneGrid();
            btnGrid.Children.Add(cancel, 0, 0);
            btnGrid.Children.Add(save, 1, 0);

            StackLayout stack = new StackLayout()
            {
                Children =
                {
                    name,
                    listview,
                    add,
                    btnGrid
                }
            };
            ScrollView scrollview = new ScrollView()
            {
                Content = stack
            };
            this.Content = scrollview;
        }

        private void Add_Clicked(object sender, EventArgs e)
        {
            addPage();
        }

        private void Cancel_Clicked(object sender, EventArgs e)
        {
            InstructorNameConverter.populateInstructors();
            Application.Current.MainPage.Navigation.PopAsync();
        }

        private async void Save_Clicked(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < InstructorViewModel.IVMInstructors.Count; i++)
                {
                    if (InstructorViewModel.IVMInstructors[i].Email == "" || !Validation.validateEmail(InstructorViewModel.IVMInstructors[i].Email))
                    {
                        //await DisplayAlert("Invalid Email", "Please enter valid email addresses.", "OK");
                        throw new Exception("Please enter valid email addresses");
                    }
                    else if (InstructorViewModel.IVMInstructors[i].Phone == "" || !Validation.validatePhone(InstructorViewModel.IVMInstructors[i].Phone))
                    {
                        //await DisplayAlert("Invalid Phone", "Please enter 10 digit phone numbers with no dashes.", "OK");
                        throw new Exception("Please enter 10 digit phone numbers with no dashes.");
                    }
                }
                for (int i = 0; i < InstructorViewModel.IVMInstructors.Count; i++)
                {
                    DB.updateInstructor(InstructorViewModel.IVMInstructors[i]);
                }
                InstructorNameConverter.populateInstructors();
                if (currentPicker.Title != "null")
                {
                    currentPicker.ItemsSource = InstructorNameConverter.instructorNames;
                }
                if (currentListView.StyleId != "null")
                {
                    CourseNameConverter.populateCourseNames();
                    currentListView.ItemsSource = CourseNameConverter.courses;
                }
                await Application.Current.MainPage.Navigation.PopAsync();
            }
            catch (Exception x)
            {
                await DisplayAlert("Error", x.Message, "OK");
            }
        }
        // ↑↑↑  Standard Page  ↑↑↑


        // ↓↓↓  Add Instructor Page  ↓↓↓
        private void addPage()
        {
            InstructorViewModel.newInstructor = new Instructor()
            {
                InstructorName = "Enter name",
                Email = "Enter email",
                Phone = "Enter phone"
            };
            this.BindingContext = InstructorViewModel.newInstructor;
            Label iName = Generics.label("right", "Name: ");
            Label iPhone = Generics.label("right", "Phone: ");
            Label iEmail = Generics.label("right", "Email: ");
            Entry name = new Entry()
            {
                Style = (Style)Application.Current.Resources["title"],
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            name.SetBinding(Entry.TextProperty, "InstructorName", BindingMode.TwoWay);
            Entry phone = new Entry()
            {
                Style = (Style)Application.Current.Resources["title"],
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            phone.SetBinding(Entry.TextProperty, "Phone", BindingMode.TwoWay);
            Entry email = new Entry()
            {
                Style = (Style)Application.Current.Resources["title"],
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            email.SetBinding(Entry.TextProperty, "Email", BindingMode.TwoWay);
            Grid instructorGrid = Generics.twoByThreeGrid();
            instructorGrid.Children.Add(iName, 0, 0);
            instructorGrid.Children.Add(name, 1, 0);
            instructorGrid.Children.Add(iPhone, 0, 1);
            instructorGrid.Children.Add(phone, 1, 1);
            instructorGrid.Children.Add(iEmail, 0, 2);
            instructorGrid.Children.Add(email, 1, 2);
            //Buttons
            Button addInstructor = new Button
            {
                Text = "Save",
                Style = (Style)Application.Current.Resources["leftButton"]
            };
            addInstructor.Clicked += AddInstructor_Clicked;
            Button cancelInstructor = new Button
            {
                Text = "Cancel",
                Style = (Style)Application.Current.Resources["rightButton"]
            };
            cancelInstructor.Clicked += CancelInstructor_Clicked;

            Grid btnGrid = Generics.twoByOneGrid();
            btnGrid.Children.Add(cancelInstructor, 0, 0);
            btnGrid.Children.Add(addInstructor, 1, 0);

            StackLayout stack = new StackLayout()
            {
                Padding = new Thickness(5),
                Children =
                {
                    name,
                    instructorGrid,
                    btnGrid
                }
            };
            ScrollView scrollview = new ScrollView()
            {
                Content = stack
            };
            this.Content = scrollview;
        }

        private void CancelInstructor_Clicked(object sender, EventArgs e)
        {
            populatePage();
        }

        private async void AddInstructor_Clicked(object sender, EventArgs e)
        {
            if (InstructorViewModel.newInstructor.Email == "" || !Validation.validateEmail(InstructorViewModel.newInstructor.Email))
            {
                await DisplayAlert("Invalid Email", "Please enter a valid email address.", "OK");
            }
            else if (InstructorViewModel.newInstructor.Phone == "" || !Validation.validatePhone(InstructorViewModel.newInstructor.Phone))
            {
                await DisplayAlert("Invalid Phone", "Please enter a 10 digit phone number with no dashes.", "OK");
            }
            else
            {
                DB.createInstructor(InstructorViewModel.newInstructor);
                InstructorNameConverter.populateInstructors();
                if (currentPicker.Title != "null")
                {
                    currentPicker.ItemsSource = InstructorNameConverter.instructorNames;
                }
                if (currentListView.StyleId != "null")
                {
                    CourseNameConverter.populateCourseNames();
                    currentListView.ItemsSource = CourseNameConverter.courses;
                }
                populatePage();
            }
        }
        // ↑↑↑  Add Instructor Page  ↑↑↑
    }
}
