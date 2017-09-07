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
                if (data.Read())
                {
                    product = new Product()
                    {
                        Name = data["name"].ToString(),
                        ID = ProductID,
                        CompanyPrice = data.GetDouble("companyPrice"),
                        WholesalePrice = data.GetDouble("wholesalePrice"),
                        RetailPrice = data.GetDouble("retailPrice")
                    };
                }
                else
                    return null;
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
            string query = "SELECT * FROM Customer;";
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

        public void SetCredit(int CusID, double Amount)
        {
            string query = string.Format("INSERT INTO CREDIT(cusid, amount, writtentime) values({0}, {1}, NOW())", CusID, Amount);
            MySqlCommand cm = new MySqlCommand(query, connection);
            cm.ExecuteNonQuery();
        }

        /// <summary>
        /// Create a new row in invoice table and return the id of the last row
        /// </summary>
        /// <returns></returns>
        public uint CreateInvoice()
        {
            string query = "INSERT INTO INVOICE(Invoicedate, cusid) VALUES(NOW(), 1013)";
            MySqlCommand cm = new MySqlCommand(query, connection);
            cm.ExecuteNonQuery();

            query = "SELECT MAX(ID) FROM INVOICE";
            cm = new MySqlCommand(query, connection);
            return (uint)cm.ExecuteScalar();
        }

        public uint CreateInvoiceItem(uint InvoiceID, uint ItemID, int Quantity, double Amount)
        {
            string query = string.Format("INSERT INTO InvoiceItem(invoiceid, productid, quantity, amount) VALUES({0}, {1}, {2}, {3})",InvoiceID,ItemID,Quantity,Amount);
            MySqlCommand cm = new MySqlCommand(query, connection);
            cm.ExecuteNonQuery();

            query = "SELECT MAX(ID) FROM InvoiceItem";
            cm = new MySqlCommand(query, connection);
            return (uint)cm.ExecuteScalar();
        }

        public void UpdateInvoiceItem(uint ItemID, int Quantity, double Amount)
        {
            string query = string.Format("UPDATE InvoiceItem SET Quantity = {0}, Amount = {1} WHERE ID = {2}", Quantity, Amount, ItemID);
            MySqlCommand cm = new MySqlCommand(query, connection);
            cm.ExecuteNonQuery();
        }

        public decimal GetInvoiceTotal(uint InvoiceID)
        {
            string query = string.Format("SELECT SUM(Amount*Quantity) FROM InvoiceItem WHERE invoiceID = {0}", InvoiceID);
            MySqlCommand cm = new MySqlCommand(query, connection);
            object x = cm.ExecuteScalar();
            if (x == DBNull.Value)
                return 0;
            return (decimal)cm.ExecuteScalar();
        }

        public void ChangeInvoiceOwner(int customerID, uint InvoiceID)
        {
            string query = string.Format("UPDATE Invoice SET CusID = {0} WHERE ID = {1}", customerID, InvoiceID);
            MySqlCommand cm = new MySqlCommand(query, connection);
            cm.ExecuteNonQuery();
        }

        public string GetInvoiceOwner(uint InvoiceID)
        {
            string query = string.Format("SELECT name FROM customer WHERE" +
                " id = (SELECT cusid FROM invoice WHERE id = {0})", InvoiceID);
            MySqlCommand cm = new MySqlCommand(query, connection);
            string customer = (string)cm.ExecuteScalar();
            return customer;
        }

        public DateTime GetInvoiceDate(uint InvoiceID)
        {
            string query = string.Format("SELECT InvoiceDate FROM Invoice WHERE id = {0}",InvoiceID);
            MySqlCommand cm = new MySqlCommand(query, connection);
            DateTime date = (DateTime)cm.ExecuteScalar();
            return date;
        }

    }
}
