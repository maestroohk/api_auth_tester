using System.ComponentModel.DataAnnotations;

namespace tester.Models
{
    public class RolePermission
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required int RoleId { get; set; }
        public Role Role { get; set; }


        [Required]
        public required int PermissionId { get; set; }
        public Permission Permission { get; set; }
    }
}
