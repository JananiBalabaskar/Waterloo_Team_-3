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
    /// Interaction logic for DoctorLoginPage.xaml
    /// </summary>

   // private const string connectionString = @"Data Source=JANANIDESK\MSSQLSERVER05;Initial Catalog=Digidentdb;Integrated Security=True;TrustServerCertificate=True";
    public partial class DoctorLoginPage : Page
    {
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

        }

        private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            
        }

        private void JobComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }

        private void EmailTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
