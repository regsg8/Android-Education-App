using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using WGUMobileAppRegGarrett.Models;
using WGUMobileAppRegGarrett.Services;
using Xamarin.Forms;

namespace WGUMobileAppRegGarrett.ViewModels
{
    public class DegreeViewModel : INotifyPropertyChanged
    {
        public static Degree degree { get; set; }
        public static ObservableCollection<Term> terms;
        public static int selectedTermId;
        public Term selectedTerm
        {
            get => _selectedTerm;
            set
            {
                _selectedTerm = value;
                RaisePropertyChanged(nameof(selectedTerm));
            }
        }
        public static Term newTerm { get; set; }
        public DateTime start
        {
            get => _start;
            set
            {
                _start = value;
                RaisePropertyChanged(nameof(start));
            }
        }
        public DateTime end
        {
            get => _end;
            set
            {
                _end = value;
                RaisePropertyChanged(nameof(end));
            }
        }

        public DegreeViewModel()
        {
            terms = new ObservableCollection<Term>();
            DB.getActiveDegree(Auth.user.StudentId);
            DB.getDegreeTerms(degree.DegreeId);
            deselectTerm(this);
        }

        public void addNewTerm()
        {
            List<DateTime> dates = new List<DateTime> { this.start, this.end };
            List<string> sqlDates = DB.convertDates(dates);
            DB.createTerm(degree.DegreeId, newTerm.TermName, sqlDates[0], sqlDates[1]);
            DB.getDegreeTerms(degree.DegreeId);
            deselectTerm(this);
        }

        public static void deselectTerm(DegreeViewModel dVM) 
        {
            dVM.selectedTerm = new Term()
            {
                TermId = -1
            };
            selectedTermId = -1;
        }

        //Checks for overlapping appointments when updating an appointment
        public static bool checkOverlapping(DegreeViewModel dVM)
        {
            bool overlapping = false;
            try
            {
                DateTime termStart = Convert.ToDateTime(dVM.start);
                DateTime termEnd = Convert.ToDateTime(dVM.end);
                for (int i = 0; i < DegreeViewModel.terms.Count; i++)
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
                return overlapping;
            }
            catch (Exception x)
            {
                Application.Current.MainPage.DisplayAlert("Error", x.Message, "OK");
                return overlapping;
            }
        }

        private Term _selectedTerm;
        private DateTime _start;
        private DateTime _end;
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
