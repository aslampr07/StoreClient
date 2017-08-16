using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace StoreClient.SQL
{
    public partial class SQLEngine
    {
        private MySqlConnection connection;
        public SQLEngine()
        {
            connection = new MySqlConnection("server=localhost;user=root;database=shop;" +
                                                           "port=3306;password=search4Here.;");
            connection.Open();
        }


        /// <summary>
        /// Return the name and id of the entire Product table
        /// </summary>
        /// <returns></returns>
        public List<Product> GetProductList()
        {
            string query = "SELECT id,name FROM Product";
            return GetProducts(query);
        }

        /// <summary>
        /// Return the name and if of the entire product table with a particular supplier ID
        /// </summary>
        /// <param name="SupplierID">Supplier ID of the prouduct</param>
        /// <returns></returns>
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

        /// <summary>
        /// Used to return the details of a product.
        /// </summary>
        /// <param name="ProductID">Product ID of the item.</param>
        /// <returns></returns>
        public Product GetProduct(uint ProductID)
        {
            string q = string.Format("SELECT * FROM Product WHERE id = {0}", ProductID);
            Product product;
            MySqlCommand cm = new MySqlCommand(q, connection);
            using (MySqlDataReader data = cm.ExecuteReader())
            {
                data.Read();
                product = new Product()
                {
                    Name = data["name"].ToString(),
                    ID = ProductID,
                    CompanyPrice = data.GetDouble("companyPrice"),
                    WholesalePrice = data.GetDouble("wholesalePrice"),
                    RetailPrice = data.GetDouble("retailPrice")
                };
            }
            return product;
        }

        public void UpdateProduct(ProductAttributes productAttribute, uint ProductID,string Updates)
        {
            if(productAttribute == ProductAttributes.Name)
            {
                string query = string.Format("UPDATE Product SET Name = '{0}' WHERE ID = {1}", Updates, ProductID);
                MySqlCommand cm = new MySqlCommand(query, connection);
                cm.ExecuteNonQuery();
            }

            else if (productAttribute == ProductAttributes.CompanyPrice)
            {
                string query = string.Format("UPDATE Product SET CompanyPrice = {0} WHERE ID = {1}", Updates, ProductID);
                MySqlCommand cm = new MySqlCommand(query, connection);
                cm.ExecuteNonQuery();
            }
            else if (productAttribute == ProductAttributes.WholesalePrice)
            {
                string query = string.Format("UPDATE Product SET WholesalePrice = {0} WHERE ID = {1}", Updates, ProductID);
                MySqlCommand cm = new MySqlCommand(query, connection);
                cm.ExecuteNonQuery();
            }
            else if (productAttribute == ProductAttributes.RetailPrice)
            {
                string query = string.Format("UPDATE Product SET RetailPrice = {0} WHERE ID = {1}", Updates, ProductID);
                MySqlCommand cm = new MySqlCommand(query, connection);
                cm.ExecuteNonQuery();
            }


        }

        public List<Customer> GetCustomers()
        {
            string query = "SELECT * FROM Customer";
            return getCustomersList(query);
        }

        public List<Customer> GetCustomers(string SearchQuery)
        {
            string query = string.Format("SELECT * FROM Customer WHERE name LIKE '%{0}%' OR id LIKE '%{0}%'", SearchQuery);
            return getCustomersList(query);
        }
        private List<Customer> getCustomersList(string query)
        {
            List<Customer> CustomerList = new List<Customer>(); 
            MySqlCommand cm = new MySqlCommand(query, connection);
            using (MySqlDataReader data = cm.ExecuteReader())
            {
                while (data.Read())
                {
                    Customer cus = new Customer()
                    {
                        Name = data.GetString("name"),
                        ID = data.GetInt32("ID"),
                        Address = data.GetString("Address"),
                        Phone = data.GetString("Phone")
                    };
                    CustomerList.Add(cus);
                }
            }
            return CustomerList;
        }

        public List<Credit> GetCreditList(int CustomerID)
        {
            string query = string.Format("SELECT * FROM CREDIT WHERE CusID = {0}", CustomerID);
            List<Credit> CreditList = new List<Credit>();
            MySqlCommand cm = new MySqlCommand(query,connection);
            using (MySqlDataReader data = cm.ExecuteReader())
            {
                while(data.Read())
                {
                    Credit credit = new Credit()
                    {
                        ID = (uint)data.GetInt32("ID"),
                        Amount = data.GetDouble("Amount"),
                        CusID = (uint)data.GetInt32("cusid"),
                        WrittenTime = data.GetDateTime("Writtentime")
                    };
                    CreditList.Add(credit);
                }
            }
            return CreditList;
        }

    }
}
