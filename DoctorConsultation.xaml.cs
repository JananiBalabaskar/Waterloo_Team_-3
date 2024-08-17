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
using System.Data.SqlClient;

namespace Digident_Group3
{
    /// <summary>
    /// Interaction logic for DoctorConsultation.xaml
    /// </summary>
    public partial class DoctorConsultation : Page
    {
        string connectionString = @"Data Source=JANANIDESK\MSSQLSERVER05;Initial Catalog=Digidentdb;Integrated Security=True;TrustServerCertificate=True";
        int patientId;

        public DoctorConsultation(int patientId)
        {
            InitializeComponent();
            this.patientId = patientId;
        }

        private void SaveConsultation(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "INSERT INTO Consultations (PatientID, ProcedureDetails, RecheckNeeded) VALUES (@PatientID, @ProcedureDetails, @RecheckNeeded)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PatientID", patientId);
                        command.Parameters.AddWithValue("@ProcedureDetails", ProcedureTextBox.Text);
                        command.Parameters.AddWithValue("@RecheckNeeded", RecheckCheckBox.IsChecked ?? false);
                        command.ExecuteNonQuery();
                    }
                    MessageBox.Show("Consultation details saved successfully.");
                }
                catch (Exception ex)
                {
                    // Handle exception
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }

        private void RecheckCheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow? mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.ChangePage(new DentistDashboard());
            }
        }

        private void ProcedureTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}