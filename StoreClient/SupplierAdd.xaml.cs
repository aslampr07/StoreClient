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
using System.Diagnostics;

namespace StoreClient
{
    /// <summary>
    /// Interaction logic for SupplierAdd.xaml
    /// </summary>
    public partial class SupplierAdd : Window
    {
        private MySqlConnection conn;
        public SupplierAdd(MySqlConnection co)
        {
            //The parameter is moved to a window wide varible;
            conn = co;
            InitializeComponent();
            //For getting the next Autoincrement variable
            string sql = "SELECT `AUTO_INCREMENT`FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Shop' AND TABLE_NAME = 'Supplier'";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            object x = cmd.ExecuteScalar();
            IDLabel.Text += string.Format(" {0}", x);
        }

        /// <summary>
        /// Excuted when the save button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string name = NameBox.Text;
            string place = PlaceBox.Text;
            string phone = PhoneBox.Text;
            string sql = string.Format("INSERT INTO Supplier(Name, Place, Phone) VALUES(\"{0}\",\"{1}\",\"{2}\");", name, place, phone);
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
            SaveButton.IsEnabled = false;
            //Set the done image to checkmark
            DoneImage.Visibility = Visibility.Visible;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            Debug.WriteLine(e.Key);
        }
    }
}
