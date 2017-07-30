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
using System.ComponentModel;
using StoreClient.Windows;
using StoreClient.SQL;

namespace StoreClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MySqlConnection conn;
        private SQLEngine connection;

        public MainWindow()
        {
            InitializeComponent();
            connection = new SQLEngine();
            try
            {
                conn = new MySqlConnection("server=localhost;user=root;database=shop;" +
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

        //This method occur when the user presses the X button on the window.
        //Displays a message box
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;

            MessageBoxResult result = MessageBox.Show("Do you want to close this application?", 
                "Confirm Exit", 
                MessageBoxButton.OKCancel, MessageBoxImage.Question,MessageBoxResult.No);

            if (result == MessageBoxResult.OK)
                e.Cancel = false;
        }

        private void AddSupplier_Click(object sender, RoutedEventArgs e)
        {
            SupplierAdd newCust = new SupplierAdd(conn);
            newCust.ShowDialog();
        }

        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            ItemAdd newCust = new ItemAdd(conn);
            newCust.ShowDialog();
        }

        private void AddCustomer_Click(object sender, RoutedEventArgs e)
        {
            CustomerAdd newCust = new CustomerAdd(conn);
            newCust.ShowDialog();
        }

        private void FindProductButton_Click(object sender, RoutedEventArgs e)
        {
            ProductPage product = new ProductPage(connection);
            product.ShowDialog();
        }
    }
}
