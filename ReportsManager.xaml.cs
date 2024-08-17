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
    /// Interaction logic for ReportsManager.xaml
    /// </summary>
    public partial class ReportsManager : Page
    {
        public ReportsManager()
        {
            InitializeComponent();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow? mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.ChangePage(new ManagerDash());
            }
        }

        private void GenerateReport_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmbReportType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
