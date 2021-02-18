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
        public static Instructor courseInstructor;
        public EnrollmentViewModel()
        {
            currentEnrollment = DB.getEnrollment(TermViewModel.selectedEnrollmentId);
            enrollmentAssessments = new ObservableCollection<Assessment>();
            DB.getEnrollmentAssessments(currentEnrollment.EnrollmentId);
            InstructorNameConverter.populateInstructors();
            courseInstructor = new Instructor();
            populateInstructor(currentEnrollment.CourseId);
            AssessmentNameConverter.populateAssessments();
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
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
