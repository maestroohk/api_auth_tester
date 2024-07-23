using System.ComponentModel.DataAnnotations;

namespace tester.Models
{
    public class PasswordReset
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public string ResetPasswordToken { get; set; }

        [Required]
        public DateTime ResetPasswordExpires { get; set; }

        public bool Used { get; set; }

        public User User { get; set; }
    }
}
