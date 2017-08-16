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
using StoreClient.SQL;
using System.Diagnostics;

namespace StoreClient.Windows
{
    /// <summary>
    /// Interaction logic for ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Window
    {
        private SQLEngine connection;

        public ProductPage(SQLEngine c)
        {
            connection = c;
            InitializeComponent();

            //Populating the supplier List
            List<Supplier> suppliers = connection.GetSupplierList();
            foreach (Supplier item in suppliers)
            {
                ListViewItem l = new ListViewItem()
                {
                    Content = item.ID + " - " + item.Name,
                    Tag = item.ID
                };
                SupplierList.Items.Add(l);
            }


            //To populate the Product ListView.
            List<Product> products = connection.GetProductList();
            foreach (Product item in products)
            {
                ListViewItem l = new ListViewItem()
                {
                    Content = item.ID + " - " + item.Name,
                    Tag = item.ID
                };
                ProductList.Items.Add(l);
            }
        }


        private void SupplierList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            //Clear the previos Listview items.
            ProductList.Items.Clear();
            SearchBox.Clear();
            List<Product> products;

            //When 'All' Item is selected.
            if (SupplierList.SelectedIndex == 0)
                products = connection.GetProductList();
            else
            {
                uint suppid = (uint)((ListViewItem)SupplierList.SelectedItem).Tag;
                products = connection.GetProductList(suppid);
            }
            foreach (Product item in products)
            {
                ListViewItem l = new ListViewItem()
                {
                    Content = item.ID + " - " + item.Name,
                    Tag = item.ID
                };
                ProductList.Items.Add(l);
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ProductList.Items.Clear();
            List<Product> products;
            if(SupplierList.SelectedIndex <= 1)
            {
                products = connection.GetProductList(SearchBox.Text);
            }
            else
            {
                uint suppid = (uint)((ListViewItem)SupplierList.SelectedItem).Tag;
                products = connection.GetProductList(suppid,SearchBox.Text);
            }

            foreach (Product item in products)
            {
                ListViewItem l = new ListViewItem()
                {
                    Content = item.ID + " - " + item.Name,
                    Tag = item.ID
                };
                ProductList.Items.Add(l);
            }
        }

        private void ProductList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Product product;
            if(ProductList.SelectedIndex != -1)
            {
                product = connection.GetProduct((uint)((ListViewItem)ProductList.SelectedItem).Tag);
                IDLabel.Text = "Product ID: " + product.ID;
                NameLabel.Text = product.Name;
                PriceLabel.Text = "₹ " + product.CompanyPrice;
                WholesaleLabel.Text = "₹ " + product.WholesalePrice;
                RetailLabel.Text = "₹ " + product.RetailPrice;
            }
        }

        private void EditButtonClick(object sender, RoutedEventArgs e)
        {
            //For setting the Location of the Edit Box Popup.
            if (sender.Equals(NameEditButton))
            {
                EditPopup.PlacementTarget = NameLabel;
            }
            else if(sender.Equals(PriceEditButton))
            {
                EditPopup.PlacementTarget = PriceLabel;
            }
            else if(sender.Equals(WholesaleEditButton))
            {
                EditPopup.PlacementTarget = WholesaleLabel;
            }
            else if(sender.Equals(RetailEditButton))
            {
                EditPopup.PlacementTarget = RetailLabel;
            }
            EditPopup.IsOpen = !EditPopup.IsOpen;
            InputBox.Focus();
            InputBox.Clear();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            EditPopup.IsOpen = false;
            uint id = (uint)((ListViewItem)(ProductList.SelectedItem)).Tag;


            //This is to identify where is the popup corrently opened.
            if (((TextBlock)EditPopup.PlacementTarget).Equals(NameLabel))
            {
                connection.UpdateProduct(SQLEngine.ProductAttributes.Name,id, InputBox.Text);
                NameLabel.Text = InputBox.Text;
            }
            else if (((TextBlock)EditPopup.PlacementTarget).Equals(PriceLabel))
            {
                connection.UpdateProduct(SQLEngine.ProductAttributes.CompanyPrice, id, InputBox.Text);
                PriceLabel.Text = "₹ " + InputBox.Text; 
            }
            else if (((TextBlock)EditPopup.PlacementTarget).Equals(WholesaleLabel))
            {
                connection.UpdateProduct(SQLEngine.ProductAttributes.WholesalePrice, id, InputBox.Text);
                WholesaleLabel.Text = "₹ " + InputBox.Text;
            }
            else if (((TextBlock)EditPopup.PlacementTarget).Equals(RetailLabel))
            {
                connection.UpdateProduct(SQLEngine.ProductAttributes.RetailPrice, id, InputBox.Text);
                RetailLabel.Text = "₹ " + InputBox.Text;
            }
        }
    }
}
