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
using System.Windows.Shapes;
using System.Data.SqlClient;

namespace Digident_Group3
{
    public partial class EditAppointment : Window
    {
        private UserAppointment _appointment;
        private int _userID;

        public EditAppointment(UserAppointment appointment, int userID)
        {
            InitializeComponent();
            _appointment = appointment;
            _userID = userID;
            LoadAppointmentDetails();
        }

        private void LoadAppointmentDetails()
        {
            txtPatientName.Text = _appointment.PatientName;
            dpAppointmentDate.SelectedDate = _appointment.AppointmentDate;

            cmbAppointmentTime.SelectedItem = cmbAppointmentTime.Items.Cast<ComboBoxItem>()
                                            .FirstOrDefault(item => item.Content.ToString() == _appointment.AppointmentTime);

            cmbAppointmentType.SelectedItem = cmbAppointmentType.Items.Cast<ComboBoxItem>()
                                             .FirstOrDefault(item => item.Content.ToString() == _appointment.AppointmentType);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Update the _appointment object with the new values from the UI
            _appointment.PatientName = txtPatientName.Text;
            _appointment.AppointmentDate = dpAppointmentDate.SelectedDate ?? DateTime.Now;
            _appointment.AppointmentTime = ((ComboBoxItem)cmbAppointmentTime.SelectedItem).Content.ToString();
            _appointment.AppointmentType = ((ComboBoxItem)cmbAppointmentType.SelectedItem).Content.ToString();

            // Check if the selected date and time are available
            if (!IsAppointmentTimeAvailable(_appointment.AppointmentDate, _appointment.AppointmentTime, _appointment.AppointmentID))
            {
                MessageBox.Show("The selected date and time are already booked. Please choose another time.");
                return;
            }

            // Update the appointment in the database
            UpdateAppointmentInDatabase(_appointment);

            // Close the window after saving
            this.Close();
        }

        private bool IsAppointmentTimeAvailable(DateTime date, string time, int appointmentId)
        {
            bool isAvailable = true;
            string connectionString = @"Data Source=JANANIDESK\MSSQLSERVER05;Initial Catalog=Digidentdb;Integrated Security=True;TrustServerCertificate=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT COUNT(*)
                    FROM Appointments
                    WHERE AppointmentDate = @AppointmentDate
                    AND AppointmentTime = @AppointmentTime
                    AND AppointmentID != @AppointmentID"; // Exclude the current appointment

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@AppointmentDate", date);
                command.Parameters.AddWithValue("@AppointmentTime", time);
                command.Parameters.AddWithValue("@AppointmentID", appointmentId);

                try
                {
                    connection.Open();
                    int count = (int)command.ExecuteScalar();
                    if (count > 0)
                    {
                        isAvailable = false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error checking appointment availability: {ex.Message}");
                }
            }

            return isAvailable;
        }

        private void UpdateAppointmentInDatabase(UserAppointment appointment)
        {
            string connectionString = @"Data Source=JANANIDESK\MSSQLSERVER05;Initial Catalog=Digidentdb;Integrated Security=True;TrustServerCertificate=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
                    UPDATE Appointments
                    SET PatientName = @PatientName,
                        AppointmentDate = @AppointmentDate,
                        AppointmentTime = @AppointmentTime,
                        AppointmentType = @AppointmentType
                    WHERE AppointmentID = @AppointmentID";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@PatientName", appointment.PatientName);
                command.Parameters.AddWithValue("@AppointmentDate", appointment.AppointmentDate);
                command.Parameters.AddWithValue("@AppointmentTime", appointment.AppointmentTime);
                command.Parameters.AddWithValue("@AppointmentType", appointment.AppointmentType);
                command.Parameters.AddWithValue("@AppointmentID", appointment.AppointmentID);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Appointment updated successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while updating the appointment: {ex.Message}");
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}