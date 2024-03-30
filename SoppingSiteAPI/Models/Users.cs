using System.ComponentModel.DataAnnotations;

namespace SoppingSiteAPI.Models
{
    public class Users
    {
        [Key]
        public int UserId { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Username { get; set; }
        public int? History { get; set;}
        public string? Address { get; set;
        }
    }
}
