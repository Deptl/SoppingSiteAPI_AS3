using System.ComponentModel.DataAnnotations;

namespace SoppingSiteAPI.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        public int Ros { get; set; }
    }
}
