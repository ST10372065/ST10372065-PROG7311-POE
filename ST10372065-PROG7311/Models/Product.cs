using System.ComponentModel.DataAnnotations;

namespace ST10372065_PROG7311.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Category { get; set; }
        public DateOnly Date { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
