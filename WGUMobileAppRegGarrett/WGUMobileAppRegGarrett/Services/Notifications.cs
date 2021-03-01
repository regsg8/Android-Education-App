using Plugin.LocalNotifications;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using WGUMobileAppRegGarrett.Converters;
using WGUMobileAppRegGarrett.Models;
using WGUMobileAppRegGarrett.ViewModels;

namespace WGUMobileAppRegGarrett.Services
{
    public static class Notifications
    {
        private static DateTime now;
        private static List<Enrollment> nEnrollments;
        private static List<Assessment> nAssessments;
        private static List<Term> nTerms;
        private static List<string> enrollmentStartNotifications;
        private static List<string> enrollmentEndNotifications;
        private static List<string> assessmentDueNotifications;
        private static string message;


        public static void checkNotifications()
        {
            initializeNotifications();
            checkEnrollmentDates();
            checkAssessmentDates();
            message = createNotificationMessage();
            if (message != "") sendNotification();
        }

        private static void initializeNotifications()
        {
            DegreeViewModel.checkedNotifications = true;
            now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            nTerms = new List<Term>();
            nEnrollments = new List<Enrollment>();
            nAssessments = new List<Assessment>();
            enrollmentStartNotifications = new List<string>();
            enrollmentEndNotifications = new List<string>();
            assessmentDueNotifications = new List<string>();
            collectTerms();
            collectEnrollments();
            collectAssessments();
        }

        private static void collectTerms()
        {
            if (DegreeViewModel.terms.Count > 0)
            {
                foreach (Term t in DegreeViewModel.terms)
                {
                    nTerms.Add(t);
                }
            }
        }

        private static void collectEnrollments()
        {
            if (nTerms.Count > 0)
            {
                foreach (Term t in nTerms)
                {
                    TermViewModel.enrollments = new ObservableCollection<Enrollment>();
                    TermViewModel.enrollments.Clear();
                    DB.getTermEnrollments(t.TermId);
                    if (TermViewModel.enrollments.Count > 0)
                    {
                        foreach (Enrollment e in TermViewModel.enrollments)
                        {
                            nEnrollments.Add(e);
                        }
                    }
                }
            }
        }

        private static void collectAssessments()
        {
            if (nEnrollments.Count > 0)
            {
                foreach (Enrollment e in nEnrollments)
                {
                    EnrollmentViewModel.enrollmentAssessments = new ObservableCollection<Assessment>();
                    EnrollmentViewModel.enrollmentAssessments.Clear();
                    DB.getEnrollmentAssessments(e.EnrollmentId);
                    if (EnrollmentViewModel.enrollmentAssessments.Count > 0)
                    {
                        foreach (Assessment a in EnrollmentViewModel.enrollmentAssessments)
                        {
                            nAssessments.Add(a);
                        }
                    }
                }
            }
        }

        private static void checkEnrollmentDates()
        {
            if (nEnrollments.Count > 0)
            {
                CourseNameConverter.populateCourseNames();
                foreach (Enrollment e in nEnrollments)
                {
                    if (e.EnrollmentStartNotify == 1)
                    {
                        DateTime start = DateTime.Parse(e.EnrollmentStart);
                        DateTime enrollmentStart = new DateTime(start.Year, start.Month, start.Day);
                        if (isToday(enrollmentStart))
                        {
                            string courseName = "";
                            for (int i = 0; i < CourseNameConverter.courses.Count; i++)
                            {
                                if (CourseNameConverter.courses[i].CourseId.ToString() == e.CourseId.ToString())
                                {
                                    courseName = CourseNameConverter.courses[i].CourseName.ToString();
                                }
                            }
                            enrollmentStartNotifications.Add(courseName);
                            CrossLocalNotifications.Current.Show("Your course starts today!", courseName);
                        }
                    }
                    if (e.EnrollmentEndNotify == 1)
                    {
                        DateTime end = DateTime.Parse(e.EnrollmentEnd);
                        DateTime enrollmentEnd = new DateTime(end.Year, end.Month, end.Day);
                        if (isToday(enrollmentEnd))
                        {
                            string courseName = "";
                            for (int i = 0; i < CourseNameConverter.courses.Count; i++)
                            {
                                if (CourseNameConverter.courses[i].CourseId.ToString() == e.CourseId.ToString())
                                {
                                    courseName = CourseNameConverter.courses[i].CourseName.ToString();
                                }
                            }
                            enrollmentEndNotifications.Add(courseName);
                            CrossLocalNotifications.Current.Show("Your course ends today!", courseName);
                        }
                    }
                }
            }
        }


        private static void checkAssessmentDates()
        {
            if (nAssessments.Count > 0)
            {
                foreach (Assessment a in nAssessments)
                {
                    if (a.AssessmentDueNotify == 1)
                    {
                        DateTime due = DateTime.Parse(a.AssessmentDue);
                        DateTime assessmentDue = new DateTime(due.Year, due.Month, due.Day);
                        if (isToday(assessmentDue))
                        {
                            assessmentDueNotifications.Add(a.AssessmentName);
                            CrossLocalNotifications.Current.Show("Your assessment is due today!", a.AssessmentName);
                        }
                    }
                }
            }
        }

        private static bool isToday(DateTime date)
        {
            int result = DateTime.Compare(now, date);
            if (result == 0)
            {
                return true;
            }
            else return false;
        }

        private static string createNotificationMessage()
        {
            string notificationMessage = "";
            if (enrollmentStartNotifications.Count > 0)
            {
                notificationMessage = "The following courses start today:\n";
                foreach (string s in enrollmentStartNotifications)
                {
                    notificationMessage = notificationMessage + $"{s}\n";
                }
            }
            if (enrollmentEndNotifications.Count > 0)
            {
                notificationMessage = notificationMessage + "\nThe following courses end today:\n";
                foreach(string s in enrollmentEndNotifications)
                {
                    notificationMessage = notificationMessage + $"{s}\n";
                }
            }
            if (assessmentDueNotifications.Count > 0)
            {
                notificationMessage = notificationMessage + "\nThe following assessments are due today:\n";
                foreach(string s in assessmentDueNotifications)
                {
                    notificationMessage = notificationMessage + $"{s}\n";
                }
            }
            return notificationMessage;
        }

        private static void sendNotification()
        {
            //CrossLocalNotifications.Current.Show("Notification", message);
        }
    }
}
