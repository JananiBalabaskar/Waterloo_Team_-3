using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace DashboardLogin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string connectionString = "Data Source=localhost;Initial Catalog=DentalCheckUp;Integrated Security=True";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text;
            string password = PasswordBox.Password;

            // Get the selected role from the ComboBox
            ComboBoxItem selectedItem = jobComboBox.SelectedItem as ComboBoxItem;

            if (selectedItem != null)
            {
                // Retrieve the selected role
                string selectedRole = selectedItem.Content.ToString();

                if (ValidateCredentials(email, password, selectedRole))
                {
                    MessageBox.Show("Role: " + selectedRole);

                    // Proceed with opening the dashboard based on the role
                    OpenDashboardBasedOnRole(selectedRole);
                }
                else
                {
                    MessageBox.Show("Invalid username or password.");
                }
            }
            else
            {
                // Handle the case where no role is selected
                MessageBox.Show("Please select a role.");
            }
        }

        private bool ValidateCredentials(string email, string password, string selectedRole)
        {
            //userRole = null;  // Initialize the output parameter

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "";

                    // Determine which table to query based on the role
                    switch (selectedRole)
                    {
                        case "Manager":
                            query = "SELECT Role FROM Managers WHERE Email = @Email AND Password = @Password";
                            break;
                        case "Dentist":
                            query = "SELECT Role FROM Dentists WHERE Email = @Email AND Password = @Password";
                            break;
                        case "Customer representatives":
                            query = "SELECT Role FROM CustomerRepresentatives WHERE Email = @Email AND Password = @Password";
                           break;
                        default:
                            MessageBox.Show("Invalid role specified.");
                            return false;
                    }

                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Password", password);

                        Console.WriteLine($"Executing SQL query: {command.CommandText}");

                        object result = command.ExecuteScalar();

                        if (result != null)
                        {
                            selectedRole = result.ToString();
                            return true;
                        }
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                    return false;
                }
            }
        }

        private void OpenDashboardBasedOnRole(string role)
        {
            Window dashboardWindow = null;

            // Determine which window to open based on role
            switch (role)
            {
                case "Manager":
                    dashboardWindow = new PatientDashboardWindow();
                    break;
                case "Dentist":
                    dashboardWindow = new DoctorDashboardWindow();
                    break;
                case "Customer representatives":
                    dashboardWindow = new CustomerRepDashboardWindow();
                    break;
                default:
                    MessageBox.Show("Unknown role.");
                    return;
            }

            // Open the selected dashboard window
            dashboardWindow.Show();

            // Close the current login window
            this.Close();
        }

        private void JobComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Get the selected ComboBoxItem
            ComboBoxItem selectedItem = jobComboBox.SelectedItem as ComboBoxItem;

            if (selectedItem != null)
            {
                string selectedRole = selectedItem.Content.ToString();
                MessageBox.Show("Selected Role: " + selectedRole);
            }
            else
            {
                MessageBox.Show("Please select a role.");
            }
        }
    }
}
