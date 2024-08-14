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
    /// Interaction logic for PatientProfile.xaml
    /// </summary>
    public partial class PatientProfile : Page
    {
        private const string connectionString = @"Data Source=JANANIDESK\MSSQLSERVER05;Initial Catalog=Digidentdb;Integrated Security=True;TrustServerCertificate=True";
        private int _userID;

        public PatientProfile(int userID)
        {
            InitializeComponent();
            _userID = userID;
            LoadPatientData();
            LoadAppointmentHistory();
        }

        private void LoadPatientData()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT FirstName, LastName, PhoneNumber, Address, Email FROM Users WHERE UserID = @UserID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserID", _userID);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        txtFirstName.Text = reader["FirstName"].ToString();
                        txtLastName.Text = reader["LastName"].ToString();
                        txtPhoneNumber.Text = reader["PhoneNumber"].ToString();
                        txtAddress.Text = reader["Address"].ToString();
                        txtEmail.Text = reader["Email"].ToString();
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading patient data: {ex.Message}");
                }
            }
        }

        private void LoadAppointmentHistory()
        {
            List<PatientAppointment> appointments = new List<PatientAppointment>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT AppointmentDate, AppointmentTime, AppointmentType, DentistName FROM Appointments WHERE UserID = @UserID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserID", _userID);

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
                    AppointmentsDataGrid.ItemsSource = appointments;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading appointment history: {ex.Message}");
                }
            }
        }

        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE Users SET FirstName = @FirstName, LastName = @LastName, PhoneNumber = @PhoneNumber, Address = @Address, Email = @Email WHERE UserID = @UserID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@FirstName", txtFirstName.Text);
                command.Parameters.AddWithValue("@LastName", txtLastName.Text);
                command.Parameters.AddWithValue("@PhoneNumber", txtPhoneNumber.Text);
                command.Parameters.AddWithValue("@Address", txtAddress.Text);
                command.Parameters.AddWithValue("@Email", txtEmail.Text);
                command.Parameters.AddWithValue("@UserID", _userID);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Changes saved successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving changes: {ex.Message}");
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Clear fields or reset changes if necessary, or simply navigate back
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.ChangePage(new PatientDashboard());
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate back to the PatientDashboard
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.ChangePage(new PatientDashboard());
            }
        }
    }

    public class PatientAppointment
    {
        public DateTime AppointmentDate { get; set; }
        public string AppointmentTime { get; set; }
        public string AppointmentType { get; set; }
        public string DentistName { get; set; }
    }
}