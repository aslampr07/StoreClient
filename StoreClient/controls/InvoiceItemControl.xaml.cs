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
using System.Windows.Navigation;
using System.Windows.Shapes;
using StoreClient.SQL;
using System.Diagnostics;

namespace StoreClient.controls
{
    /// <summary>
    /// Interaction logic for InvoiceItemControl.xaml
    /// </summary>
    public partial class InvoiceItemControl : UserControl
    {
        private uint invoiceID;
        private SQLEngine conn;
        private uint productID;
        private int quantity;
        private bool itemSet;
        public uint ID { get; set; }
            
        public InvoiceItemControl(int SerialNumber,Product product, uint IDInvoice,SQLEngine c)
        {
            invoiceID = IDInvoice;
            conn = c;
            productID = product.ID;
            itemSet = false;

            InitializeComponent();

            ItemNameBox.Text = product.Name;
            ItemPriceBox.Text = product.WholesalePrice.ToString();
            SerialNumberBlock.Text = SerialNumber.ToString();
        }

        private void ItemQuantityBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                UpdateQuantity();
            }
        }

        private void ItemQuantityBox_LostFocus(object sender, RoutedEventArgs e)
        {
            UpdateQuantity();
        }

        

        private void ItemPriceBox_LostFocus(object sender, RoutedEventArgs e)
        {
            UpdateQuantity();
        }

        
        private void ItemPriceBox_KeyDown(object sender, KeyEventArgs e)
        {
            UpdateQuantity();
        }

        private void UpdateQuantity()
        {
            quantity = int.Parse(ItemQuantityBox.Text);
            double amount = double.Parse(ItemPriceBox.Text);
            TotalAmountBlock.Text = (quantity * amount).ToString();
            if (!itemSet)
            {
                ID = conn.CreateInvoiceItem(invoiceID, productID, quantity, amount);
                itemSet = true;
            }
            else
                conn.UpdateInvoiceItem(ID, quantity, amount);
            decimal total = conn.GetInvoiceTotal(InvoiceID: invoiceID);
            //Setting the total value of the invoice to the mainwindow TotalBlock Textblock control.
            ((MainWindow)(Window.GetWindow(this))).TotalBlock.Text = "₹ " + total.ToString();
        }

    }
}
