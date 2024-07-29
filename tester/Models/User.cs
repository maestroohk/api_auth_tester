using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tester.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public required string FirstName { get; set; }

        [Required]
        public required string LastName { get; set; }
        
        [Required]
        [EmailAddress]
        public required string Username { get; set; }
        
        [Required]
        public required string Password { get; set; }

        [Required]
        public bool Active{ get; set; }

        public int FailedLoginAttempts { get; set; }

        public DateTime? LockoutEnd { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }

    }
}
