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
    /// Interaction logic for CustomerRepDash.xaml
    /// </summary>
    public partial class CustomerRepDash : Page
    {
        string connectionString = @"Data Source=JANANIDESK\MSSQLSERVER05;Initial Catalog=Digidentdb;Integrated Security=True;TrustServerCertificate=True";
        public string LastName { get; set; }

        public CustomerRepDash()
        {
            InitializeComponent();
            LoadCustomerRepData();
            DataContext = this;
        }

        private void LoadCustomerRepData()
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
                // Navigate to AppointmentPage for customer representatives
                var appointmentPage = new CustomerRepAppointmentPage(UserSession.UserID);
                mainWindow.ChangePage(appointmentPage);
            }
        }

        private void Reports(object sender, RoutedEventArgs e)
        {
            MainWindow? mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                // Navigate to Reports page
                mainWindow.ChangePage(new ManagerReports(UserSession.UserID));
            }
        }

        private void ManagerProfile(object sender, RoutedEventArgs e)
        {
            MainWindow? mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                // Navigate to CustomerRepProfile page
                mainWindow.ChangePage(new CustomerRepProfile(UserSession.UserID));
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
    }
}