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
            string query = "SELECT id,name FROM Product";
            return GetProducts(query);
        }

        public List<Product> GetProductList(uint SupplierID)
        {
            string query = string.Format("SELECT id,name FROM Product WHERE SuppID = {0}", SupplierID);
            return GetProducts(query);
        }

        public List<Product> GetProductList(string searchQuery)
        {
            string query = string.Format("SELECT id,name FROM Product WHERE name LIKE '%{0}%' OR id LIKE '%{0}%'",searchQuery);
            return GetProducts(query);
        }

        public List<Product> GetProductList(uint SupplierID, string searchQuery)
        {
            string query = string.Format("SELECT id,name FROM Product WHERE suppId = {0} AND (name LIKE '%{1}%' OR id LIKE '%{1}%')",SupplierID, searchQuery);
            return GetProducts(query);
        }

        private List<Product> GetProducts(string query)
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
