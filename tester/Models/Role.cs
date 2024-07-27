using System.ComponentModel.DataAnnotations;

namespace tester.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }

        [Required]
        [RegularExpression("(^[0-9]{10000,9999999}$)", ErrorMessage = "Please enter a valid number between 10,000 and 9,999,999.")]        
        public int RoleCode { get; set; }

        [Required]
        [MaxLength(100)]
        public required string RoleName { get; set; }

        [Required]
        [MaxLength(100)]
        public required string RoleDescription { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}
