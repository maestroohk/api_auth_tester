using System.ComponentModel.DataAnnotations;

namespace tester.Models
{
    public class Permission
    {
        [Key]
        public int PermissionId { get; set; }

        [Required]
        [MaxLength(100)]
        public required string PermissionName { get; set; }

        [Required]
        [MaxLength(255)]
        public required string PermissionDescription { get; set; }
    }
}
