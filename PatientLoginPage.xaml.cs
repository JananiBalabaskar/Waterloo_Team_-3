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
using Digident_Group3.Interfaces;
using Digident_Group3.Services;
using Microsoft.Data.SqlClient;
using System.Configuration;

namespace Digident_Group3
{
    /// <summary>
    /// Interaction logic for PatientLoginPage.xaml
    /// </summary>
    public partial class PatientLoginPage : Page
    {
        private readonly IDatabaseService _databaseService;
        private readonly IMessageBoxService _messageBoxService;

        private const string connectionString = @"Data Source=JANANIDESK\MSSQLSERVER05;Initial Catalog=Digidentdb;Integrated Security=True;TrustServerCertificate=True";

        public PatientLoginPage(IDatabaseService databaseService, IMessageBoxService messageBoxService)
        {
            InitializeComponent();
            _databaseService = databaseService;
            _messageBoxService = messageBoxService;
        }

        public PatientLoginPage()
            : this(new DatabaseService(ConfigurationManager.ConnectionStrings["MyDbConnectionString"].ConnectionString), new MessageBoxService())
        {
        }

        private void Homebutton(object sender, RoutedEventArgs e)
        {
            MainWindow window1 = new MainWindow();
            window1.Show();
            Window.GetWindow(this)?.Close();
        }

        internal void Loginbutton(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text.Trim();
            string password = PasswordBox.Password.Trim();

            Console.WriteLine($"Username: '{username}', Password: '{password}'");

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                _messageBoxService.Show("Please enter both username and password.", "Error", MessageBoxButton.OK);
                return;
            }

            if (_databaseService.ValidateCredentials(username, password))
            {
                _messageBoxService.Show("Login successful!", "Success", MessageBoxButton.OK);

                MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
                if (mainWindow != null)
                {
                    mainWindow.ChangePage(new PatientDashboard());
                }
            }
            else
            {
                _messageBoxService.Show("Invalid username or password.", "Error", MessageBoxButton.OK);
            }
        }

        private bool ValidateCredentials(string email, string password)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM Users WHERE Email = @Email AND Password = @Password";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Password", password);

                        Console.WriteLine($"Executing SQL query: {command.CommandText}");

                        int count = (int)command.ExecuteScalar();
                        return count > 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                    return false;
                }
            }
        }

        private void RegisterHyperlink_Click(object sender, RoutedEventArgs e)
        {
            MainWindow? mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.ChangePage(new Register());
            }
        }
    }
}
