using AutoMapper;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using tester.Data;
using tester.DTOs;
using tester.Helpers;
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
            
            var existingUser = await _context.Users.FirstOrDefaultAsync(u=> u.Username == requestDTO.Username);

            if (existingUser != null) throw new Exception(Constants.UsernameAlreadyExistsMessage);

            var user = _mapper.Map<User>(requestDTO);
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            user.Active = Constants.DefaultUserActiveStatus;

            try
            {
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
            
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == userForLogin.Username);

            if (user == null || !user.Active) 
                throw new Exception(Constants.InvalidUsernameOrPasswordMessage);

            var currentEATTime = DateTimeHelper.GetCurrentEATTime();

            // Check if the account is locked
            if (user.LockoutEnd.HasValue && user.LockoutEnd.Value > currentEATTime)
                throw new Exception(Constants.AccountLockedMessage);

            //Verify the Password
            if (!BCrypt.Net.BCrypt.Verify(userForLogin.Password, user.Password))
            {
                user.FailedLoginAttempts++;

                //Lock the account if max attempts are reached
                if (user.FailedLoginAttempts >= Constants.MaxLogMaxFailedLoginAttemptsinAttempts)
                {
                    user.LockoutEnd = currentEATTime.AddMinutes(Constants.AccountLockoutDurationInMinutes);
                    user.FailedLoginAttempts = 0;
                }

                await _context.SaveChangesAsync();
                throw new Exception(Constants.InvalidUsernameOrPasswordMessage);
            }

            // Reset failed login attempts on successful login
            user.FailedLoginAttempts = 0;
            user.LockoutEnd = null;

            await _context.SaveChangesAsync();

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

            try
            {
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex) { 
                throw new Exception(Constants.LoginErrorMessage, ex);
            }           
        }

        public async Task<bool> ForgotPassword(ForgotPasswordRequestDTO forgotPasswordRequest)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == forgotPasswordRequest.Email);

            if (user == null) throw new Exception(Constants.InvalidUsernameOrPasswordMessage);

            var resetPasswordToken = Guid.NewGuid().ToString();
            var resetPasswordExpires = DateTimeHelper.GetCurrentEATTime().AddMinutes(Constants.PasswordResetTokenExpiryMinutes);

            var passwordReset = new PasswordReset
            {
                UserId = user.UserId,
                ResetPasswordExpires = resetPasswordExpires,
                ResetPasswordToken = resetPasswordToken
            };

            try
            {
                _context.PasswordResets.Add(passwordReset);
                await _context.SaveChangesAsync();
                _emailService.SendPasswordResetEmail(user.Username, resetPasswordToken);
                return true;
            }
            catch (Exception ex)
            { 
                throw new Exception(Constants.ForgotPasswordErrorMessage, ex); 
            }            
        }

        public async Task<bool> ResetPassword(ResetPasswordRequestDTO resetPasswordRequest)
        {
            var currentEATTime = DateTimeHelper.GetCurrentEATTime();

            var passwordReset = await _context.PasswordResets
            .Include(pr => pr.User)
            .FirstOrDefaultAsync(pr => pr.ResetPasswordToken == resetPasswordRequest.Token && pr.ResetPasswordExpires > currentEATTime && !pr.Used);

            if (passwordReset == null) throw new Exception(Constants.InvalidUsernameOrPasswordMessage);


            passwordReset.User.Password = BCrypt.Net.BCrypt.HashPassword(resetPasswordRequest.NewPassword);
            passwordReset.Used = true;

            try 
            {            
                _context.Users.Update(passwordReset.User);
                _context.PasswordResets.Update(passwordReset);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(Constants.ResetPasswordErrorMessage, ex);
            }
        }

    }
}
