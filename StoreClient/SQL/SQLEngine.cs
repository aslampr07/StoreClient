using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace StoreClient.SQL
{
    public class SQLEngine
    {
        private MySqlConnection connection;
        public SQLEngine()
        {
            connection = new MySqlConnection("server=localhost;user=root;database=shop;" +
                                                           "port=3306;password=search4Here.;");
            connection.Open();
        }


        public List<Product> GetProductList()
        {
            string query = "SELECT * FROM Product";
            return GetProductList(query);
        }

        public List<Product> GetProductList(uint SupplierID)
        {
            string query = string.Format("SELECT * FROM Product WHERE SuppID = {0}", SupplierID);
            return GetProductList(query);
        }

        private List<Product> GetProductList(string query)
        {
            List<Product> products = new List<Product>();
            MySqlCommand cm = new MySqlCommand(query, connection);
            using (MySqlDataReader datas = cm.ExecuteReader())
            {
                while (datas.Read())
                {
                    Product product = new Product()
                    {
                        ID = datas.GetUInt32("ID"),
                        Name = datas.GetString("name")
                    };
                    products.Add(product);
                }
            }
            return products;
        }

        /// <summary>
        /// Returns the entire ID and Name colomn of Supplier table.
        /// </summary>
        /// <returns></returns>
        public List<Supplier> GetSupplierList()
        {
            List<Supplier> suppliers = new List<Supplier>();
            string q = "SELECT * FROM Supplier";
            MySqlCommand cm = new MySqlCommand(q, connection);
            using (MySqlDataReader datas = cm.ExecuteReader())
            {
                while (datas.Read())
                {
                    Supplier supplier = new Supplier()
                    {
                        ID = datas.GetUInt32("ID"),
                        Name = datas.GetString("Name")
                    };
                    suppliers.Add(supplier);
                }
            }
            return suppliers;
        }

    }
}
