using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShopDAL
{
    public class User
    {
        public int UserID { get; set; }

        [MinLength(2)]
        public string Name { get; set; }

        [MinLength(6)]
        [Required] 
        public string Email { get; set; }

        [MinLength(9)]
        [MaxLength(16)]
        public string PhoneNumber { get; set; }
        public List<Order> Orders{ get; set; }
        public override string ToString()
        {
            return $"{UserID},{Name},{Email},{PhoneNumber}";
        }
    }
}