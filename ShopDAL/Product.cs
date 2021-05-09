using System.ComponentModel.DataAnnotations;

namespace ShopDAL
{
    public class Product
    {
        public int ProductID { get; set; }

        [Required]
        [MinLength(2)]
        public string Name { get; set; }

        [Range(1,123456)]
        public int Value { get; set; }

        public override string ToString()
        {
            return $"{ProductID},{Name},{Value}";
        }
    }
}