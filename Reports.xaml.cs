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
    /// Interaction logic for Reports.xaml
    /// </summary>
    public partial class Reports : Page
    {
        private const string connectionString = @"Data Source=JANANIDESK\MSSQLSERVER05;Initial Catalog=Digidentdb;Integrated Security=True;TrustServerCertificate=True";
        private int _userID;

        public Reports(int userID)
        {
            InitializeComponent();
            _userID = userID;
            LoadUploadedFiles();
            LoadConsultationHistory();
        }

        private void LoadUploadedFiles()
        {
            List<PatientFile> files = new List<PatientFile>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT FileName, FilePath, UploadDate FROM Files WHERE UserID = @UserID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserID", _userID);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        files.Add(new PatientFile
                        {
                            FileName = reader["FileName"].ToString(),
                            FilePath = reader["FilePath"].ToString(),
                            UploadDate = Convert.ToDateTime(reader["UploadDate"])
                        });
                    }
                    reader.Close();
                    FilesDataGrid.ItemsSource = files;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading files: {ex.Message}");
                }
            }
        }

        private void LoadConsultationHistory()
        {
            List<Consultation> consultations = new List<Consultation>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT ConsultationDate, DoctorName, Summary FROM Consultations WHERE UserID = @UserID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserID", _userID);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        consultations.Add(new Consultation
                        {
                            ConsultationDate = Convert.ToDateTime(reader["ConsultationDate"]),
                            DoctorName = reader["DoctorName"].ToString(),
                            Summary = reader["Summary"].ToString()
                        });
                    }
                    reader.Close();
                    ConsultationHistoryDataGrid.ItemsSource = consultations;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading consultation history: {ex.Message}");
                }
            }
        }

       /* private void ViewFileButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            string filePath = button?.Tag.ToString();

            if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
            {
                System.Diagnostics.Process.Start(filePath);
            }
            else
            {
                MessageBox.Show("File not found.");
            }
        }
*/
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.ChangePage(new PatientDashboard());
            }
        }
    }

    public class PatientFile
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTime UploadDate { get; set; }
    }

    public class Consultation
    {
        public DateTime ConsultationDate { get; set; }
        public string DoctorName { get; set; }
        public string Summary { get; set; }
    }
}