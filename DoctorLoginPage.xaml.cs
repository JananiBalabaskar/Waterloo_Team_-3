using NUnit.Framework;
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
    /// Interaction logic for DoctorLoginPage.xaml
    /// </summary>
    public partial class DoctorLoginPage : Page
    {
        // Correctly placed connectionString field at the class level
        private const string connectionString = @"Data Source=JANANIDESK\MSSQLSERVER05;Initial Catalog=Digidentdb;Integrated Security=True;TrustServerCertificate=True";

        public DoctorLoginPage()
        {
            InitializeComponent();
        }

        private void Homebutton(object sender, RoutedEventArgs e)
        {
            MainWindow window1 = new MainWindow();
            window1.Show();
            Window.GetWindow(this)?.Close();
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

                    // Proceed with navigating to the dashboard based on the role
                    NavigateToDashboardBasedOnRole(selectedRole);
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
                                    query = @"
                                    SELECT U.UserID 
                                    FROM Users U
                                    INNER JOIN Managers M ON U.UserID = M.UserID
                                    WHERE U.Email = @Email AND U.PasswordHash = @PasswordHash";
                            break;
                            case "Dentist":
                            query = @"
                                    SELECT U.UserID 
                                    FROM Users U
                                    INNER JOIN Dentists D ON U.UserID = D.UserID
                                    WHERE U.Email = @Email AND U.PasswordHash = @PasswordHash";
                            break;
                            case "Customer representatives":
                            query = @"
                                    SELECT U.UserID 
                                    FROM Users U
                                    INNER JOIN CustomerRepresentatives CR ON U.UserID = CR.UserID
                                    WHERE U.Email = @Email AND U.PasswordHash = @PasswordHash";
                            break;
                            default:
                            MessageBox.Show("Invalid role specified.");
                            return false;
                    }

                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@PasswordHash", password);

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

        private void NavigateToDashboardBasedOnRole(string role)
        {
            Page dashboardPage = null;

            // Determine which page to navigate to based on role
            switch (role)
            {
                case "Manager":
                    dashboardPage = new ManagerDash();
                    break;
                case "Dentist":
                    dashboardPage = new DentistDashboard();
                    break;
                case "Customer representatives":
                    dashboardPage = new CustomerRepDash();
                    break;
                default:
                    MessageBox.Show("Unknown role.");
                    return;
            }

            MainWindow? mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                // Use the main window's ChangePage method to navigate to the selected dashboard page
                mainWindow.ChangePage(dashboardPage);
            }
            else
            {
                MessageBox.Show("Unable to navigate. MainWindow is not available.");
            }
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
        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

    }
}