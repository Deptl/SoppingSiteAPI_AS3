using System.ComponentModel.DataAnnotations;

namespace SoppingSiteAPI.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }
        public int CTProduct { get; set;}
        public int Quantities { get; set;}
        public int CTUser { get; set; }
    }
}
