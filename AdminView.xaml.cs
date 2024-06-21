using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Digident
{
    public partial class AdminView : Page
    {
        public List<DataItem> DataItems { get; set; }

        public AdminView()
        {
            InitializeComponent();

            // Initialize data
            DataItems = new List<DataItem>
            {
                new DataItem { FieldName = "First Name", FieldValue = "" },
                new DataItem { FieldName = "Last Name", FieldValue = "" },
                new DataItem { FieldName = "Appointment Date and Time", FieldValue = "" },
                new DataItem { FieldName = "Reason for Visit", FieldValue = "" }
            };

            // Set data context for the ListView
            DataContext = this;
        }
    }

    // Data class for binding
    public class DataItem
    {
        public string FieldName { get; set; }
        public string FieldValue { get; set; }
    }
}
