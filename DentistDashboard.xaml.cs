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
using Microsoft.Data.SqlClient;
using Digident_Group3.Utils;
using Digident_Group3.Models;
using System.Data;

namespace Digident_Group3
{
    /// <summary>
    /// Interaction logic for DentistDashboard.xaml
    /// </summary>
    public partial class DentistDashboard : Page
    {
        string connectionString = @"Data Source=JANANIDESK\MSSQLSERVER05;Initial Catalog=Digidentdb;Integrated Security=True;TrustServerCertificate=True";
        public string FirstName { get; set; }

        public DentistDashboard()
        {
            InitializeComponent();
            LoadDentistData();
            DataContext = this;
        }

        private void LoadDentistData()
        {
            int userId = UserSession.UserID;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT FirstName FROM Users WHERE UserID = @UserID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", userId);
                        var result = command.ExecuteScalar();
                        if (result != null)
                        {
                            FirstName = result.ToString();
                        }
                        else
                        {
                            FirstName = "Unknown";
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }

        private void BookAppointments(object sender, RoutedEventArgs e)
        {
            // Navigate to Book Appointments page
        }

        private void Appointments(object sender, RoutedEventArgs e)
        {
            // Navigate to Appointments page
        }

        private void Reports(object sender, RoutedEventArgs e)
        {
            // Navigate to Reports page
        }

        private void PatientProfile(object sender, RoutedEventArgs e)
        {
            // Navigate to Patient Profile page
        }

        private void Feedbacks(object sender, RoutedEventArgs e)
        {
            // Navigate to Feedbacks page
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            UserSession.UserID = 0;
            UserSession.UserEmail = string.Empty;
            UserSession.CurrentUsername = string.Empty;

            MainWindow? mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.ChangePage(new DoctorLoginPage());
            }
        }
    }
}
