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
using System.Data;
using Digident_Group3.Models;

namespace Digident_Group3
{
    /// <summary>
    /// Interaction logic for ManagerDash.xaml
    /// </summary>
    public partial class ManagerDash : Page
    {
        string connectionString = @"Data Source=JANANIDESK\MSSQLSERVER05;Initial Catalog=Digidentdb;Integrated Security=True;TrustServerCertificate=True";
        public string LastName { get; set; }

        public ManagerDash()
        {
            InitializeComponent();
            LoadManagerData();
            DataContext = this;
        }

        private void LoadManagerData()
        {
            int userId = UserSession.UserID;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT LastName FROM Users WHERE UserID = @UserID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", userId);
                        var result = command.ExecuteScalar();
                        if (result != null)
                        {
                            LastName = result.ToString();
                        }
                        else
                        {
                            LastName = "Unknown";
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }

        private void Appointments(object sender, RoutedEventArgs e)
        {
            MainWindow? mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                // Navigate to AppointmentPage for managers
                var appointmentPage = new ManagerAppointmentPage();
                mainWindow.ChangePage(appointmentPage);
            }
        }

        private void Reports(object sender, RoutedEventArgs e)
        {
            MainWindow? mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                // Navigate to Reports page
                mainWindow.ChangePage(new ReportsManager());
            }
        }

        private void ManagerProfile(object sender, RoutedEventArgs e)
        {
            MainWindow? mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                // Navigate to ManagerProfile page
                mainWindow.ChangePage(new ManagerProfile(UserSession.UserID));
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            UserSession.UserID = 0;
            UserSession.UserEmail = string.Empty;
            UserSession.CurrentUsername = string.Empty;

            // Navigate back to the login page
            MainWindow? mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.ChangePage(new DoctorLoginPage());
            }
        }

        private void Feedbacks(object sender, RoutedEventArgs e)
        {
            MainWindow? mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                // Navigate to ManagerProfile page
                mainWindow.ChangePage(new ManagerFeedbacks());
            }
        }
    }
}