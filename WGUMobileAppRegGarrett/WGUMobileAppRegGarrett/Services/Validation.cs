using System;
using System.Collections.Generic;
using System.Text;
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

        //Validate email format
        public static bool validateEmail(string email)
        {
            try
            {
                System.Net.Mail.MailAddress validAddress = new System.Net.Mail.MailAddress(email);
                return true;
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
    }
}
