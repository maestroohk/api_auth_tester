using System.ComponentModel.DataAnnotations;

namespace tester.DTOs.Auth
{
    public class ForgotPasswordRequestDTO
    {
        [Required]
        public string Email { get; set; }
    }
}
