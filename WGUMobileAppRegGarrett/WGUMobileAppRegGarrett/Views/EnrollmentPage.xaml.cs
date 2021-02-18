using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WGUMobileAppRegGarrett.Converters;
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
            linkViewModel();
            editing = false;
            populatePage();
        }

        private void linkViewModel()
        {
            eVM = new EnrollmentViewModel();
            this.BindingContext = eVM;
        }

        private void populatePage()
        {
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
            //course name

            //start and end date pickers

            //start and end notification toggles

            //status picker

            //assessment edit, add button, go to assessment page

            //instructor picker, edit/add button, go to instructor page

            //course notes

            //save and cancel buttons   
        }
        // ↑↑↑  Edit Page  ↑↑↑
    }
}