using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for Book_Appointment.xaml
    /// </summary>
    public partial class Book_Appointment : Page
    {
        private const string connectionString = @"Data Source=JANANIDESK\MSSQLSERVER05;Initial Catalog=Digidentdb;Integrated Security=True;TrustServerCertificate=True";
        public Book_Appointment()
        {
            InitializeComponent();
        }

        private bool ValidateAppointment()
        {
            bool isValid = true;

            // check Patient Name
            isValid &= ValidatePatientName(txtPatientName.Text);
            // check Phone Number
            isValid &= ValidatePhoneNumber(txtPhoneNumber.Text);
            // check Address
            isValid &= ValidateAddress(txtAddress.Text);
            // check Appointment Type
            isValid &= !string.IsNullOrWhiteSpace(cmbAppointmentType.Text);
            // check Appointment Date
            isValid &= dpAppointmentDate.SelectedDate.HasValue;
            // check Appointment Time
            isValid &= !string.IsNullOrWhiteSpace(cmbAppointmentTime.Text);
            // check Patient Allergy Info
            isValid &= ValidatePatientAllergy(txtPatientinfo.Text);

            return isValid;
        }

        // Error Handling 
        private bool ValidatePatientName(string PatientName)
        {
            if (string.IsNullOrWhiteSpace(PatientName))
            {
                SetError(txtPatientName, "Invalid Input.");
                return false;
            }
            else if (!Regex.IsMatch(PatientName, @"^[a-zA-Z]+$"))
            {
                SetError(txtPatientName, "only alphabets.");
                return false;
            }

            ClearError(txtPatientName);
            return true;
        }

        private bool ValidatePhoneNumber(string PhoneNumber)
        {
            if (!Regex.IsMatch(PhoneNumber, @"^\d{10}$") || PhoneNumber.All(c => c == '0'))
            {
                SetError(txtPhoneNumber, "Invalid Input");
                return false;
            }

            ClearError(txtPhoneNumber);
            return true;
        }

        private bool ValidateAddress(string Address)
        {
            if (!Regex.IsMatch(Address, @"^[a-zA-Z0-9\s\-\.,#]+$"))
            {
                SetError(txtAddress, "Invalid Input");
                return false;
            }

            ClearError(txtAddress);
            return true;
        }

        private bool ValidatePatientAllergy(string PatientAllergy)
        {
            if (!Regex.IsMatch(PatientAllergy, @"^[a-zA-Z]+$"))
            {
                SetError(txtPatientinfo, "Invalid Input");
                return false;
            }

            ClearError(txtPatientinfo);
            return true;
        }

        private void SetError(TextBox textBox, string errorMessage)
        {
            TextBlock err = GetErrorTextBlock(textBox);
            err.Text = errorMessage;
            err.Visibility = Visibility.Visible;
        }

        // Remove error message 
        private void ClearError(TextBox textBox)
        {
            TextBlock err = GetErrorTextBlock(textBox);
            err.Visibility = Visibility.Collapsed;
        }

        // Get error message when handling error
        private TextBlock GetErrorTextBlock(TextBox textBox)
        {
            switch (textBox.Name)
            {
                case "txtPatientName":
                    return PatientNameError;
                case "txtPhoneNumber":
                    return PhoneNumberError;
                case "txtAddress":
                    return AddressError;
                case "txtPatientinfo":
                    return PatientError;
                default:
                    throw new Exception("Invalid TextBox name.");
            }
        }

        // Clearing all input fields
        private void ClearInputFields()
        {
            txtPatientName.Clear();
            txtPhoneNumber.Clear();
            txtAddress.Clear();
            cmbAppointmentType.SelectedIndex = -1;
            dpAppointmentDate.SelectedDate = null;
            cmbAppointmentTime.SelectedIndex = -1;
            txtPatientinfo.Clear();
        }

        public class Appointment
        {
            public string PatientName { get; set; } = "";
            public string PhoneNumber { get; set; } = "";
            public string Address { get; set; } = "";
            public string AppointmentType { get; set; } = "";
            public DateTime AppointmentDate { get; set; }
            public string AppointmentTime { get; set; } = "";
            public string PatientInfo { get; set; } = "";
        }

        private void Bookbutton(object sender, RoutedEventArgs e)
        {
            if (ValidateAppointment())
            {
                Appointment appointment = new Appointment
                {
                    PatientName = txtPatientName.Text,
                    PhoneNumber = txtPhoneNumber.Text,
                    Address = txtAddress.Text,
                    AppointmentType = cmbAppointmentType.Text,
                    AppointmentDate = dpAppointmentDate.SelectedDate.Value,
                    AppointmentTime = cmbAppointmentTime.Text,
                    PatientInfo = txtPatientinfo.Text
                };

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand("InsertAppointments", connection))
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@PatientName", appointment.PatientName);
                            command.Parameters.AddWithValue("@PhoneNumber", appointment.PhoneNumber);
                            command.Parameters.AddWithValue("@Address", appointment.Address);
                            command.Parameters.AddWithValue("@AppointmentType", appointment.AppointmentType);
                            command.Parameters.AddWithValue("@AppointmentDate", appointment.AppointmentDate);
                            command.Parameters.AddWithValue("@AppointmentTime", appointment.AppointmentTime);
                            command.Parameters.AddWithValue("@PatientAllergy", appointment.PatientInfo);

                            command.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Appointment booked successfully!");
                    MainWindow? mainWindow = Window.GetWindow(this) as MainWindow;
                    if (mainWindow != null)
                    {
                        mainWindow.ChangePage(new PatientDashboard());
                    }

                    ClearInputFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while booking the appointment: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Please fill in all required fields with valid data.");
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
}
