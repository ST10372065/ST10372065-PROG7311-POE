using System.ComponentModel.DataAnnotations;

namespace ST10372065_PROG7311.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; } // PRIMARY KEY
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        [Required]
        [RegularExpression("^(Farmer|Employee)$", ErrorMessage = "Role must be either 'Farmer' or 'Employee'.")]
        public string Role { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();

    }
}
