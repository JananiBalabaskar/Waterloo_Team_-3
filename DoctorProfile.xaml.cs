using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace Digident_Group3
{
    public partial class DoctorProfile : Page
    {
        string connectionString = @"Data Source=JANANIDESK\MSSQLSERVER05;Initial Catalog=Digidentdb;Integrated Security=True;TrustServerCertificate=True";

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string LicenseNumber { get; set; }

        public DoctorProfile()
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

                    // Query to get dentist details from Users table
                    string detailsQuery = "SELECT FirstName, LastName, PhoneNumber, Address, Email FROM Users WHERE UserID = @UserID";
                    using (SqlCommand command = new SqlCommand(detailsQuery, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", userId);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                FirstName = reader["FirstName"].ToString();
                                LastName = reader["LastName"].ToString();
                                PhoneNumber = reader["PhoneNumber"].ToString();
                                Address = reader["Address"].ToString();
                                Email = reader["Email"].ToString();
                            }
                            else
                            {
                                MessageBox.Show("User details not found.");
                                return;
                            }
                        }
                    }

                    // Query to get the dentist's license number from the Dentists table
                    string licenseQuery = "SELECT LicenseNumber FROM Dentists WHERE UserID = @UserID";
                    using (SqlCommand command = new SqlCommand(licenseQuery, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", userId);
                        LicenseNumber = command.ExecuteScalar()?.ToString();
                        if (string.IsNullOrEmpty(LicenseNumber))
                        {
                            MessageBox.Show("Dentist's license number not found.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow? mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.ChangePage(new DentistDashboard());
            }
        }

        private void LoadAppointmentHistory()
        {
            int userId = UserSession.UserID;
            List<PatientAppointment> appointments = new List<PatientAppointment>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT AppointmentDate, AppointmentTime, AppointmentType, DentistName FROM Appointments WHERE UserID = @UserID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserID", userId);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        appointments.Add(new PatientAppointment
                        {
                            AppointmentDate = Convert.ToDateTime(reader["AppointmentDate"]),
                            AppointmentTime = reader["AppointmentTime"].ToString(),
                            AppointmentType = reader["AppointmentType"].ToString(),
                            DentistName = reader["DentistName"].ToString()
                        });
                    }
                    reader.Close();
                    AppointmentsDataGrid.ItemsSource = appointments; // Bind the data to the DataGrid
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading appointment history: {ex.Message}");
                }
            }
        }
    }
}
