using AutoMapper;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using tester.Data;
using tester.DTOs;
using tester.Models;
using tester.Services.Interfaces;

namespace tester.Services
{
    public class AuthService : IAuthService
    {
        public readonly AppDbContext _context;
        public readonly IMapper _mapper;
        public readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public AuthService(AppDbContext context, IMapper mapper, IConfiguration configuration, IEmailService emailService)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
            _emailService = emailService;
        }

        public async Task<User> Register(CreateUserRequestDTO requestDTO)
        {
            try
            {
                var existingUser = await _context.Users.FirstOrDefaultAsync(u=> u.Username == requestDTO.Username);

                if (existingUser != null) throw new Exception("Username already exists");

                var user = _mapper.Map<User>(requestDTO);
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                user.Active = true;

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return user;

            }
            catch (Exception ex) {

                throw new Exception("An error occcured while registering the user.", ex);
            }
        }

        public async Task<string> Login(LoginRequestDTO userForLogin)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == userForLogin.Username);

                // if (user == null || !BCrypt.Net.BCrypt.Verify(userForLogin.Password, user.Password) || !user.Active) return null;
                if (user == null || !user.Active) return null;


                // Check if the account is locked
                //if (user.)



                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.Username)
                }),
                    Expires = DateTime.UtcNow.AddDays(Convert.ToDouble(_configuration["Jwt:ExpireDays"])),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            } catch (Exception ex) 
            { 
                throw new Exception("An error occurred while logging in.", ex); 
            }
        }

        public async Task<bool> ForgotPassword(ForgotPasswordRequestDTO forgotPasswordRequest)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == forgotPasswordRequest.Email);

                if (user == null) return false;

                // Convert current time to EAT
                var easternAfricaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("E. Africa Standard Time");

                var resetPasswordToken = Guid.NewGuid().ToString();
                var resetPasswordExpires = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow.AddMinutes(Constants.PasswordResetTokenExpiryMinutes), easternAfricaTimeZone);

                var passwordReset = new PasswordReset
                {
                    UserId = user.UserId,
                    ResetPasswordExpires = resetPasswordExpires,
                    ResetPasswordToken = resetPasswordToken
                };

                _context.PasswordResets.Add(passwordReset);

                await _context.SaveChangesAsync();

                // Implement send email
                _emailService.SendPasswordResetEmail(user.Username, resetPasswordToken);

                return true;
            }
            catch (Exception ex) 
            {
                throw new Exception($"An error occurred while processing forgot password request", ex);
            }
        }

        public async Task<bool> ResetPassword(ResetPasswordRequestDTO resetPasswordRequest)
        {
            try
            {

                // Convert current time to EAT
                var easternAfricaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("E. Africa Standard Time");
                var currentEATTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, easternAfricaTimeZone);


                var passwordReset = await _context.PasswordResets
                .Include(pr => pr.User)
                .FirstOrDefaultAsync(pr => pr.ResetPasswordToken == resetPasswordRequest.Token && pr.ResetPasswordExpires > currentEATTime && !pr.Used);

                if (passwordReset == null) return false;


                passwordReset.User.Password = BCrypt.Net.BCrypt.HashPassword(resetPasswordRequest.NewPassword);
                passwordReset.Used = true;

                _context.Users.Update(passwordReset.User);
                _context.PasswordResets.Update(passwordReset);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex) 
            {
                throw new Exception("An error occurred while resetting the password", ex);
            }
        }

    }
}
