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
    /// Interaction logic for PatientDashboard.xaml
    /// </summary>
    public partial class PatientDashboard : Page
    {
        public PatientDashboard()
        {
            InitializeComponent();
        }


        private void BookAppointments(object sender, RoutedEventArgs e)
        {
            MainWindow? mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.ChangePage(new Book_Appointment());
            }
        }

        private void Appointments(object sender, RoutedEventArgs e)
        {

        }

        private void Reports(object sender, RoutedEventArgs e)
        {

        }

        private void Feedbacks(object sender, RoutedEventArgs e)
        {

        }

        private void PatientProfile(object sender, RoutedEventArgs e)
        {

        }

        private void Settings(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }
    }
}
