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
    /// Interaction logic for Admin.xaml
    /// </summary>
    public partial class Admin : Page
    {
        public List<DataItem> DataItems { get; set; }

        public Admin()
        {
 
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
