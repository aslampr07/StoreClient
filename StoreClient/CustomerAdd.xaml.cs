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
using MySql.Data.MySqlClient;
using MySql.Data;

namespace StoreClient
{
    /// <summary>
    /// Interaction logic for CustomerAdd.xaml
    /// </summary>
    public partial class CustomerAdd : Window
    {
        private MySqlConnection conn;
        public CustomerAdd(MySqlConnection co)
        {
            conn = co;
            InitializeComponent();
            string sql = "SELECT `AUTO_INCREMENT`FROM INFORMATION_SCHEMA.TABLES " +
               "WHERE TABLE_SCHEMA = 'Shop' AND TABLE_NAME = 'Customer'";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            object x = cmd.ExecuteScalar();
            IDLabel.Text += string.Format(" {0}", x);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string name = NameBox.Text;
            string phone = PhoneBox.Text;
            string address = AddressBox.Text;
            string sql = string.Format("INSERT INTO Customer(Name, Phone,Address) " +
                "VALUES(\"{0}\",\"{1}\",\"{2}\");", name, phone,address);
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
            SaveButton.IsEnabled = false;
            //Set the done image to checkmark
            DoneImage.Visibility = Visibility.Visible;
        }
    }
}
