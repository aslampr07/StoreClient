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
using System.Windows.Shapes;
using StoreClient.SQL;
using System.Diagnostics;
using StoreClient.controls;
using System.Globalization;

namespace StoreClient.Windows
{
    /// <summary>
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {
        private SQLEngine connection;
        public List<Customer> ListCustomer { get; set; } = new List<Customer>();
        public CustomerWindow(SQLEngine conn)
        {
            connection = conn;
            InitializeComponent();
            ListCustomer = connection.GetCustomers();
            CustomerList.ItemsSource = ListCustomer;
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ListCustomer = connection.GetCustomers(SearchBox.Text); 
            CustomerList.ItemsSource = null;
            CustomerList.ItemsSource = ListCustomer;
        }

        private int SelectedID;
        private void CustomerList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CreditList.Children.Clear();
            if (CustomerList.SelectedIndex != -1)
            {
                Customer c = ((Customer)CustomerList.SelectedItem);
                IDBlock.Text = "ID: " + c.ID.ToString();
                SelectedID = c.ID;
                NameBlock.Text = c.Name;
                PhoneBlock.Text = "Phone: " + c.Phone;
                AddressBlock.Text = c.Address;
                //Get the list of the credit of a customer
                RefreshCreditList();
            }
        }

        private void RefreshCreditList()
        {
            CreditList.Children.Clear();
            List<Credit> credits = connection.GetCreditList(SelectedID);
            BalanceBlock.Text = "Balance ₹ " + credits.Sum(item => item.Amount).ToString();
            foreach (Credit item in credits)
            {
                CreditDetails details = new CreditDetails
                {
                    Date = item.WrittenTime.ToString("dd/MM/yyyy hh:mm tt"),
                    Amount = item.Amount
                };
                CreditList.Children.Insert(0, details);
            }
        }

        private bool transactionButton;
        private void TransactionButton_click(object sender, RoutedEventArgs e)
        {
            TransactionPopup.IsOpen = true;
            TransactionInputBox.Text = "";
            //Executes when paybuttons when is pressed.
            if (sender.Equals(PayButton))
            {
                transactionButton = true;
            }
            //Executes when creditbutton is pressed.
            else if(sender.Equals(CreditButton))
            {
                transactionButton = false;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            double amount;
            if(transactionButton)
            {
                amount = -double.Parse(TransactionInputBox.Text);
            }
            else
            {
                amount = double.Parse(TransactionInputBox.Text);
            }
            connection.SetCredit(SelectedID, amount);
            TransactionPopup.IsOpen = false;
            RefreshCreditList();
        }
    }
}
