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
    /// Interaction logic for Consultationn.xaml
    /// </summary>
    public partial class Consultationn : Page
    {
        public Consultationn()
        {
            InitializeComponent();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            
                MainWindow? mainWindow = Window.GetWindow(this) as MainWindow;
                if (mainWindow != null)
                {
                    mainWindow.ChangePage(new DentistDashboard());
                }
            
        }

        private void UploadProcedure(object sender, RoutedEventArgs e)
        {

        }

        private void GoToConsultation(object sender, RoutedEventArgs e)
        {

        }
    }
}
