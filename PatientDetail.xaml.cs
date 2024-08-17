using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Digident_Group3
{
    /// <summary>
    /// Interaction logic for PatientDetail.xaml
    /// </summary>
    public partial class PatientDetail : Page
    {
        public PatientDetail()
        {
            InitializeComponent();
            LoadPatientList();
        }
        private void LoadPatientList()
        {
            // Example data - replace with actual data loading logic
            PatientListView.ItemsSource = new List<Patient>
            {
                new Patient { PatientID = 1, Name = "John Doe", PhoneNumber = "123-456-7890", Email = "john.doe@example.com", Address = "123 Elm Street", TreatmentHistory = "None", AdditionalInfo = "N/A" },
                new Patient { PatientID = 2, Name = "Jane Smith", PhoneNumber = "987-654-3210", Email = "jane.smith@example.com", Address = "456 Oak Avenue", TreatmentHistory = "Routine Checkup", AdditionalInfo = "N/A" },
                new Patient { PatientID = 3, Name = "Michael Brown", PhoneNumber = "456-789-1230", Email = "michael.brown@example.com", Address = "789 Pine Road", TreatmentHistory = "Flu Vaccination", AdditionalInfo = "N/A" },
                new Patient { PatientID = 4, Name = "Emily Davis", PhoneNumber = "321-654-9870", Email = "emily.davis@example.com", Address = "321 Maple Lane", TreatmentHistory = "Dental Cleaning", AdditionalInfo = "N/A" },
                new Patient { PatientID = 5, Name = "Daniel Wilson", PhoneNumber = "654-321-0987", Email = "daniel.wilson@example.com", Address = "654 Cedar Boulevard", TreatmentHistory = "Annual Physical", AdditionalInfo = "N/A" }
            };
        }

        private void PatientListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (PatientListView.SelectedItem != null)
            {
                var selectedPatient = PatientListView.SelectedItem as Patient;
                ShowPatientDetails(selectedPatient);
            }
        }

        private void ShowPatientDetails(Patient patient)
        {
            CustomerNameTextBox.Text = patient.Name;
            PhoneNumberTextBox.Text = patient.PhoneNumber;
            EmailTextBox.Text = patient.Email;
            AddressTextBox.Text = patient.Address;
            TreatmentHistoryTextBox.Text = patient.TreatmentHistory;
            AdditionalInfoTextBox.Text = patient.AdditionalInfo;

            DetailsPopup.IsOpen = true;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            DetailsPopup.IsOpen = false;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow? mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.ChangePage(new DentistDashboard());
            }
        }
        public class Patient
        {
            public int PatientID { get; set; }
            public string Name { get; set; }
            public string PhoneNumber { get; set; }
            public string Email { get; set; }
            public string Address { get; set; }
            public string TreatmentHistory { get; set; }
            public string AdditionalInfo { get; set; }
        }
    }
}
