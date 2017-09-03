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
using System.Diagnostics;
using StoreClient.SQL;


namespace StoreClient.controls
{
    /// <summary>
    /// Interaction logic for InvoiceSection.xaml
    /// </summary>
    public partial class InvoiceSection : UserControl
    {

        public uint ID { get; set; }
        public int CustomerListComboIndex { get; set; }

        private SQLEngine connection;
        public InvoiceSection(SQLEngine c)
        {
            connection = c;
            InitializeComponent();
            ID = connection.CreateInvoice();
        }

        private void IDInput_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                Product item = connection.GetProduct(uint.Parse(IDInput.Text));
                IDInput.Clear();
                InvoiceItemControl invoiceitem = new InvoiceItemControl(ItemList.Children.Count + 1,item,ID,connection);
                ItemList.Children.Add(invoiceitem);
            }
        }
    }
}
