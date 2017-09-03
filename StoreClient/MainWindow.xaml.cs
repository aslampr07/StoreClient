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
using StoreClient.controls;

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
            CustomerList.ItemsSource = connection.GetCustomers();
            CustomerList.DisplayMemberPath = "Name";
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

        private void ViewCustomer_Click(object sender, RoutedEventArgs e)
        {
            CustomerWindow customer = new CustomerWindow(connection);
            customer.ShowDialog();
        }

        private void AddInvoice_Click(object sender, RoutedEventArgs e)
        {
            //Test starts here
            TabItem tab = new TabItem
            {
                Header = "POSit",
                BorderThickness = new Thickness(0)
            };
            InvoiceSection invoice = new InvoiceSection(connection);
            tab.Content = invoice;
            InvoiceTab.Items.Add(tab);
            //Test end here.
            InvoiceTab.SelectedIndex = InvoiceTab.Items.Count - 1;
            InvoiceIDBlock.Text = "ID: " + invoice.ID.ToString();

            //The default index for customer list in the invoice section, the content is 'Others'
            CustomerList.SelectedIndex = 12;
        }

        private void InvoiceTab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            InvoiceSection currentTab = ((InvoiceSection)((TabItem)InvoiceTab.SelectedItem).Content);
            uint id = currentTab.ID;
            InvoiceIDBlock.Text = "ID: " + id;
            TotalBlock.Text = "₹ " + connection.GetInvoiceTotal(id).ToString();
            CustomerList.SelectedIndex = currentTab.CustomerListComboIndex;
        }

        private void CustomerList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (InvoiceTab.SelectedIndex != -1)
            {
                InvoiceSection currentTab = ((InvoiceSection)((TabItem)InvoiceTab.SelectedItem).Content);
                uint Invoiceid = currentTab.ID;
                currentTab.CustomerListComboIndex = CustomerList.SelectedIndex;
                int customerID = ((Customer)CustomerList.SelectedItem).ID;
                connection.ChangeInvoiceOwner(customerID: customerID, InvoiceID: Invoiceid);
            }
        }

        private void PrintSaveButton_Click(object sender, RoutedEventArgs e)
        {
            InvoiceSection currentTab = ((InvoiceSection)((TabItem)InvoiceTab.SelectedItem).Content);
            InvoicePrint print = new InvoicePrint(currentTab);
            print.Print();
        }
    }
}
