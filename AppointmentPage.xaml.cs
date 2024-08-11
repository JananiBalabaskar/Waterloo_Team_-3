using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Data;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using Digident_Group3.Models;

namespace Digident_Group3
{
    public partial class AppointmentPage : Page
    {
        private const string connectionString = @"Data Source=JANANIDESK\MSSQLSERVER05;Initial Catalog=Digidentdb;Integrated Security=True;TrustServerCertificate=True";
        private int _userID;
        private UserAppointment _appointment;

        public AppointmentPage(int userID)
        {
            InitializeComponent();
            _userID = userID;
            LoadAppointments();
        }

        private void LoadAppointments()
        {
            List<UserAppointment> appointments = new List<UserAppointment>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT AppointmentID, PatientName, AppointmentDate, AppointmentTime, AppointmentType FROM Appointments WHERE UserID = @UserID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserID", _userID);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        appointments.Add(new UserAppointment
                        {
                            AppointmentID = Convert.ToInt32(reader["AppointmentID"]),
                            PatientName = reader["PatientName"].ToString(),
                            AppointmentDate = Convert.ToDateTime(reader["AppointmentDate"]),
                            AppointmentTime = reader["AppointmentTime"].ToString(),
                            AppointmentType = reader["AppointmentType"].ToString()
                        });
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading appointments: {ex.Message}");
                }
            }

            AppointmentsDataGrid.ItemsSource = appointments;
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            UserAppointment selectedAppointment = (UserAppointment)AppointmentsDataGrid.SelectedItem;
            if (selectedAppointment != null)
            {
                // Open the EditAppointmentWindow to edit the selected appointment
                EditAppointment editWindow = new EditAppointment(selectedAppointment, _userID);
                editWindow.ShowDialog();

                // After editing, reload the appointments to reflect any changes
                LoadAppointments();
            }
            else
            {
                MessageBox.Show("Please select an appointment to update.");
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            UserAppointment selectedAppointment = (UserAppointment)AppointmentsDataGrid.SelectedItem;
            if (selectedAppointment != null)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this appointment?", "Delete Confirmation", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        string query = "DELETE FROM Appointments WHERE AppointmentID = @AppointmentID";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@AppointmentID", selectedAppointment.AppointmentID);

                        try
                        {
                            connection.Open();
                            command.ExecuteNonQuery();
                            LoadAppointments(); // Reload appointments after deletion
                            MessageBox.Show("Appointment deleted successfully.");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error deleting appointment: {ex.Message}");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select an appointment to delete.");
            }
        }

        private void Backbutton(object sender, RoutedEventArgs e)
        {
            MainWindow? mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.ChangePage(new PatientDashboard());
            }
        }
    }

    public class UserAppointment
    {
        public int AppointmentID { get; set; }
        public string PatientName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string AppointmentTime { get; set; }
        public string AppointmentType { get; set; }
    }
}