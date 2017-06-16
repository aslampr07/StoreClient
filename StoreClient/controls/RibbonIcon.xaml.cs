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

namespace StoreClient
{
    /// <summary>
    /// Interaction logic for RibbonIcon.xaml
    /// </summary>
    public partial class RibbonIcon : UserControl
    {
        public ImageSource Source
        {
            get { return img.Source; }
            set { img.Source = value; }
        }
        public string TextFirst
        {
            get { return first.Text; }
            set { first.Text = value; }
        }
        public string TextSecond
        {
            get { return second.Text; }
            set { second.Text = value; }
        }
        public event RoutedEventHandler Click;

        public RibbonIcon()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Click?.Invoke(this, e);
        }
    }
}
