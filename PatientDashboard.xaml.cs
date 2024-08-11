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
    /// Interaction logic for PatientDashboard.xaml
    /// </summary>
    public partial class PatientDashboard : Page
    {
        string connectionString = @"Data Source=JANANIDESK\MSSQLSERVER05;Initial Catalog=Digidentdb;Integrated Security=True;TrustServerCertificate=True";
        public string FirstName { get; set; }
        public PatientDashboard()
        {
            InitializeComponent();
            LoadUserData();
            //LoadAppointments();
            DataContext = this;
        }

        private void LoadUserData()
        {
            int userId = UserSession.UserID; // Retrieve UserID from UserSession

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
        /*private void LoadAppointments()
        {
            List<Appointment> appointments = new List<Appointment>();
            int userId = UserSession.UserID; // Use UserSession to get the current user's ID

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT Id, PatientName, PhoneNumber, Address, AppointmentType, AppointmentDate, AppointmentTime, PatientAllergy FROM Appointments WHERE UserID = @UserID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", userId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                appointments.Add(new Appointment
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    PatientName = reader.GetString(reader.GetOrdinal("PatientName")),
                                    PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                    Address = reader.GetString(reader.GetOrdinal("Address")),
                                    AppointmentType = reader.GetString(reader.GetOrdinal("AppointmentType")),
                                    AppointmentDate = reader.GetDateTime(reader.GetOrdinal("AppointmentDate")),
                                    AppointmentTime = reader.GetString(reader.GetOrdinal("AppointmentTime")),
                                    PatientAllergy = reader.GetString(reader.GetOrdinal("PatientAllergy"))
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }

            // Assuming you have a DataGrid or ListView to display the appointments
            DetailsDataGrid.ItemsSource = appointments;
        }*/
        private void BookAppointments(object sender, RoutedEventArgs e)
        {
            int userID = UserSession.UserID; // Assuming you store the UserID in a session or a static class

            Book_Appointment bookAppointmentPage = new Book_Appointment(userID);
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.ChangePage(bookAppointmentPage);
            }
        }

        private void Appointments(object sender, RoutedEventArgs e)
        {
            MainWindow? mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                // Navigate to AppointmentPage
                var appointmentPage = new AppointmentPage(UserSession.UserID); // Pass the username or any other identifier
                mainWindow.ChangePage(appointmentPage);
            }
            //LoadAppointments();
        }
       /* private void LoadAppointments()
        {
            int userId = UserSession.UserID; // Use UserID or Email as needed
            string query = "SELECT * FROM Appointments WHERE PatientName = (SELECT FirstName FROM Users WHERE UserID = @UserID)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", userId);

                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        DetailsDataGrid.ItemsSource = dataTable.DefaultView; // Bind the DataGrid to the DataTable
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }*/



        private void Reports(object sender, RoutedEventArgs e)
        {
            MainWindow? mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                int userID = UserSession.UserID; // Get the current user ID
                mainWindow.ChangePage(new Reports(userID)); // Pass the userID to the Reports constructor
            }
        }

        private void Feedbacks(object sender, RoutedEventArgs e)
        {
            MainWindow? mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.ChangePage(new Feedbacks());
            }
        }

        private void PatientProfile(object sender, RoutedEventArgs e)
        {
            MainWindow? mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                int userID = UserSession.UserID; // Get the current user ID
                mainWindow.ChangePage(new PatientProfile(userID)); // Pass the userID to the PatientProfile constructor
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
                mainWindow.ChangePage(new PatientLoginPage());
            }
        }
    }
}
