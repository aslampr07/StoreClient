using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreClient.SQL
{
    public class Product
    {
        public string Name { get; set; }
        public uint ID { get; set; }
        public double CompanyPrice { get; set; }
        public double WholesalePrice { get; set; }
        public double RetailPrice { get; set; }
    }
}
