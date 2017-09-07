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
using StoreClient.controls;
using System.Diagnostics;

namespace StoreClient.SQL
{
    /// <summary>
    /// Interaction logic for InvoicePrint.xaml
    /// </summary>
    public partial class InvoicePrint : UserControl
    {
        private SQLEngine connection;
        public InvoicePrint(InvoiceSection section, SQLEngine c)
        {
            connection = c;
            InitializeComponent();
            InvoiceIDBlock.Text = "ID: " + section.ID.ToString();
            NameBlock.Text = connection.GetInvoiceOwner(section.ID);
            DateBlock.Text = connection.GetInvoiceDate(section.ID).ToString("dd/MM/yyyy hh:mm tt");

            foreach (InvoiceItemControl item in section.ItemList.Children)
            {
                ItemList.Children.Add(new InvoicePrintItem(
                    serial: item.SerialNumberBlock.Text,
                    name: item.ItemNameBox.Text, 
                    quantity: item.ItemQuantityBox.Text, 
                    amount: item.ItemPriceBox.Text, 
                    total: item.TotalAmountBlock.Text));
            }

            FinalAmountBlock.Text = ((MainWindow)Window.GetWindow(section)).TotalBlock.Text;
        }

        public void Print()
        {
            PrintDialog pd = new PrintDialog();
            pd.PrintVisual(this, "Invoice");
        }
    }
}
