using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreClient.SQL
{
    public class Credit
    {
        public uint ID { get; set; }
        public uint CusID { get; set; }
        public DateTime WrittenTime { get; set; }
        public double Amount { get; set; }
    }
}
