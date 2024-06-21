using Digident_Group3;
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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Loginbutton(object sender, RoutedEventArgs e)
        {



            // Reset error messages
            UsernameError.Text = "";
            PasswordError.Text = "";

            // Validate input
            if (string.IsNullOrWhiteSpace(UsernameTextBox.Text))
            {
                UsernameError.Text = "Username is required.";
                return;
            }

            if (string.IsNullOrWhiteSpace(PasswordBox.Password))
            {
                PasswordError.Text = "Password is required.";
                return;
            }

            // Get username and password from input fields
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

          
        }

        private void Homebutton(object sender, RoutedEventArgs e)
        {
            MainWindow window1 = new MainWindow();
            window1.Show();
            Window.GetWindow(this)?.Close();
        }

        private void RegisterHyperlink_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.ChangePage(new Register());
            }
        }
    }

}
