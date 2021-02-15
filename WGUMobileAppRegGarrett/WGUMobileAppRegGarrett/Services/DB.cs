using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using SQLite;
using WGUMobileAppRegGarrett.Models;
using Xamarin.Forms;
using WGUMobileAppRegGarrett.ViewModels;
using System.Data;
using WGUMobileAppRegGarrett.Converters;

namespace WGUMobileAppRegGarrett.Services
{
    class DB
    {

        //Degree CRUD
        //Gets active degree by student Id
        public async static void getActiveDegree(int studentId)
        {
            SQLiteConnection con = new SQLiteConnection(dbPath);
            try
            {
                var degrees = con.Query<Degree>($"SELECT * FROM Degrees WHERE StudentId = '{studentId}' AND DegreeActive = '1'");
                if (degrees.Count == 1)
                {
                    DegreeViewModel.degree = degrees[0];
                }
                else throw new Exception("Error finding student's degree.");
            }
            catch (Exception x)
            {
                Console.WriteLine(x.Message);
                await Application.Current.MainPage.DisplayAlert("Error", x.Message, "OK");
            }
            finally
            {
                con.Close();
            }
        }

        //Term CRUD
        //Get all terms in a degree
        public async static void getDegreeTerms(int degreeId)
        {
            SQLiteConnection con = new SQLiteConnection(dbPath);
            try
            {
                var terms = con.Query<Term>($"SELECT * FROM Terms WHERE DegreeId = '{degreeId}'");
                if (terms.Count != 0)
                {
                    terms.ForEach(t =>
                    {
                        DegreeViewModel.terms.Add(t);
                    });
                }
                else throw new Exception("Error finding student's terms.");
            }
            catch (Exception x)
            {
                Console.WriteLine(x.Message);
                await Application.Current.MainPage.DisplayAlert("Error", x.Message, "OK");
            }
            finally
            {
                con.Close();
            }
        }

        //

        //Course CRUD
        //Get all course names and add to CourseNameConverter
        public async static void getCourseNames()
        {
            SQLiteConnection con = new SQLiteConnection(dbPath);
            try
            {
                DataTable coursesTable = new DataTable();
                coursesTable.Columns.Add();
                coursesTable.Columns.Add();
                var courses = con.Query<Course>($"SELECT CourseId, CourseName FROM Courses");
                if (courses.Count != 0)
                {
                    courses.ForEach(c =>
                    {
                        DataRow dataRow = coursesTable.NewRow();
                        dataRow[0] = c.CourseId;
                        dataRow[1] = c.CourseName;
                        coursesTable.Rows.Add(dataRow);
                        //CourseNameConverter.courseNames.Rows.Add(c);
                    });
                    CourseNameConverter.courseNames = coursesTable;
                }
                else throw new Exception("Error retrieving course names.");
            }
            catch (Exception x)
            {
                Console.WriteLine(x.Message);
                await Application.Current.MainPage.DisplayAlert("Error", x.Message, "OK");
            }
            finally
            {
                con.Close();
            }
        }

        //Enrollment CRUD
        //Get all enrollments in a term
        public async static void getTermEnrollments(int termId)
        {
            SQLiteConnection con = new SQLiteConnection(dbPath);
            try
            {
                var enrollments = con.Query<Enrollment>($"SELECT * FROM Enrollments WHERE TermId = '{termId}'");
                if (enrollments.Count != 0)
                {
                    enrollments.ForEach(e =>
                    {
                        TermViewModel.enrollments.Add(e);
                    });
                }
                else throw new Exception("Error finding student's enrollments.");
            }
            catch (Exception x)
            {
                Console.WriteLine(x.Message);
                await Application.Current.MainPage.DisplayAlert("Error", x.Message, "OK");
            }
            finally
            {
                con.Close();
            }
        }

        //Assessment CRUD

        //Instructor CRUD

        //Student CRUD

        //Helpers
        //Attempts to log user in
        public async static void studentLogin(string username, string password)
        {
            SQLiteConnection con = new SQLiteConnection(dbPath);
            try
            {
                //Find user with username
                var student = con.Query<Student>($"SELECT * FROM Students WHERE username = '{username}'");
                if (student.Count == 1)
                {
                    if (student[0].Password == password)
                    {
                        Auth.user = student[0];
                        Auth.loggedIn = true;
                        Application.Current.MainPage = new AppShell();
                    }
                    else throw new Exception("Username and Password do not match.");
                }
                else throw new Exception("Error logging in.");
            }
            catch (Exception x)
            {
                Console.WriteLine(x.Message);
                await Application.Current.MainPage.DisplayAlert("Error", x.Message, "OK");
            }
            finally
            {
                con.Close();
            }
        }

