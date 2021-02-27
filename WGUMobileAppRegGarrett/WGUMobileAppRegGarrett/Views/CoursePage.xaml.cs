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
    public partial class CoursePage : ContentPage
    {
        public CourseViewModel cVM;
        private bool editing;
        public CoursePage()
        {
            InitializeComponent();
            Auth.loginCheck(this);
            if (Auth.loggedIn)
            {
                editing = false;
                populatePage();
            }
        }

        private void linkViewModel()
        {
            cVM = new CourseViewModel();
            this.BindingContext = cVM;
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
                Text = "Courses"
            };
            ListView listview = new ListView()
            {
                RowHeight = 200,
                Margin = new Thickness(5)
            };
            listview.ItemTemplate = new DataTemplate(typeof(CourseCellEdit));
            CourseNameConverter.populateCourseNames();
            listview.ItemsSource = CourseNameConverter.courses;

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
            Button editInstructors = Generics.button("right", "Edit Instructors");
            editInstructors.Clicked += EditInstructors_Clicked;
            Button add = Generics.button("left", "Add Course");
            add.Clicked += Add_Clicked;
            Grid btnGrid = Generics.twoByTwoGrid();
            btnGrid.Children.Add(cancel, 0, 0);
            btnGrid.Children.Add(save, 1, 0);
            btnGrid.Children.Add(editInstructors, 0, 1);
            btnGrid.Children.Add(add, 1, 1);

            StackLayout stack = new StackLayout()
            {
                Children =
                {
                    name,
                    listview,
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
            CourseNameConverter.populateCourseNames();
            Application.Current.MainPage.Navigation.PopAsync();
        }

        private void Save_Clicked(object sender, EventArgs e)
        {
            for (int i = 0; i < CourseNameConverter.courses.Count; i ++)
            {
                DB.updateCourse(CourseNameConverter.courses[i]);
            }
            CourseNameConverter.populateCourseNames();
            TermViewModel.TVMCourses.Clear();
            TermViewModel.TVMCourses = CourseNameConverter.courses;
            Application.Current.MainPage.Navigation.PopAsync();
        }

        private async void EditInstructors_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new InstructorPage());
        }


        // ↑↑↑  Standard Page  ↑↑↑


        // ↓↓↓  Add Course Page  ↓↓↓
        private void addPage()
        {
            CourseViewModel.newCourse = new Course()
            {
                CourseName = "New Course",
                CourseId = -1,
                InstructorId = -1
            };
            this.BindingContext = CourseViewModel.newCourse;
            //Name
            Entry name = new Entry()
            {
                Style = (Style)Application.Current.Resources["title"],
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            name.SetBinding(Entry.TextProperty, "CourseName", BindingMode.TwoWay);
            //Instructor
            InstructorNameConverter.populateInstructors();;
            Picker instructor = new Picker()
            {
                Title = "Instructor",
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            instructor.ItemsSource = InstructorNameConverter.instructorNames;
            instructor.SetBinding(Picker.SelectedItemProperty, "InstructorId", BindingMode.TwoWay, new InstructorNameConverter());
            
            //Buttons
            Button addCourse = new Button
            {
                Text = "Save",
                Style = (Style)Application.Current.Resources["leftButton"]
            };
            addCourse.Clicked += AddCourse_Clicked; 
            Button cancelCourse = new Button
            {
                Text = "Cancel",
                Style = (Style)Application.Current.Resources["rightButton"]
            };
            cancelCourse.Clicked += CancelCourse_Clicked;
            
            Grid btnGrid = Generics.twoByOneGrid();
            btnGrid.Children.Add(cancelCourse, 0, 0);
            btnGrid.Children.Add(addCourse, 1, 0);

            StackLayout stack = new StackLayout()
            {
                Padding = new Thickness(5),
                Children =
                {
                    name,
                    instructor,
                    btnGrid
                }
            };
            ScrollView scrollview = new ScrollView()
            {
                Content = stack
            };
            this.Content = scrollview;
        }

        private void CancelCourse_Clicked(object sender, EventArgs e)
        {
            populatePage();
        }

        private async void AddCourse_Clicked(object sender, EventArgs e)
        {
            if (CourseViewModel.newCourse.InstructorId == -1)
            {
                await DisplayAlert("No Instructor Selected", "Please select an instructor.", "OK");
            }
            else
            {
                DB.createCourse(CourseViewModel.newCourse);
                CourseNameConverter.populateCourseNames();
                TermViewModel.TVMCourses.Clear();
                TermViewModel.TVMCourses = CourseNameConverter.courses;
                populatePage();
            } 
        }
        // ↑↑↑  Add Course Page  ↑↑↑
    }
}