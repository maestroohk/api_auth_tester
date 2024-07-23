namespace tester.DTOs.Auth
{
    public class ResetPasswordRequestDTO
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}
