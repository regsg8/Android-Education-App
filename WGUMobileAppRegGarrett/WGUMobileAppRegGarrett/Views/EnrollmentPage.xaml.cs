using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WGUMobileAppRegGarrett.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WGUMobileAppRegGarrett.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EnrollmentPage : ContentPage
    {
        public EnrollmentViewModel eVM;
        private bool editing;
        public EnrollmentPage()
        {
            InitializeComponent();
            linkViewModel();
            editing = false;
            populatePage();
        }

        private void linkViewModel()
        {
            eVM = new EnrollmentViewModel();
            this.BindingContext = eVM;
        }

        private void populatePage()
        {
            if (editing) editingPage();
            else standardPage();
        }

        // ↓↓↓  Standard Page  ↓↓↓
        private void standardPage()
        {

        }
        // ↑↑↑  Standard Page  ↑↑↑


        // ↓↓↓  Edit Page  ↓↓↓
        private void editingPage()
        {

        }
        // ↑↑↑  Edit Page  ↑↑↑
    }
}