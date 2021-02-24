﻿using System;
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
    public partial class EnrollmentPage : ContentPage
    {
        public EnrollmentViewModel eVM;
        private bool editing;
        public EnrollmentPage()
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
            eVM = new EnrollmentViewModel();
            this.BindingContext = eVM;
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
            this.BindingContext = EnrollmentViewModel.currentEnrollment;
            //course name
            Label name = new Label()
            {
                Style = (Style)Application.Current.Resources["title"]
            };
            Label startDate = Generics.label("left");
            Label endDate = Generics.label("left");
            Label start = Generics.label("right", "Start: ");
            Label end = Generics.label("right", "End: ");
            name.SetBinding(Label.TextProperty, new Binding("CourseId", BindingMode.OneWay, new CourseNameConverter(), null, null));
            //start and end
            startDate.SetBinding(Label.TextProperty, "EnrollmentStart", BindingMode.OneWay, new DateConverter());
            endDate.SetBinding(Label.TextProperty, "EnrollmentEnd", BindingMode.OneWay, new DateConverter());
            Grid dateGrid = Generics.twoByTwoGrid();
            dateGrid.Children.Add(start, 0, 0);
            dateGrid.Children.Add(startDate, 1, 0);
            dateGrid.Children.Add(end, 0, 1);
            dateGrid.Children.Add(endDate, 1, 1);
            dateGrid.Margin = new Thickness(10);
            //status
            Label status = Generics.label("right", "Status: ");
            Label statusEnrollment = Generics.label("left");
            statusEnrollment.SetBinding(Label.TextProperty, "Status");
            Grid statusGrid = Generics.twoByOneGrid();
            statusGrid.Children.Add(status, 0, 0);
            statusGrid.Children.Add(statusEnrollment, 1, 0);
            //edit button
            Button edit = Generics.button("center", "Edit");
            edit.Clicked += Edit_Clicked;
            //instructor info
            Label instructor = Generics.label("center", "Instructor Info");
            instructor.Margin = new Thickness(0, 10, 0, 0);
            Label iName = Generics.label("right", "Name: ");
            Label iPhone = Generics.label("right", "Phone: ");
            Label iEmail = Generics.label("right", "Email: ");
            Label boundName = Generics.label("left");
            boundName.SetBinding(Label.TextProperty, "InstructorName");
            Label boundPhone = Generics.label("left");
            boundPhone.SetBinding(Label.TextProperty, "Phone");
            Label boundEmail = Generics.label("left");
            boundEmail.SetBinding(Label.TextProperty, "Email");
            Grid instructorGrid = Generics.twoByThreeGrid();
            instructorGrid.BindingContext = EnrollmentViewModel.courseInstructor;
            instructorGrid.Children.Add(iName, 0, 0);
            instructorGrid.Children.Add(boundName, 1, 0);
            instructorGrid.Children.Add(iPhone, 0, 1);
            instructorGrid.Children.Add(boundPhone, 1, 1);
            instructorGrid.Children.Add(iEmail, 0, 2);
            instructorGrid.Children.Add(boundEmail, 1, 2);
            //notes
            Label noteName = Generics.label("center", "Notes:");
            Label notes = Generics.label("center");
            notes.LineBreakMode = LineBreakMode.WordWrap;
            notes.SetBinding(Label.TextProperty, "Notes");

            BoxView line1 = Generics.horizontalLine();
            BoxView line2 = Generics.horizontalLine();
            BoxView line3 = Generics.horizontalLine();
            BoxView line4 = Generics.horizontalLine();
            BoxView line5 = Generics.horizontalLine();
            BoxView line6 = Generics.horizontalLine();
            //assessments, handle no assessments
            if (EnrollmentViewModel.enrollmentAssessments.Count == 0)
            {
                Label NA = Generics.label("center", "No available assessments");
                StackLayout noA = new StackLayout()
                {
                    Padding = new Thickness(5),
                    Children =
                    {
                        name,
                        line6,
                        dateGrid,
                        line5,
                        statusGrid,
                        line1,
                        instructor,
                        instructorGrid,
                        line2,
                        NA,
                        line3,
                        noteName,
                        notes,
                        line4,
                        edit
                    }
                };
                ScrollView noAssessments = new ScrollView
                {
                    Content = noA
                };

                this.Content = noAssessments;
            }
            else
            {
                Label aName = Generics.label("center", "Assessments");
                aName.Margin = new Thickness(0, 10, 0, 0);
                ListView listview = new ListView()
                {
                    RowHeight = 170,
                    Margin = new Thickness(5)
                };
                listview.ItemTemplate = new DataTemplate(typeof(AssessmentCell));
                listview.ItemsSource = EnrollmentViewModel.enrollmentAssessments;
                listview.ItemTapped += Listview_ItemTapped;
                StackLayout withList = new StackLayout()
                {
                    Padding = new Thickness(5),
                    Children =
                    {
                        name,
                        line6,
                        dateGrid,
                        line1,
                        statusGrid,
                        line2,
                        instructor,
                        instructorGrid,
                        line3,
                        aName,
                        listview,
                        line4,
                        noteName,
                        notes,
                        line5,
                        edit
                    }
                };
                ScrollView scrollview = new ScrollView
                {
                    Content = withList
                };

                this.Content = scrollview;
            }
        }

        private void Listview_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            //Open assessment page
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
            this.BindingContext = EnrollmentViewModel.currentEnrollment;

            //Course Name
            Label name = new Label()
            {
                Style = (Style)Application.Current.Resources["title"]
            };
            name.SetBinding(Label.TextProperty, new Binding("CourseId", BindingMode.OneWay, new CourseNameConverter(), null, null));

            //Start and End Date
            Label start = Generics.label("right", "Start: ");
            DatePicker startDate = new DatePicker();
            startDate.SetBinding(DatePicker.DateProperty, "EnrollmentStart", BindingMode.TwoWay, new DateConverter());
            Label end = Generics.label("right", "End: ");
            DatePicker endDate = new DatePicker();
            endDate.SetBinding(DatePicker.DateProperty, "EnrollmentEnd", BindingMode.TwoWay, new DateConverter());
            Label sNotify = Generics.label("right", "Notification: ");
            Switch startNotify = new Switch()
            {
                HorizontalOptions = LayoutOptions.Start
            };
            startNotify.SetBinding(Switch.IsToggledProperty, "EnrollmentStartNotify", BindingMode.TwoWay, new BooleanConverter());
            Label eNotify = Generics.label("right", "Notification: ");
            Switch endNotify = new Switch()
            {
                HorizontalOptions = LayoutOptions.Start
            };
            endNotify.SetBinding(Switch.IsToggledProperty, "EnrollmentEndNotify", BindingMode.TwoWay, new BooleanConverter());


            Grid dateGrid = Generics.twoByFourGrid();
            dateGrid.Children.Add(start, 0, 0);
            dateGrid.Children.Add(startDate, 1, 0);
            dateGrid.Children.Add(sNotify, 0, 1);
            dateGrid.Children.Add(startNotify, 1, 1);
            dateGrid.Children.Add(end, 0, 2);
            dateGrid.Children.Add(endDate, 1, 2);
            dateGrid.Children.Add(eNotify, 0, 3);
            dateGrid.Children.Add(endNotify, 1, 3);
            dateGrid.Margin = new Thickness(10);

            //Status
            Label changeStatus = Generics.label("right", "Status: ");
            Picker statusChange = new Picker()
            {
                Title = "New Status"
            };
            statusChange.ItemsSource = EnrollmentViewModel.courseStatusOptions;
            statusChange.SetBinding(Picker.SelectedItemProperty, "Status", BindingMode.TwoWay);
            Grid statusGrid = Generics.twoByOneGrid();
            statusGrid.Children.Add(changeStatus, 0, 0);
            statusGrid.Children.Add(statusChange, 1, 0);

            //Instructor
            Label instructor = Generics.label("right", "Instructor: ");
            Label boundName = Generics.label("left");
            boundName.SetBinding(Label.TextProperty, "InstructorName");
            InstructorNameConverter.populateInstructors();
            Label changeInstructor = Generics.label("right", "New Instructor: ");
            Picker instructorChange = new Picker()
            {
                Title = "New Instructor"
            };
            instructorChange.ItemDisplayBinding = new Binding("InstructorName");
            instructorChange.ItemsSource = InstructorNameConverter.instructors;
            instructorChange.SetBinding(Picker.SelectedItemProperty, "InstructorId", BindingMode.TwoWay, new InstructorNameConverter());
            Grid instructorGrid = Generics.twoByTwoGrid();
            instructorGrid.BindingContext = EnrollmentViewModel.courseInstructor;
            instructorGrid.Children.Add(instructor, 0, 0);
            instructorGrid.Children.Add(boundName, 1, 0);
            instructorGrid.Children.Add(changeInstructor, 0, 1);
            instructorGrid.Children.Add(instructorChange, 1, 1);
            Button editInstructors = Generics.button("center", "Edit Instructors");
            editInstructors.Clicked += EditInstructors_Clicked;

            //Notes
            Label noteName = Generics.label("center", "Notes:");
            Editor notes = new Editor();
            notes.SetBinding(Editor.TextProperty, "Notes", BindingMode.TwoWay);
            notes.AutoSize = EditorAutoSizeOption.TextChanges;

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

            //Layout
            BoxView line1 = Generics.horizontalLine();
            BoxView line2 = Generics.horizontalLine();
            BoxView line3 = Generics.horizontalLine();
            BoxView line4 = Generics.horizontalLine();
            BoxView line5 = Generics.horizontalLine();
            BoxView line6 = Generics.horizontalLine();
            StackLayout editPage = new StackLayout()
            {
                Padding = new Thickness(5),
                Children =
                    {
                        name,
                        line1,
                        dateGrid,
                        line2,
                        statusGrid,
                        line3,
                        instructorGrid,
                        editInstructors,
                        line4,
                        noteName,
                        notes,
                        line5,
                        btnGrid
                    }
            };
            ScrollView scroll = new ScrollView
            {
                Content = editPage
            };

            this.Content = scroll;
        }

        //Handlers
        private void Cancel_Clicked(object sender, EventArgs e)
        {
            editing = false;
            populatePage();
            
        }

        private void Save_Clicked(object sender, EventArgs e)
        {
            DB.updateEnrollment(EnrollmentViewModel.currentEnrollment);
            editing = false;
            populatePage();
        }

        private async void EditInstructors_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new InstructorPage());
        }
    }
        // ↑↑↑  Edit Page  ↑↑↑
}
