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
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace StoreClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            try
            {

                MySqlConnection conn = new MySqlConnection("server=localhost;user=root;database=shop;" +
                                                            "port=3306;password=search4Here.;");
                conn.Open();
                BottomStatusBar.Background = new SolidColorBrush(Color.FromRgb(0, 122, 204));
                StatusBarConnection.Text = "Connected to Database";
            }
            catch (Exception e)
            {
                //Connection Excpetion is writter Here.
                Debug.WriteLine(e.Message);
            }
        }

        private void RibbonIcon_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Click Worked like charm");
        }
    }
}
