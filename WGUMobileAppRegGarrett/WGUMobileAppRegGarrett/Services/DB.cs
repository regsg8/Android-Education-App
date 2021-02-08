using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using SQLite;

namespace WGUMobileAppRegGarrett.Services
{
    class DB
    {


        //Login

        //Degree CRUD

        //Term CRUD

        //Course CRUD

        //Enrollment CRUD

        //Assessment CRUD

        //Instructor CRUD

        //Student CRUD

        //Helpers
        //Create dbPath variable for Android
        static string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "wgu.db3");
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
                    DateTime courseStart = start.AddDays(7);
                    DateTime courseEnd = courseStart.AddDays(14);
                    DateTime paAssessmentStart = courseStart.AddDays(12).AddHours(15).AddMinutes(30);
                    DateTime paAssessmentEnd = paAssessmentStart.AddDays(1);
                    DateTime oaAssessmentStart = paAssessmentEnd.AddHours(2).AddMinutes(30);
                    DateTime oaAssessmentEnd = oaAssessmentStart.AddHours(2);
                    List<DateTime> dates = new List<DateTime> { start, end, courseStart, courseEnd, paAssessmentStart, paAssessmentEnd, oaAssessmentStart, oaAssessmentEnd };
                    List<string> sqlDates = convertDates(dates);
                    //Insert test student
                    Models.Student student = new Models.Student
                    {
                        Name = "test",
                        Username = "test",
                        Password = "test"
                    };
                    con.Insert(student);
                    int studentId = student.StudentId;
                    //Insert test degree
                    Models.Degree degree = new Models.Degree
                    {
                        StudentId = studentId,
                        Name = "My Degree",
                        Active = 1
                    };
                    con.Insert(degree);
                    int degreeId = degree.DegreeId;
                    //Insert test term
                    Models.Term term = new Models.Term
                    {
                        DegreeId = degreeId,
                        Name = "First Term",
                        Start = sqlDates[0],
                        End = sqlDates[1]
                    };
                    con.Insert(term);
                    int termId = term.TermId;
                    //Insert test instructor
                    Models.Instructor instructor = new Models.Instructor()
                    {
                        Name = "Reg Garrett",
                        Email = "rgarr25@wgu.edu",
                        Phone = "801-623-8070"
                    };
                    con.Insert(instructor);
                    int instructorId = instructor.InstructorId;
                    //Insert test course
                    Models.Course course = new Models.Course()
                    {
                        InstructorId = instructorId,
                        Name = "Intro to Xamarin Forms",
                        Status = "Enrolled",
                        Notes = "",
                        Start = sqlDates[2],
                        End = sqlDates[3],
                        StartNotification = 0,
                        EndNotification = 0
                    };
                    con.Insert(course);
                    int courseId = course.CourseId;
                    //Insert test Assessments
                    Models.Assessment pa = new Models.Assessment()
                    {
                        CourseId = courseId,
                        Type = "Performance Assessment",
                        Start = sqlDates[4],
                        End = sqlDates[5],
                        StartNotification = 0,
                        EndNotification = 0
                    };
                    con.Insert(pa);
                    Models.Assessment oa = new Models.Assessment()
                    {
                        CourseId = courseId,
                        Type = "Objective Assessment",
                        Start = sqlDates[6],
                        End = sqlDates[7],
                        StartNotification = 0,
                        EndNotification = 0
                    };
                    con.Insert(oa);
                    Models.Enrollment enrollment = new Models.Enrollment()
                    {
                        TermId = termId,
                        CourseId = courseId
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
        private static List<string> convertDates(List<DateTime> dates)
        {
            List<string> converted = new List<string>();
            dates.ForEach(d =>
           {
               string s = d.ToString("YYYY-MM-DD HH:MM:SS.SSS");
               converted.Add(s);
           });
            return converted;
        }
    }
}
