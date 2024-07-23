using System.ComponentModel.DataAnnotations;

namespace tester.DTOs
{
    public class ForgotPasswordRequestDTO
    {
        [Required]
        public string Email { get; set; }
    }
}
