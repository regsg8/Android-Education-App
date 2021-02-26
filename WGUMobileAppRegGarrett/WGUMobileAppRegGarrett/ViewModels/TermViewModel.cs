using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using WGUMobileAppRegGarrett.Converters;
using WGUMobileAppRegGarrett.Models;
using WGUMobileAppRegGarrett.Services;

namespace WGUMobileAppRegGarrett.ViewModels
{
    public class TermViewModel : INotifyPropertyChanged
    {
        public static Term currentTerm;
        public static ObservableCollection<Enrollment> enrollments;
        public static ObservableCollection<Course> TVMCourses;
        public static int selectedEnrollmentId;
        public static Enrollment NewEnrollment { get; set; }
        public DateTime NewEnrollmentStart
        {
            get => _start;
            set
            {
                _start = value;
                RaisePropertyChanged(nameof(NewEnrollmentStart));
            }
        }
        public DateTime NewEnrollmentEnd
        {
            get => _end;
            set
            {
                _end = value;
                RaisePropertyChanged(nameof(NewEnrollmentEnd));
            }
        }
        public Enrollment SelectedEnrollment
        {
            get => _selectedEnrollment;
            set
            {
                _selectedEnrollment = value;
                RaisePropertyChanged(nameof(SelectedEnrollment));
            }
        }
        public Course SelectedCourse
        {
            get => _selectedCourse;
            set
            {
                _selectedCourse = value;
                RaisePropertyChanged(nameof(SelectedCourse));
            }
        }

        public TermViewModel()
        {
            enrollments = new ObservableCollection<Enrollment>();
            currentTerm = new Term();
            checkTerms();
            deselectEnrollment(this);
        }
        private void checkTerms()
        {
            if (DegreeViewModel.terms.Count == 0)
            {
                currentTerm = new Term()
                {
                    TermName = "No terms available",
                    TermId = -1
                };
            }
            else if (DegreeViewModel.selectedTermId == -1)
            {
                currentTerm = DegreeViewModel.terms[DegreeViewModel.terms.Count - 1];
            }
            else 
            {
                currentTerm = DB.getTerm(DegreeViewModel.selectedTermId);
            }  
        }

        public void addNewEnrollment()
        {
            List<DateTime> dates = new List<DateTime> { this.NewEnrollmentStart, this.NewEnrollmentEnd };
            List<string> sqlDates = DB.convertDates(dates);
            DB.createEnrollment(currentTerm.TermId, SelectedCourse.CourseId, sqlDates[0], sqlDates[1]);
            DB.getTermEnrollments(TermViewModel.currentTerm.TermId);
            deselectEnrollment(this);
        }

        public static void deselectEnrollment(TermViewModel tVM)
        {
            tVM.SelectedEnrollment = new Enrollment()
            {
                EnrollmentId = -1
            };
            selectedEnrollmentId = -1;
        }

        //Need to add check for class dates within term

        private Enrollment _selectedEnrollment;
        private DateTime _start;
        private DateTime _end;
        private Course _selectedCourse;
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
