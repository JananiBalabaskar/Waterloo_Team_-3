using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace appointment_screen
{
    public partial class MainWindow : Window
    {
        // ObservableCollection to hold the list of appointments
        public ObservableCollection<Appointment> Appointments { get; set; }
        private Appointment selectedAppointment; // The currently selected appointment

        public MainWindow()
        {
            InitializeComponent();
            Appointments = new ObservableCollection<Appointment>();
            DataContext = this;
        }

        // Event handler for scheduling a new appointment
        private void ScheduleAppointment_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve user input from the UI
            string patientName = txtPatientName.Text.Trim();
            DateTime? selectedDate = dpAppointmentDate.SelectedDate;
            string timeString = ((ComboBoxItem)cbAppointmentTime.SelectedItem)?.Content?.ToString();
            string notes = txtNotes.Text.Trim();

            // Validate input fields
            if (string.IsNullOrWhiteSpace(patientName) || selectedDate == null || string.IsNullOrWhiteSpace(timeString))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            // Combine date and time into a single DateTime object
            DateTime appointmentDateTime;
            try
            {
                appointmentDateTime = DateTime.ParseExact(
                    selectedDate.Value.ToString("yyyy-MM-dd") + " " + timeString,
                    "yyyy-MM-dd hh:mm tt",
                    CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                MessageBox.Show("Invalid time format.");
                return;
            }

            // Create a new appointment and add it to the collection
            Appointment newAppointment = new Appointment
            {
                PatientName = patientName,
                AppointmentDate = appointmentDateTime,
                Notes = notes
            };

            Appointments.Add(newAppointment);
            ClearFields(); // Clear the input fields after adding the appointment
        }

        // Event handler for updating an existing appointment
        private void UpdateAppointment_Click(object sender, RoutedEventArgs e)
        {
            if (selectedAppointment == null)
            {
                MessageBox.Show("Please select an appointment to update.");
                return;
            }

            // Retrieve user input from the UI
            string patientName = txtPatientName.Text.Trim();
            DateTime? selectedDate = dpAppointmentDate.SelectedDate;
            string timeString = ((ComboBoxItem)cbAppointmentTime.SelectedItem)?.Content?.ToString();
            string notes = txtNotes.Text.Trim();

            // Validate input fields
            if (string.IsNullOrWhiteSpace(patientName) || selectedDate == null || string.IsNullOrWhiteSpace(timeString))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            // Combine date and time into a single DateTime object
            DateTime appointmentDateTime;
            try
            {
                appointmentDateTime = DateTime.ParseExact(
                    selectedDate.Value.ToString("yyyy-MM-dd") + " " + timeString,
                    "yyyy-MM-dd hh:mm tt",
                    CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                MessageBox.Show("Invalid time format.");
                return;
            }

            // Update the selected appointment with new values
            selectedAppointment.PatientName = patientName;
            selectedAppointment.AppointmentDate = appointmentDateTime;
            selectedAppointment.Notes = notes;

            // Refresh the ListBox to show updated information
            lbAppointments.Items.Refresh();
            ClearFields(); // Clear the input fields after updating the appointment
        }

        // Event handler for deleting an existing appointment
        private void DeleteAppointment_Click(object sender, RoutedEventArgs e)
        {
            if (selectedAppointment == null)
            {
                MessageBox.Show("Please select an appointment to delete.");
                return;
            }

            // Remove the selected appointment from the collection
            Appointments.Remove(selectedAppointment);
            ClearFields(); // Clear the input fields after deleting the appointment
        }

        // Event handler for when the selected appointment in the ListBox changes
        private void LbAppointments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedAppointment = lbAppointments.SelectedItem as Appointment;
            if (selectedAppointment != null)
            {
                // Populate the input fields with the selected appointment's details
                txtPatientName.Text = selectedAppointment.PatientName;
                dpAppointmentDate.SelectedDate = selectedAppointment.AppointmentDate.Date;
                cbAppointmentTime.Text = selectedAppointment.AppointmentDate.ToString("hh:mm tt", CultureInfo.InvariantCulture);
                txtNotes.Text = selectedAppointment.Notes;
            }
        }

        // Helper method to clear the input fields
        private void ClearFields()
        {
            txtPatientName.Clear();
            dpAppointmentDate.SelectedDate = null;
            cbAppointmentTime.SelectedIndex = -1;
            txtNotes.Clear();
            selectedAppointment = null;
        }
    }

    // Appointment class implementing INotifyPropertyChanged to support property change notifications
    public class Appointment : INotifyPropertyChanged
    {
        private string patientName;
        private DateTime appointmentDate;
        private string notes;

        // Property for PatientName with notification support
        public string PatientName
        {
            get => patientName;
            set
            {
                if (patientName != value)
                {
                    patientName = value;
                    OnPropertyChanged(nameof(PatientName));
                    OnPropertyChanged(nameof(DisplayText));
                }
            }
        }

        // Property for AppointmentDate with notification support
        public DateTime AppointmentDate
        {
            get => appointmentDate;
            set
            {
                if (appointmentDate != value)
                {
                    appointmentDate = value;
                    OnPropertyChanged(nameof(AppointmentDate));
                    OnPropertyChanged(nameof(DisplayText));
                }
            }
        }

        // Property for Notes with notification support
        public string Notes
        {
            get => notes;
            set
            {
                if (notes != value)
                {
                    notes = value;
                    OnPropertyChanged(nameof(Notes));
                    OnPropertyChanged(nameof(DisplayText));
                }
            }
        }

        // Property to display appointment details in a readable format
        public string DisplayText => $"{PatientName} - {AppointmentDate:MM/dd/yyyy hh:mm tt} - {Notes}";

        public event PropertyChangedEventHandler PropertyChanged;

        // Method to raise the PropertyChanged event
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
