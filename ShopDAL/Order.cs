using System;
using System.Collections.Generic;

namespace ShopDAL
{
    public class Order
    {
        public int OrderID { get; set; }
        public DateTime Created { get; set; }
        public int UserID { get; set; }
        public User User { get; set; }
        public List<OrderProduct> OrderProducts { get; set; }
        public override string ToString()
        {
            return $"{OrderID},{UserID}";
        }
    }
}