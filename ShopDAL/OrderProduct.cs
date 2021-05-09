namespace ShopDAL
{
    public class OrderProduct
    {
        public int OrderID { get; set; }
        public Order Order { get; set; }
        public int ProductID { get; set; }
        public Product Product { get; set; }
        public int ProductCount { get; set; }

        public override string ToString()
        {
            return  $"{OrderID},{ProductID},{ProductCount}";
        }

    }
}