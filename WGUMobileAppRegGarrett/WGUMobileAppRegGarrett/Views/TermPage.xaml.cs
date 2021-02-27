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
            Label name = new Label()
            {
                Style = (Style)Application.Current.Resources["title"]
            };
            name.BindingContext = TermViewModel.currentTerm;
            name.SetBinding(Label.TextProperty, "TermName");
            Button edit = new Button
            {
                Text = "Edit Classes",
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

        private async void Listview_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            termVM.SelectedEnrollment = (Enrollment)e.Item;
            TermViewModel.selectedEnrollmentId = termVM.SelectedEnrollment.EnrollmentId;
            await Navigation.PushAsync(new EnrollmentPage());
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
            TermViewModel.deselectEnrollment(termVM);
            this.BindingContext = termVM;
            Label name = new Label()
            {
                Style = (Style)Application.Current.Resources["title"]
            };
            name.BindingContext = TermViewModel.currentTerm;
            name.SetBinding(Label.TextProperty, "TermName");
            ListView listview = new ListView()
            {
                RowHeight = 160,
                Margin = new Thickness(5)
            };
            listview.ItemTemplate = new DataTemplate(typeof(EnrollmentCellEdit));
            listview.ItemsSource = TermViewModel.enrollments;
            listview.ItemTapped += Edit_Listview_ItemTapped;
            Button removeEnrollment = new Button
            {
                Text = "Remove Class",
                Style = (Style)Application.Current.Resources["rightButton"]
            };
            removeEnrollment.Clicked += RemoveEnrollment_Clicked;
            Button addEnrollment = new Button
            {
                Text = "Add Class",
                Style = (Style)Application.Current.Resources["leftButton"]
            };
            addEnrollment.Clicked += AddEnrollment_Clicked;
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
            grid.Children.Add(removeEnrollment, 0, 0);
            grid.Children.Add(addEnrollment, 1, 0);
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
        private void Edit_Listview_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            termVM.SelectedEnrollment = (Enrollment)e.Item;
        }

        private void Cancel_Clicked(object sender, EventArgs e)
        {
            editing = false;
            populatePage();
        }

        private void Save_Clicked(object sender, EventArgs e)
        {
            for (int i = 0; i < TermViewModel.enrollments.Count; i++)
            {
                DB.updateEnrollment(TermViewModel.enrollments[i]);
            }
            editing = false;
            populatePage();
        }
        private async void RemoveEnrollment_Clicked(object sender, EventArgs e)
        {
            if (termVM.SelectedEnrollment.EnrollmentId < 0)
            {
                await DisplayAlert("Error", "Please select a class to remove.", "Okay");
            }
            else
            {
                string courseName = "";
                CourseNameConverter.populateCourseNames();
                for (int i = 0; i < CourseNameConverter.courses.Count; i++)
                {
                    if (CourseNameConverter.courses[i].CourseId.ToString() == termVM.SelectedEnrollment.CourseId.ToString())
                    {
                        courseName = CourseNameConverter.courses[i].CourseName.ToString();
                    }
                }
                var action = await DisplayAlert("Confirm Deletion", $"Are you sure you would like to remove '{courseName}' from your term?  This cannot be undone.", "Yes", "No");
                if (action)
                {
                    DB.deleteEnrollment(termVM.SelectedEnrollment.EnrollmentId);
                    populatePage();
                }
            }
        }
        // ↑↑↑  Edit Page  ↑↑↑



        // ↓↓↓  Add Enrollment Page  ↓↓↓
        private void AddEnrollment_Clicked(object sender, EventArgs e)
        {
            //
            this.BindingContext = termVM;
            TermViewModel.NewEnrollment = new Enrollment();
            Grid grid = Generics.twoByTwoGrid();
            grid.Margin = new Thickness(0, 20, 0, 0);
            Picker picker = new Picker()
            {
                Title = "Courses",
                TitleColor = (Color)Application.Current.Resources["Secondary"],
                Margin = new Thickness(10, 40, 10, 0)
            };
            CourseNameConverter.populateCourseNames();
            TermViewModel.TVMCourses = CourseNameConverter.courses;
            picker.ItemDisplayBinding = new Binding("CourseName");
            picker.SetBinding(Picker.SelectedItemProperty, "SelectedCourse", BindingMode.TwoWay);
            picker.ItemsSource = TermViewModel.TVMCourses;
            Button courseEdit = Generics.button("center", "Edit Courses");
            courseEdit.Clicked += CourseEdit_Clicked;
            courseEdit.VerticalOptions = LayoutOptions.Start;
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
            DateTime now = DateTime.Now;
            DatePicker startDate = new DatePicker();
            startDate.SetBinding(DatePicker.DateProperty, "NewEnrollmentStart", BindingMode.TwoWay);
            startDate.BindingContext = termVM;
            termVM.NewEnrollmentStart = new DateTime(now.Year, (now.Month + 1), 1);
            DatePicker endDate = new DatePicker();
            endDate.SetBinding(DatePicker.DateProperty, "NewEnrollmentEnd", BindingMode.TwoWay);
            endDate.BindingContext = termVM;
            termVM.NewEnrollmentEnd = new DateTime(now.Year, (now.Month + 1), 15);
            grid.Children.Add(start, 0, 0);
            grid.Children.Add(startDate, 1, 0);
            grid.Children.Add(end, 0, 1);
            grid.Children.Add(endDate, 1, 1);
            Button addEnrollment = new Button
            {
                Text = "Add",
                Style = (Style)Application.Current.Resources["centerButton"]
            };
            addEnrollment.Clicked += Add_Clicked;
            Button cancelEnrollment = new Button
            {
                Text = "Cancel",
                Style = (Style)Application.Current.Resources["centerButton"]
            };
            cancelEnrollment.Clicked += CancelEnrollment_Clicked;
            StackLayout buttons = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.End,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Children =
                {
                    cancelEnrollment,
                    addEnrollment
                }
            };
            StackLayout stack = new StackLayout
            {
                Children =
                {
                    picker,
                    courseEdit,
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

        private async void CourseEdit_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CoursePage());
        }

        private void CancelEnrollment_Clicked(object sender, EventArgs e)
        {
            populatePage();
        }

        private async void Add_Clicked(object sender, EventArgs e)
        {
            if (termVM.SelectedCourse == null)
            {
                await DisplayAlert("Course Not Selected", "Please select a course to add to your term.", "OK");
            }
            else if (TermViewModel.enrollments.Count >= 6)
            {
                await DisplayAlert("Cannot Add Course", "Students can only take 6 courses per term.", "OK");
            }
            else
            {
                termVM.addNewEnrollment();
                populatePage();
            }
        }
        // ↑↑↑  Add Enrollment Page  ↑↑↑
    }
}