        //Create dbPath variable for Android
        static string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "date.db3");

        //Create DB if none exists and populate test data
        public static void initializeDB()
        {
            SQLiteConnection con = new SQLiteConnection(dbPath);
            try
            {
                con.CreateTable<Models.Assessment>();
                con.CreateTable<Models.Course>();
                con.CreateTable<Models.Degree>();
                con.CreateTable<Models.Enrollment>();
                con.CreateTable<Models.Instructor>();
                con.CreateTable<Models.Student>();
                con.CreateTable<Models.Term>();
            }
            catch (Exception x)
            {
                Console.WriteLine("Error creating tables: " + x.Message);
            }
            try
            {
                if (con.Table<Models.Student>().Count() == 0)
                {
                    //Get start and end dates for test data

                    DateTime now = DateTime.Now;
                    DateTime start = new DateTime(now.Year, now.Month, 1);
                    DateTime end = start.AddMonths(6).AddHours(23).AddMinutes(59).AddSeconds(59);
                    DateTime enrollmentStart = start.AddDays(7);
                    DateTime enrollmentEnd = enrollmentStart.AddDays(14);
                    DateTime paAssessmentStart = enrollmentStart.AddDays(12).AddHours(15).AddMinutes(30);
                    DateTime paAssessmentEnd = paAssessmentStart.AddDays(1);
                    DateTime oaAssessmentStart = paAssessmentEnd.AddHours(2).AddMinutes(30);
                    DateTime oaAssessmentEnd = oaAssessmentStart.AddHours(2);
                    List<DateTime> dates = new List<DateTime> { start, end, enrollmentStart, enrollmentEnd, paAssessmentStart, paAssessmentEnd, oaAssessmentStart, oaAssessmentEnd };
                    List<string> sqlDates = convertDates(dates);
                    //Insert test student
                    Models.Student student = new Models.Student
                    {
                        StudentName = "test",
                        Username = "test",
                        Password = "test"
                    };
                    con.Insert(student);
                    int studentId = student.StudentId;
                    //Insert test degree
                    Models.Degree degree = new Models.Degree
                    {
                        StudentId = studentId,
                        DegreeName = "My Degree",
                        DegreeActive = 1
                    };
                    con.Insert(degree);
                    int degreeId = degree.DegreeId;
                    //Insert test term
                    Models.Term term = new Models.Term
                    {
                        DegreeId = degreeId,
                        TermName = "First Term",
                        TermStart = sqlDates[0],
                        TermEnd = sqlDates[1]
                    };
                    con.Insert(term);
                    int termId = term.TermId;
                    //Insert test instructor
                    Models.Instructor instructor = new Models.Instructor()
                    {
                        InstructorName = "Reg Garrett",
                        Email = "rgarr25@wgu.edu",
                        Phone = "801-623-8070"
                    };
                    con.Insert(instructor);
                    int instructorId = instructor.InstructorId;
                    //Insert test course
                    Models.Course course = new Models.Course()
                    {
                        InstructorId = instructorId,
                        CourseName = "Intro to Xamarin Forms"
                    };
                    con.Insert(course);
                    int courseId = course.CourseId;
                    //Insert test Assessments
                    Models.Assessment pa = new Models.Assessment()
                    {
                        CourseId = courseId,
                        Type = "Performance Assessment",
                        AssessmentStart = sqlDates[4],
                        AssessmentEnd = sqlDates[5],
                        AssessmentStartNotify = 0,
                        AssessmentEndNotify = 0
                    };
                    con.Insert(pa);
                    Models.Assessment oa = new Models.Assessment()
                    {
                        CourseId = courseId,
                        Type = "Objective Assessment",
                        AssessmentStart = sqlDates[6],
                        AssessmentEnd = sqlDates[7],
                        AssessmentStartNotify = 0,
                        AssessmentEndNotify = 0
                    };
                    con.Insert(oa);
                    Models.Enrollment enrollment = new Models.Enrollment()
                    {
                        TermId = termId,
                        CourseId = courseId,
                        Status = "Enrolled",
                        Notes = "",
                        EnrollmentStart = sqlDates[2],
                        EnrollmentEnd = sqlDates[3],
                        EnrollmentStartNotify = 0,
                        EntrollmentEndNotify = 0
                    };
                    con.Insert(enrollment);
                }
            }
            catch (Exception x)
            {
                Console.WriteLine("Error populating test data: " + x.Message);
            }
            finally
            {
                con.Close();
            }
        }

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
    }
}
