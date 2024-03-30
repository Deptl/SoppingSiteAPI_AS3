using System.ComponentModel.DataAnnotations;

namespace SoppingSiteAPI.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }
        public string? CommentText { get; set; }
        public int CUser {  get; set; }
        public int CUserProduct {  get; set; }
        public int Rating { get; set; }
        public string? CImages { get; set;}
    }
}
