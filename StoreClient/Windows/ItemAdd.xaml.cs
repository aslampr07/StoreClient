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
using System.Globalization;

namespace StoreClient
{
    /// <summary>
    /// Interaction logic for ItemAdd.xaml
    /// </summary>
    public partial class ItemAdd : Window
    {
        private MySqlConnection conn;

        private class SupplierItem
        {
            public string DisplayString { get; set; }
            public int ID { get; set; }
        }
        public ItemAdd(MySqlConnection c)
        {
            InitializeComponent();
            conn = c;
            //For retreaving the id and name for the supplier dropdown list.
            //Currently databinding is not used to populate the list.
            //Please change the below code to MVVM approach.
            string str = "SELECT id, name FROM Supplier";
            MySqlCommand cmd = new MySqlCommand(str, conn);
            MySqlDataReader vals = cmd.ExecuteReader();
            while (vals.Read())
            {
                //The SupplierItem is classes is added to the combolist
                SupplierItem item = new SupplierItem()
                {
                    DisplayString = vals.GetInt32("id") + " - " + vals.GetString("name"),
                    ID = vals.GetInt32("id")
                };
                supplierlist.Items.Add(item);
            }
            vals.Close();
            //Setting the display property for the supplierlist combobox.
            supplierlist.DisplayMemberPath = "DisplayString";
            //For retreaving the next autoincrement value of ID coloumn from the Product table
            str = "SELECT `AUTO_INCREMENT`FROM INFORMATION_SCHEMA.TABLES " +
                "WHERE TABLE_SCHEMA = 'Shop' AND TABLE_NAME = 'Product'";
            cmd = new MySqlCommand(str, conn);
            object x = cmd.ExecuteScalar();
            IDLabel.Text = string.Format("Item Code: {0}", x);
        }

        //Requires MVVM here.
        private void PriceBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool status = float.TryParse(PriceBox.Text, out float price);
            if (status)
            {
                float wholeSalePrice = (price + (price * 0.1f));
                float retailPrice = (wholeSalePrice + (wholeSalePrice * 0.3f));
                WholesaleBox.Text = wholeSalePrice.ToString();
                RetailBox.Text = retailPrice.ToString();
            }
        }

        //Event on save button, adding everything to the database.
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            //Findig the ID of the selected item on the supplierlist combobox.
            int id = ((SupplierItem)supplierlist.SelectedItem).ID;
            string name = NameBox.Text;
            float price = float.Parse(PriceBox.Text);
            float wholesale = float.Parse(WholesaleBox.Text);
            float retail = float.Parse(RetailBox.Text);
            string str = string.Format("INSERT INTO Product(suppID,name,companyprice,wholesaleprice,retailprice) " +
                "VALUES ({0},'{1}',{2},{3},{4})",id,name,price,wholesale,retail);
            MySqlCommand cmd = new MySqlCommand(str, conn);
            cmd.ExecuteNonQuery();
            ((Button)sender).IsEnabled = false;
            DoneImage.Visibility = Visibility.Visible;
        }
        
        //Caution Dirty code below.
        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            string str = "SELECT `AUTO_INCREMENT`FROM INFORMATION_SCHEMA.TABLES " +
                "WHERE TABLE_SCHEMA = 'Shop' AND TABLE_NAME = 'Product'";
            MySqlCommand  cmd = new MySqlCommand(str, conn);
            object x = cmd.ExecuteScalar();
            IDLabel.Text = string.Format("Item Code: {0}", x);
            NameBox.Clear();
            PriceBox.Clear();
            WholesaleBox.Clear();
            RetailBox.Clear();
            SaveButton.IsEnabled = true;
            DoneImage.Visibility = Visibility.Hidden;
        }
    }
}
