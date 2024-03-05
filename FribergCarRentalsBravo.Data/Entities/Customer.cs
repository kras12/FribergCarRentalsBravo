using System.ComponentModel.DataAnnotations;

namespace FribergCarRentalsBravo.DataAccess.Entities
{
    public class Customer
    {
        public int CustomerId { get; set; }

        public string FirstName { get; set; } = "";

        public string LastName { get; set; } = "";

        public string Email { get; set; } = "";

        [Required]
        public string? Password { get; set; }
    }
}
