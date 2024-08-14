using Digident_Group3;
using Digident_Group3.Interfaces;
using Digident_Group3.Services;
using Microsoft.Data.SqlClient;
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
using Digident_Group3.Interfaces;
using Digident_Group3.Services;
using System.Configuration;

namespace Digident_Group3
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {

        private readonly IDatabaseService _databaseService;
        private readonly IMessageBoxService _messageBoxService;
        public Login(IDatabaseService databaseService, IMessageBoxService messageBoxService)
        {
            InitializeComponent();

           
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MyDbConnectionString"].ConnectionString;
            
            _databaseService = databaseService; 
            _messageBoxService = messageBoxService;
        }
        public Login()
    : this(new DatabaseService(ConfigurationManager.ConnectionStrings["MyDbConnectionString"].ConnectionString), new MessageBoxService())
        {
        }


        private void Homebutton(object sender, RoutedEventArgs e)
        {
            MainWindow window1 = new MainWindow();
            window1.Show();
            Window.GetWindow(this)?.Close();
        }

       
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow? mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                string connectionString = @"Data Source=JANANIDESK\MSSQLSERVER05;Initial Catalog=Digidentdb;Integrated Security=True;TrustServerCertificate=True";
                
                var databaseService = new DatabaseService(connectionString);

                mainWindow.ChangePage(new PatientLoginPage(databaseService, _messageBoxService));
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
            MainWindow? mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.ChangePage(new DoctorLoginPage());
            }

        }



       
    }
}
