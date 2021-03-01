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
        public static bool checkedNotifications;
        public static Term NewTerm { get; set; }
        public Term SelectedTerm
        {
            get => _selectedTerm;
            set
            {
                _selectedTerm = value;
                RaisePropertyChanged(nameof(SelectedTerm));
            }
        }
        public DateTime Start
        {
            get => _start;
            set
            {
                _start = value;
                RaisePropertyChanged(nameof(Start));
            }
        }
        public DateTime End
        {
            get => _end;
            set
            {
                _end = value;
                RaisePropertyChanged(nameof(End));
            }
        }

        public DegreeViewModel()
        {
            terms = new ObservableCollection<Term>();
            DB.getActiveDegree(Authentication.user.StudentId);
            DB.getDegreeTerms(degree.DegreeId);
            deselectTerm(this);
            if (!checkedNotifications) Notifications.checkNotifications();
        }

        public void addNewTerm()
        {
            List<DateTime> dates = new List<DateTime> { this.Start, this.End };
            List<string> sqlDates = Validation.convertDates(dates);
            DB.createTerm(degree.DegreeId, NewTerm.TermName, sqlDates[0], sqlDates[1]);
            DB.getDegreeTerms(degree.DegreeId);
            deselectTerm(this);
        }

        public static void deselectTerm(DegreeViewModel dVM) 
        {
            dVM.SelectedTerm = new Term()
            {
                TermId = -1
            };
            selectedTermId = -1;
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
