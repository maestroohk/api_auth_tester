using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using tester.DTOs;
using tester.Services.Interfaces;

namespace tester.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(CreateUserRequestDTO userForCreation)
        {
            try
            {
                var user = await _authService.Register(userForCreation);

                return Ok(new { user.UserId, user.Username });

            }
            catch (Exception ex) {

                return StatusCode(500, new { message = ex.Message});
            
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO userForLogin)
        {
            try
            {
                var token = await _authService.Login(userForLogin);

                if (token == null) return Unauthorized();

                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequestDTO forgotPasswordRequest)
        {
            try
            {
                var result = await _authService.ForgotPassword(forgotPasswordRequest);

                if (!result) return BadRequest("User not found.");

                return Ok("Password reset token sent.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequestDTO resetPasswordRequest)
        {
            try
            {
                var result = await _authService.ResetPassword(resetPasswordRequest);

                if (!result) return BadRequest("Invalid or expired token");

                return Ok("Password has been reset");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            };
        }
    }
}
