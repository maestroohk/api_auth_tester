using tester.DTOs.Auth;
using tester.Models;

namespace tester.Services.Interfaces
{
    public interface IAuthService
    {
        Task<User> Register(CreateUserRequestDTO requestDTO);

        Task<string> Login(LoginRequestDTO userForLogin);

        Task<bool> ForgotPassword(ForgotPasswordRequestDTO forgotPasswordRequest);

        Task<bool> ResetPassword(ResetPasswordRequestDTO resetPasswordRequest);
    }
}
