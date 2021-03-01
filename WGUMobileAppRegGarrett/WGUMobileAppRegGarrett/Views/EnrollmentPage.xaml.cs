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
    public partial class EnrollmentPage : ContentPage
    {
        public EnrollmentViewModel eVM;
        private bool editing;
        private bool pA;
        private bool oA;
        private Picker passPicker;
        public EnrollmentPage()
        {
            InitializeComponent();
            Authentication.loginCheck(this);
            pA = false;
            oA = false;
            passPicker = new Picker();
            if (Authentication.loggedIn)
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
            checkAssessmentTypes();
            if (editing) editingPage();
            else standardPage();
        }
        private void checkAssessmentTypes()
        {
            DB.getEnrollmentAssessments(EnrollmentViewModel.currentEnrollment.EnrollmentId);
            for (int i = 0; i < EnrollmentViewModel.enrollmentAssessments.Count; i++)
            {
                if (EnrollmentViewModel.enrollmentAssessments[i].Type == "Performance Assessment") pA = true;
                if (EnrollmentViewModel.enrollmentAssessments[i].Type == "Objective Assessment") oA = true;
            }
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
            Label end = Generics.label("right", "Due: ");
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
            Button edit = Generics.button("center", "Edit Class");
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
            //assessments
            StackLayout assessmentStack = new StackLayout();
            if (EnrollmentViewModel.enrollmentAssessments.Count == 0)
            {
                Label NA = Generics.label("center", "No assessments available");
                assessmentStack.Children.Add(NA);
            }
            else
            {
                Label aName = Generics.label("center", "Assessments");
                aName.Margin = new Thickness(0, 10, 0, 0);
                ListView listview = new ListView()
                {
                    RowHeight = 120,
                    Margin = new Thickness(5)
                };
                listview.ItemTemplate = new DataTemplate(typeof(AssessmentCell));
                listview.ItemsSource = EnrollmentViewModel.enrollmentAssessments;
                listview.ItemTapped += Listview_ItemTapped;
                assessmentStack.Children.Add(aName);
                assessmentStack.Children.Add(listview);
            }
            if (!pA)
            {
                Button addPA = Generics.button("center", "Add Performance Assessment");
                
                addPA.Clicked += AddPA_Clicked;
                assessmentStack.Children.Add(addPA);
            }
            if (!oA)
            {
                Button addOA = Generics.button("center", "Add Objective Assessment");
                addOA.Clicked += AddOA_Clicked;
                assessmentStack.Children.Add(addOA);
            }
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
                        assessmentStack,
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

        private void AddOA_Clicked(object sender, EventArgs e)
        {
            addAssessmentPage("Objective Assessment");
        }

        private void AddPA_Clicked(object sender, EventArgs e)
        {
            addAssessmentPage("Performance Assessment");
        }

        private async void Listview_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            eVM.SelectedAssessment = (Assessment)e.Item;
            await Navigation.PushAsync(new AssessmentPage(eVM.SelectedAssessment.AssessmentId));
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

            for (int i = 0; i < CourseNameConverter.courses.Count; i++)
            {
                if (CourseNameConverter.courses[i].CourseId == EnrollmentViewModel.currentEnrollment.CourseId)
                {
                    eVM.updateCourseName = CourseNameConverter.courses[i].CourseName;
                }
            }

            //Course Name
            Entry name = new Entry()
            {
                Style = (Style)Application.Current.Resources["title"],
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            name.SetBinding(Entry.TextProperty, new Binding("updateCourseName", BindingMode.TwoWay));
            name.BindingContext = eVM;
            //Start and End Date
            Label start = Generics.label("right", "Start: ");
            DatePicker startDate = new DatePicker();
            startDate.SetBinding(DatePicker.DateProperty, "EnrollmentStart", BindingMode.TwoWay, new DateConverter());
            Label end = Generics.label("right", "Due: ");
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
            Label instructor = Generics.label("center", "Instructor: ");
            InstructorNameConverter.populateInstructors();
            eVM.updateCourseInstructor = EnrollmentViewModel.courseInstructor.InstructorName;
            Picker instructorChange = new Picker()
            {
                Title = "Select Instructor"
            };
            instructorChange.ItemsSource = InstructorNameConverter.instructorNames;
            instructorChange.SetBinding(Picker.SelectedItemProperty, "updateCourseInstructor", BindingMode.TwoWay);
            instructorChange.BindingContext = eVM;
            passPicker = instructorChange;
            Grid instructorGrid = Generics.twoByOneGrid();
            instructorGrid.BindingContext = EnrollmentViewModel.courseInstructor;
            instructorGrid.Children.Add(instructor, 0, 0);
            instructorGrid.Children.Add(instructorChange, 1, 0);
            Button editInstructors = Generics.button("center", "Edit Instructors");
            editInstructors.Clicked += EditInstructors_Clicked;
            //Label changeInstructor = Generics.label("right", "New Instructor: ");
            //eVM.updateCourseInstructor = EnrollmentViewModel.courseInstructor.InstructorName;
            //Picker instructorChange = new Picker()
            //{
            //    Title = "Select Instructor"
            //};
            //instructorChange.ItemsSource = InstructorNameConverter.instructorNames;
            //instructorChange.SetBinding(Picker.SelectedItemProperty, "updateCourseInstructor", BindingMode.TwoWay);
            //instructorChange.BindingContext = eVM;
            //passPicker = instructorChange;
            //Grid instructorGrid = Generics.twoByTwoGrid();
            //instructorGrid.BindingContext = EnrollmentViewModel.courseInstructor;
            //instructorGrid.Children.Add(instructor, 0, 0);
            //instructorGrid.Children.Add(boundName, 1, 0);
            //instructorGrid.Children.Add(changeInstructor, 0, 1);
            //instructorGrid.Children.Add(instructorChange, 1, 1);
            //Button editInstructors = Generics.button("center", "Edit Instructors");
            //editInstructors.Clicked += EditInstructors_Clicked;

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
                        //instructor,
                        //instructorChange,
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
            //loop through instructor names to get instructor ID
            int updatedInstructorId = -1;
            for (int i = 0; i < InstructorNameConverter.instructors.Count; i++)
            {
                if (InstructorNameConverter.instructors[i].InstructorName.ToString() == eVM.updateCourseInstructor)
                {
                    updatedInstructorId = InstructorNameConverter.instructors[i].InstructorId;
                }
            }
            Course updatedCourse = new Course()
            {
                CourseId = EnrollmentViewModel.currentEnrollment.CourseId,
                CourseName = eVM.updateCourseName,
                InstructorId = updatedInstructorId
            };
            DB.updateCourse(updatedCourse);
            editing = false;
            populatePage();
        }

        private async void EditInstructors_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new InstructorPage(passPicker));
        }
        // ↑↑↑  Edit Page  ↑↑↑


        // ↓↓↓  Add Assessment Page  ↓↓↓
        private void addAssessmentPage(string type)
        {
            AssessmentViewModel.newAssessment = new Assessment()
            {
                Type = type,
                EnrollmentId = EnrollmentViewModel.currentEnrollment.EnrollmentId,
                AssessmentDueNotify = 0,
                AssessmentName = "New Assessment",
                AssessmentDue = EnrollmentViewModel.currentEnrollment.EnrollmentEnd
            };
            this.BindingContext = AssessmentViewModel.newAssessment;
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
            Button newAssessment = new Button
            {
                Text = "Add Assessment",
                Style = (Style)Application.Current.Resources["leftButton"]
            };
            newAssessment.Clicked += NewAssessment_Clicked;
            Button cancelAssessment = new Button
            {
                Text = "Cancel",
                Style = (Style)Application.Current.Resources["rightButton"]
            };
            cancelAssessment.Clicked += CancelAssessment_Clicked;
            Grid btnGrid = Generics.twoByOneGrid();
            btnGrid.Children.Add(cancelAssessment, 0, 0);
            btnGrid.Children.Add(newAssessment, 1, 0);
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

        private void CancelAssessment_Clicked(object sender, EventArgs e)
        {
            populatePage();
        }

        private void NewAssessment_Clicked(object sender, EventArgs e)
        {
            DB.createAssessment(AssessmentViewModel.newAssessment);
            populatePage();
        }
        // ↑↑↑  Add Assessment Page  ↑↑↑
    }



}
