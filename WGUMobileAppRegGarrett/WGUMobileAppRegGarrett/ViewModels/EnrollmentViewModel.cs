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
    public class EnrollmentViewModel : INotifyPropertyChanged
    {
        public static Enrollment currentEnrollment;
        public static ObservableCollection<Assessment> enrollmentAssessments;
        public static ObservableCollection<string> courseStatusOptions;
        public static ObservableCollection<string> assessmentTypes;
        public static Instructor courseInstructor;
        public string updateCourseName
        {
            get => _updateCourseName;
            set
            {
                _updateCourseName = value;
                RaisePropertyChanged(nameof(updateCourseName));
            }
        }
        public string updateCourseInstructor
        {
            get => _updateCourseInstructor;
            set
            {
                _updateCourseInstructor = value;
                RaisePropertyChanged(nameof(updateCourseInstructor));
            }
        }
        public Assessment SelectedAssessment
        {
            get => _selectedAssessment;
            set
            {
                _selectedAssessment = value;
                RaisePropertyChanged(nameof(SelectedAssessment));
            }
        }
        public EnrollmentViewModel()
        {
            currentEnrollment = DB.getEnrollment(TermViewModel.selectedEnrollmentId);
            enrollmentAssessments = new ObservableCollection<Assessment>();
            DB.getEnrollmentAssessments(currentEnrollment.EnrollmentId);
            InstructorNameConverter.populateInstructors();
            courseInstructor = new Instructor();
            populateInstructor(currentEnrollment.CourseId);
            AssessmentNameConverter.populateAssessments();
            courseStatusOptions = DB.courseStatusOptions;
            assessmentTypes = DB.assessmentTypes;
        }

        private void populateInstructor(int courseId)
        {
            CourseNameConverter.populateCourseNames();
            InstructorNameConverter.populateInstructors();
            for (int i = 0; i < CourseNameConverter.courses.Count; i++)
            {
                if (CourseNameConverter.courses[i].CourseId == courseId)
                {
                    for (int j = 0; j < InstructorNameConverter.instructors.Count; j++)
                    {
                        if (CourseNameConverter.courses[i].InstructorId == InstructorNameConverter.instructors[j].InstructorId)
                        {
                            courseInstructor = InstructorNameConverter.instructors[j];
                        }
                    }
                }
            }
        }

        private Assessment _selectedAssessment;
        private string _updateCourseName;
        private string _updateCourseInstructor;
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
