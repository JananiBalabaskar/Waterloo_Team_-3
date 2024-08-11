using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.SqlClient;
using Digident_Group3.Interfaces;
using Digident_Group3.Services;
using Digident_Group3.Utils;

namespace Digident_Group3
{
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
            string email = UsernameTextBox.Text.Trim();
            string password = PasswordBox.Password.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                _messageBoxService.Show("Please enter both email and password.", "Error", MessageBoxButton.OK);
                return;
            }

            // Hash the entered password before comparison
            string hashedPassword = HashPassword(password);

            var userData = GetUserData(email, hashedPassword);
            if (userData.HasValue)
            {
                var (userId, userEmail) = userData.Value;
                UserSession.UserID = userId;
                UserSession.UserEmail = userEmail;
                UserSession.CurrentUsername = email; // Use email as the username

                _messageBoxService.Show("Login successful!", "Success", MessageBoxButton.OK);

                MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
                if (mainWindow != null)
                {
                    PatientDashboard dashboard = new PatientDashboard();
                    mainWindow.ChangePage(dashboard);
                }
            }
            else
            {
                _messageBoxService.Show("Invalid email or password.", "Error", MessageBoxButton.OK);
            }
        }

        private (int UserID, string Email)? GetUserData(string email, string hashedPassword)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT UserID, Email FROM Users WHERE Email = @Email AND PasswordHash = @PasswordHash";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@PasswordHash", hashedPassword);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int userID = reader.GetInt32(reader.GetOrdinal("UserID"));
                                string userEmail = reader.GetString(reader.GetOrdinal("Email"));
                                return (userID, userEmail);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _messageBoxService.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK);
                }
            }
            return null;
        }

        private void RegisterHyperlink_Click(object sender, RoutedEventArgs e)
        {
            MainWindow? mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.ChangePage(new Register());
            }
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
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
