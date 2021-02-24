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
using WGUMobileAppRegGarrett.Views;
using System.Collections.ObjectModel;

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
        //Get all terms in a degree and add them to DegreeViewModel
        public async static void getDegreeTerms(int degreeId)
        {
            SQLiteConnection con = new SQLiteConnection(dbPath);
            try
            {
                var terms = con.Query<Term>($"SELECT * FROM Terms WHERE DegreeId = '{degreeId}'");
                if (terms.Count != 0)
                {
                    DegreeViewModel.terms.Clear();
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

        //Create a term
        public async static void createTerm(int degreeId, string termName, string start, string end)
        {
            SQLiteConnection con = new SQLiteConnection(dbPath);
            try
            {
                Models.Term term = new Models.Term
                {
                    DegreeId = degreeId,
                    TermName = termName,
                    TermStart = start,
                    TermEnd = end
                };
                con.Insert(term);
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

        //Delete a term
        public async static void deleteTerm(int termId)
        {
            SQLiteConnection con = new SQLiteConnection(dbPath);
            try
            {
                con.Delete<Term>(termId);
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

        //Update a term
        public async static void updateTerm(Term term)
        {
            SQLiteConnection con = new SQLiteConnection(dbPath);
            try
            {
                con.Execute($"UPDATE Terms SET TermStart = '{term.TermStart}', TermEnd = '{term.TermEnd}', TermName = '{term.TermName}' WHERE TermId = '{term.TermId}'");
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

        //Get term by Id
        public static Term getTerm(int termId)
        {
            SQLiteConnection con = new SQLiteConnection(dbPath);
            Term term = new Term() 
            {
                TermId = -1,
                TermName = "Term not available."
            };
            try
            {
                term = con.Get<Term>(termId);
                return term;
            }
            catch (Exception x)
            {
                Console.WriteLine(x.Message);
                Application.Current.MainPage.DisplayAlert("Error", x.Message, "OK");
                return term;
            }
            finally
            {
                con.Close();
            }
        }


        //Course CRUD
        //Get all courses for CourseNameConverter
        public async static void getCourses()
        {
            SQLiteConnection con = new SQLiteConnection(dbPath);
            try
            {
                var courses = con.Query<Course>("SELECT * FROM Courses");
                if (courses.Count != 0)
                {
                    courses.ForEach(c =>
                    {
                        CourseNameConverter.courses.Add(c);
                    });
                }
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
        //Get all enrollments in a term for TermViewModel
        public async static void getTermEnrollments(int termId)
        {
            SQLiteConnection con = new SQLiteConnection(dbPath);
            try
            {
                var enrollments = con.Query<Enrollment>($"SELECT * FROM Enrollments WHERE TermId = '{termId}'");
                if (enrollments.Count != 0)
                {
                    TermViewModel.enrollments.Clear();
                    enrollments.ForEach(e =>
                    {
                        TermViewModel.enrollments.Add(e);
                    });
                }
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

        //Get enrollment by Id
        public static Enrollment getEnrollment(int eId)
        {
            Enrollment e = new Enrollment();
            SQLiteConnection con = new SQLiteConnection(dbPath);
            try
            {
                e = con.Get<Enrollment>(eId);
                return e;
            }
            catch (Exception x)
            {
                Console.WriteLine(x.Message);
                Application.Current.MainPage.DisplayAlert("Error", x.Message, "OK");
                return e;
            }
            finally
            {
                con.Close();
            }
        }

        //Create an enrollment
        public async static void createEnrollment(int termId, int courseId, string start, string end)
        {
            SQLiteConnection con = new SQLiteConnection(dbPath);
            try
            {
                Models.Enrollment enrollment = new Models.Enrollment
                {
                    TermId = termId,
                    CourseId = courseId,
                    EnrollmentStart = start,
                    EnrollmentEnd = end,
                    Status = "Enrolled",
                    Notes = "",
                    EnrollmentStartNotify = 0,
                    EntrollmentEndNotify = 0
                };
                con.Insert(enrollment);
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

        //Update an enrollment
        public async static void updateEnrollment(Enrollment e)
        {
            SQLiteConnection con = new SQLiteConnection(dbPath);
            try
            {
                con.Execute($"UPDATE Enrollments SET EnrollmentStart = '{e.EnrollmentStart}', EnrollmentEnd = '{e.EnrollmentEnd}' WHERE EnrollmentId = '{e.EnrollmentId}'");
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

        //Delete an enrollment
        public async static void deleteEnrollment(int enrollmentId)
        {
            SQLiteConnection con = new SQLiteConnection(dbPath);
            try
            {
                con.Delete<Enrollment>(enrollmentId);
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
        //Get all assessments for AssessmentNameConverter
        public async static void getAssessments()
        {
            SQLiteConnection con = new SQLiteConnection(dbPath);
            try
            {
                var assessments = con.Query<Assessment>($"SELECT * FROM Assessments");
                if (assessments.Count != 0)
                {
                    AssessmentNameConverter.assessments.Clear();
                    assessments.ForEach(a =>
                    {
                        AssessmentNameConverter.assessments.Add(a);
                    });
                }
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

        //Get all assessments for an Enrollment
        public async static void getEnrollmentAssessments(int enrollmentId)
        {
            SQLiteConnection con = new SQLiteConnection(dbPath);
            try
            {
                var assessments = con.Query<Assessment>($"SELECT * FROM Assessments WHERE EnrollmentId = '{enrollmentId}'");
                if (assessments.Count != 0)
                {
                    EnrollmentViewModel.enrollmentAssessments.Clear();
                    assessments.ForEach(a =>
                    {
                        EnrollmentViewModel.enrollmentAssessments.Add(a);
                    });
                }
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


        //Instructor CRUD
        //Get all Instructors for InstructorNameConverter
        public async static void getInstructors()
        {
            SQLiteConnection con = new SQLiteConnection(dbPath);
            try
            {
                var instructors = con.Query<Instructor>($"SELECT * FROM Instructors");
                if (instructors.Count != 0)
                {
                    InstructorNameConverter.instructors.Clear();
                    instructors.ForEach(i =>
                    {
                        InstructorNameConverter.instructors.Add(i);
                    });
                }
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

        //Student CRUD
        //Currently not necessary, to add later

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
                        Application.Current.MainPage = new NavigationPage(new DegreePage());
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
        static string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "samwise.db3");

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
                    DateTime end = new DateTime(start.Year, (start.Month + 5), DateTime.DaysInMonth(start.Year, (start.Month + 5)));
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
                    
                    //Insert test enrollment
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
                    int enrollmentId = enrollment.EnrollmentId;

                    //Insert test Assessments
                    Models.Assessment pa = new Models.Assessment()
                    {
                        EnrollmentId = enrollmentId,
                        AssessmentName = "Xamarin Forms PA",
                        Type = "PA",
                        AssessmentStart = sqlDates[4],
                        AssessmentEnd = sqlDates[5],
                        AssessmentStartNotify = 0,
                        AssessmentEndNotify = 0
                    };
                    con.Insert(pa);
                    Models.Assessment oa = new Models.Assessment()
                    {
                        EnrollmentId = enrollmentId,
                        AssessmentName = "Xamarin Forms OA",
                        Type = "OA",
                        AssessmentStart = sqlDates[6],
                        AssessmentEnd = sqlDates[7],
                        AssessmentStartNotify = 0,
                        AssessmentEndNotify = 0
                    };
                    con.Insert(oa);
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

        public static ObservableCollection<string> courseStatusOptions = new ObservableCollection<string>()
        {
            "Enrolled",
            "In Progress",
            "Completed",
            "Incomplete",
            "Withdrawn",
            "Planned"
        };

        public static ObservableCollection<string> assessmentTypes = new ObservableCollection<string>()
        {
            "Objective Assessment",
            "Performance Assessment"
        };
    }
}
