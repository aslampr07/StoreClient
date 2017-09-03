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

namespace StoreClient.SQL
{
    /// <summary>
    /// Interaction logic for InvoicePrint.xaml
    /// </summary>
    public partial class InvoicePrint : UserControl
    {
        public InvoicePrint(InvoiceSection section)
        {
            InitializeComponent();
        }

        public void Print()
        {
            PrintDialog pd = new PrintDialog();
            pd.PrintVisual(this, "Invoice");
        }
    }
}
