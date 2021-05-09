using System;
using System.Collections.Generic;
using System.Text;

namespace ShopDAL
{
    public class OrderSummery
    {
        public int UserID { get; set; }
        public int OrderID{ get; set; }
        public string Name { get; set; }
        public int Value { get; set; }
    }
}
