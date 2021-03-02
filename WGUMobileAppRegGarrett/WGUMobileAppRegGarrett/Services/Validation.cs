using System;
using System.Collections.Generic;
using System.Text;
using WGUMobileAppRegGarrett.Converters;
using WGUMobileAppRegGarrett.Models;
using WGUMobileAppRegGarrett.ViewModels;
using Xamarin.Forms;

namespace WGUMobileAppRegGarrett.Services
{
    public static class Validation
    {
        //Convert DateTimes to SQLite strings
        public static List<string> convertDates(List<DateTime> dates)
        {
            List<string> converted = new List<string>();
            dates.ForEach(d =>
            {
                string s = d.ToString("yyyy-MM-dd HH:mm:ss");
                converted.Add(s);
            });
            return converted;
        }

        //Verify start date is before end date
        public static bool startBeforeEnd(DateTime start, DateTime end)
        {
            int checkDate = DateTime.Compare(start, end);
            if (checkDate >= 0) return false;
            else return true;
        }

        //Verify that existing term dates don't overlap with other terms
        public static bool checkOverlapping(int termId, DateTime termStart, DateTime termEnd)
        {
            bool overlapping = false;
            try
            {
                for (int i = 0; i < DegreeViewModel.terms.Count; i++)
                {
                    if (termId != DegreeViewModel.terms[i].TermId)
                    {
                        DateTime start = Convert.ToDateTime(DegreeViewModel.terms[i].TermStart);
                        DateTime end = Convert.ToDateTime(DegreeViewModel.terms[i].TermEnd);
                        int startTermStart = DateTime.Compare(start, termStart);
                        int startTermEnd = DateTime.Compare(start, termEnd);
                        int endTermStart = DateTime.Compare(end, termStart);
                        int endTermEnd = DateTime.Compare(end, termEnd);
                        if (startTermStart <= 0)
                        {
                            overlapping = (endTermStart <= 0) ? false : true;
                        }
                        else if (startTermStart > 0)
                        {
                            overlapping = (startTermEnd > 0) ? false : true;
                        }
                        else if (startTermStart > 0 && endTermEnd <= 0)
                        {
                            overlapping = true;
                        }
                    }
                }
                return overlapping;
            }
            catch (Exception x)
            {
                Application.Current.MainPage.DisplayAlert("Error", x.Message, "OK");
                return overlapping;
            }
        }

        //Verify inner object dates occur within outer object dates
        public static bool checkWithinDates(DateTime innerStart, DateTime innerEnd, DateTime outerStart, DateTime outerEnd)
        {
            bool startWithinOuter = false;
            bool endWithinOuter = false;
            bool inOuter = false;
            try
            {
                for (int i = 0; i < TermViewModel.enrollments.Count; i++)
                {
                    int startTermStart = DateTime.Compare(outerStart, innerStart);
                    int endTermStart = DateTime.Compare(outerEnd, innerStart);
                    int startTermEnd = DateTime.Compare(outerStart, innerEnd);
                    int endTermEnd = DateTime.Compare(outerEnd, innerEnd);
                    if (startTermStart <= 0 && endTermStart >= 0)
                    {
                        startWithinOuter = true;
                    }
                    if (startTermEnd <= 0 && endTermEnd >= 0)
                    {
                        endWithinOuter = true;
                    }
                    if (startWithinOuter && endWithinOuter)
                    {
                        inOuter = true;
                    }

                }
                return inOuter;
            }
            catch (Exception x)
            {
                Application.Current.MainPage.DisplayAlert("Error", x.Message, "OK");
                return inOuter;
            }
        }

        //Validate email format
        public static bool validateEmail(string email)
        {
            try
            {
                System.Net.Mail.MailAddress validAddress = new System.Net.Mail.MailAddress(email);
                string domain = validAddress.Host;
                int p = domain.LastIndexOf(".");
                string topDomain = domain.Substring(p + 1);
                int period = email.LastIndexOf(".");
                int atSymbol = email.LastIndexOf("@");
                if (p == -1 || topDomain.Length < 2 || atSymbol > period)
                {
                    return false;
                }
                else return true;
            }
            catch
            {
                return false;
            }
        }

        //Validate phone number
        public static bool validatePhone(string phone)
        {
            try
            {
                if (phone.Length != 10) throw new Exception();
                long.Parse(phone);
                return true;
            }
            catch (Exception X)
            {
                Console.WriteLine(X.Message);
                return false;
            }
        }

        //Validate unique name for new course
        public static bool validCourseName(string courseName)
        {
            bool uniqueName = true;
            CourseNameConverter.populateCourseNames();
            for (int i = 0; i < CourseNameConverter.courses.Count; i++)
            {
                if (courseName == "") uniqueName = false;
                if (courseName == CourseNameConverter.courses[i].CourseName) uniqueName = false;
            }
            return uniqueName;
        }

        //Validate unique name for updated course
        public static bool validCourseName(int courseId, string courseName)
        {
            bool uniqueName = true;
            CourseNameConverter.populateCourseNames();
            for (int i = 0; i < CourseNameConverter.courses.Count; i++)
            {
                if (courseName == "") uniqueName = false;
                if (courseId != CourseNameConverter.courses[i].CourseId)
                {
                    if (courseName == CourseNameConverter.courses[i].CourseName) uniqueName = false;
                }
            }
            return uniqueName;
        }

        //Validate unique name for new instructor
        public static bool validInstructorName(string instructorName)
        {
            bool uniqueName = true;
            InstructorNameConverter.populateInstructors();
            for (int i = 0; i < InstructorNameConverter.instructors.Count; i++)
            {
                if (instructorName == "") uniqueName = false;
                if (instructorName == InstructorNameConverter.instructors[i].InstructorName) uniqueName = false;
            }
            return uniqueName;
        }

        //Validate unique name for updated instructor
        public static bool validInstructorName(int instructorId, string instructorName)
        {
            bool uniqueName = true;
            InstructorNameConverter.populateInstructors();
            for (int i = 0; i < InstructorNameConverter.instructors.Count; i++)
            {
                if (instructorName == "") uniqueName = false;
                if (instructorId != InstructorNameConverter.instructors[i].InstructorId)
                {
                    if (instructorName == InstructorNameConverter.instructors[i].InstructorName) uniqueName = false;
                }
            }
            return uniqueName;
        }
    }
}
