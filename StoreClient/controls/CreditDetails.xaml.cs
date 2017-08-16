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

namespace StoreClient.controls
{
    /// <summary>
    /// Interaction logic for CreditDetails.xaml
    /// </summary>
    public partial class CreditDetails : UserControl
    {
        public double Amount
        {
            set
            {
                AmountBlock.Text = string.Format(" ₹ {0} ", value < 0 ? value * -1 : value);
                if (value > 0)
                {
                    DetailBlock.Foreground = Brushes.Red;
                    DetailBlock.Text = "Credited";
                }
                else
                {
                    DetailBlock.Foreground = Brushes.Green;
                    DetailBlock.Text = "Paid";
                }

            }
        }

        public string Date
        {
            set { DateBlock.Text = value; }
        }
        
        public CreditDetails()
        {
            InitializeComponent();
        }
    }
}
