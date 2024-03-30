using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SoppingSiteAPI.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? Image { get; set; }

        public string? Pricing { get; set; }

        public string? ShippingCost { get; set; }
    }
}
