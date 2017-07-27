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

namespace StoreClient.Windows
{
    /// <summary>
    /// Interaction logic for ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Window
    {
        private class SupplierItem
        {
            public string DisplayString { get; set; }
            public int ID { get; set; }
        }
        private MySqlConnection conn;
        public ProductPage(MySqlConnection c)
        {
            conn = c;
            InitializeComponent();
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
                SupplierList.Items.Add(item);
            }
            vals.Close();
            SupplierList.DisplayMemberPath = "DisplayString";
        }
    }
}
