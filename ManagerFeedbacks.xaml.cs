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

namespace Digident_Group3
{
    /// <summary>
    /// Interaction logic for ManagerFeedbacks.xaml
    /// </summary>
    public partial class ManagerFeedbacks : Page
    {
        public ManagerFeedbacks()
        {
            InitializeComponent();
            LoadFeedbacks();
        }

        private void LoadFeedbacks()
        {
            // Sample feedbacks - replace with actual data loading
            var feedbacks = new List<Feedback>
            {
                new Feedback { FeedbackId = 1, CustomerName = "John Doe", Date = "2024-08-16", Rating = 5, Status = "Pending", FeedbackContent = "Great service!" },
                new Feedback { FeedbackId = 2, CustomerName = "Jane Smith", Date = "2024-08-15", Rating = 4, Status = "Responded", FeedbackContent = "Very good, but could be improved." }
            };

            FeedbackDataGrid.ItemsSource = feedbacks;
        }

        private void CustomerNameTextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var textBlock = sender as TextBlock;
            var feedback = textBlock?.DataContext as Feedback;

            if (feedback != null)
            {
                // Populate the popup with selected feedback details
                CustomerNameTextBox.Text = feedback.CustomerName;
                DateTextBox.Text = feedback.Date;
                RatingTextBox.Text = feedback.Rating.ToString();
                FeedbackTextBox.Text = feedback.FeedbackContent;
                ResponseTextBox.Text = ""; // Reset response field

                // Open the popup
                FeedbackPopup.IsOpen = true;
            }
        }

        private void SaveResponseButton_Click(object sender, RoutedEventArgs e)
        {
            // Handle saving the response (this should be replaced with actual save logic)
            MessageBox.Show("Response saved successfully.");

            // Close the popup after saving
            FeedbackPopup.IsOpen = false;
        }

        private void ClosePopupButton_Click(object sender, RoutedEventArgs e)
        {
            // Close the popup without saving
            FeedbackPopup.IsOpen = false;
        }

        private void FeedbackDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Optional: Add logic for handling selection change if needed
        }
    

    public class Feedback
    {
        public int FeedbackId { get; set; }
        public string CustomerName { get; set; }
        public string Date { get; set; }
        public int Rating { get; set; }
        public string Status { get; set; }
        public string FeedbackContent { get; set; }
    }

    private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow? mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.ChangePage(new ManagerDash());
            }
        }

        private void SendFeedback_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
