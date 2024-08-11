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
    /// Interaction logic for Feedbacks.xaml
    /// </summary>
    public partial class Feedbacks : Page
    {
        private const string connectionString = @"Data Source=YOUR_SERVER;Initial Catalog=Digidentdb;Integrated Security=True;TrustServerCertificate=True";
        private List<Feedback> feedbackHistory;

        public Feedbacks()
        {
            InitializeComponent();
            LoadFeedbackHistory();
        }

        private void LoadFeedbackHistory()
        {
            feedbackHistory = new List<Feedback>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT ConsultationDate, Rating, Feedback FROM Feedbacks WHERE UserID = @UserID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserID", UserSession.UserID);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        feedbackHistory.Add(new Feedback
                        {
                            ConsultationDate = Convert.ToDateTime(reader["ConsultationDate"]),
                            Rating = Convert.ToInt32(reader["Rating"]),
                            FeedbackText = reader["Feedback"].ToString()
                        });
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading feedback history: {ex.Message}");
                }
            }

            FeedbackHistoryDataGrid.ItemsSource = feedbackHistory;
        }

        private void SubmitFeedbackButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime? consultationDate = dpConsultationDate.SelectedDate;
            int rating = Convert.ToInt32(((ComboBoxItem)cmbRating.SelectedItem).Content);
            string feedbackText = txtFeedback.Text;

            if (consultationDate.HasValue && !string.IsNullOrEmpty(feedbackText))
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO Feedbacks (UserID, ConsultationDate, Rating, Feedback) VALUES (@UserID, @ConsultationDate, @Rating, @Feedback)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@UserID", UserSession.UserID);
                    command.Parameters.AddWithValue("@ConsultationDate", consultationDate.Value);
                    command.Parameters.AddWithValue("@Rating", rating);
                    command.Parameters.AddWithValue("@Feedback", feedbackText);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        MessageBox.Show("Feedback submitted successfully!");

                        // Reload the feedback history
                        LoadFeedbackHistory();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error submitting feedback: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Please fill in all fields.");
            }
        }

        private void EditFeedbackButton_Click(object sender, RoutedEventArgs e)
        {
            Button editButton = sender as Button;
            Feedback selectedFeedback = editButton.Tag as Feedback;

            if (selectedFeedback != null)
            {
                dpConsultationDate.SelectedDate = selectedFeedback.ConsultationDate;
                cmbRating.SelectedItem = cmbRating.Items.Cast<ComboBoxItem>().FirstOrDefault(item => item.Content.ToString() == selectedFeedback.Rating.ToString());
                txtFeedback.Text = selectedFeedback.FeedbackText;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow? mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.ChangePage(new PatientDashboard());
            }
        }
    }

    public class Feedback
    {
        public DateTime ConsultationDate { get; set; }
        public int Rating { get; set; }
        public string FeedbackText { get; set; }
    }
}