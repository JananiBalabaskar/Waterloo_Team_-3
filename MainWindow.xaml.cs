using System.Text;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

      
        private void Booknow(object sender, RoutedEventArgs e)
        {

        }

        private void Homebutton(object sender, RoutedEventArgs e)
        {

        }

        private void aboutus(object sender, RoutedEventArgs e)
        {

        }

        private void services(object sender, RoutedEventArgs e)
        {
            ServicesImage1.BringIntoView();
        }

        private void Contactus(object sender, RoutedEventArgs e)
        {

        }

        private void Register(object sender, RoutedEventArgs e)
        {

            Register registerPage = new Register();
            ChangePage(registerPage);

        }

        private void QuestionForm(object sender, RoutedEventArgs e)
        {

        }

        private void Loginbutton(object sender, RoutedEventArgs e)
        {
            
            Login loginPage = new Login();
            ChangePage(loginPage);
          
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
        public void ChangePage(Page page)
        {
            Content = page;
        }
    }
}