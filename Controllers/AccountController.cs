using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using api.Dtos.Account;
using api.Interfaces;
using api.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace api.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());

            if (user == null) return Unauthorized("Invalid Username");

            if (string.IsNullOrEmpty(user.Email))
            {
                return Unauthorized("Email alanı boş olamaz.");
            }

            if (!user.EmailConfirmed)
            {
                return Unauthorized("Please confirm your email address before logging in.");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded)
            {
                return Unauthorized("Username not found and/or password incorrect");
            }

            // Kullanıcının TwoFactorEnabled olup olmadığını kontrol et
            if (user.TwoFactorEnabled)
            {
                // Kullanıcıdan 2FA kodu talep et ve doğrula
                var verificationCode = new Random().Next(100000, 999999).ToString();
                user.VerificationCode = verificationCode;
                await _userManager.UpdateAsync(user);

                var subject = "Your 2FA Verification Code";
                var message = $"Your 2FA verification code is: {verificationCode}";

                await SendEmailAsync(user.Email, subject, message);

                // Kullanıcıya 2FA kodu girilmesini talep et
                return Ok(new { TwoFactorRequired = true });
            }

            return Ok(
                new NewUserDto
                {
                    Username = user.UserName,
                    Email = user.Email,
                    Token = _tokenService.CreateToken(user)
                }
            );
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // E-posta kontrolü
            var emailExists = await _userManager.Users.AnyAsync(u => u.Email == registerDto.Email);
            if (emailExists)
            {
                return BadRequest("Bu e-posta adresi zaten kullanılıyor.");
            }

            var appUser = new AppUser
            {
                UserName = registerDto.Username,
                Email = registerDto.Email,
                ActivationCode = Guid.NewGuid().ToString(), // Aktivasyon kodu oluştur
                EmailConfirmed = false
            };

            var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password);

            if (createdUser.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                if (roleResult.Succeeded)
                {
                    // Aktivasyon maili gönderme işlemi
                    var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account", new { userId = appUser.Id, code = appUser.ActivationCode.ToString() }, Request.Scheme);
                    await SendEmailAsync(appUser.Email, "Email Verification", $"Please click on the following link to verify your email address: {confirmationLink}");

                    return Ok(new NewUserDto
                    {
                        Username = appUser.UserName,
                        Email = appUser.Email,
                        Token = _tokenService.CreateToken(appUser)
                    });
                }
                else
                {
                    return StatusCode(500, roleResult.Errors);
                }
            }
            else
            {
                return StatusCode(500, createdUser.Errors);
            }
        }

        [HttpPost("enable-two-factor")]
        public async Task<IActionResult> EnableTwoFactor(string username)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == username.ToLower());

            if (user == null) return NotFound("User not found");

            user.TwoFactorEnabled = true;
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return StatusCode(500, result.Errors);
            }

            return Ok("Two-Factor Authentication has been successfully enabled.");
        }

        [HttpPost("disable-two-factor")]
        public async Task<IActionResult> DisableTwoFactor(string username)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == username.ToLower());

            if (user == null) return NotFound("User not found");

            user.TwoFactorEnabled = false;
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return StatusCode(500, result.Errors);
            }

            return Ok("Two-Factor Authentication has been successfully enabled.");
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return BadRequest("Invalid request");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            if (user.ActivationCode != code)
            {
                return BadRequest("Invalid activation code");
            }

            user.EmailConfirmed = true;
            user.ActivationCode = ""; // Kod kullanımını sıfırla
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return StatusCode(500, result.Errors);
            }

            return Ok("Email confirmed successfully");
        }

        [HttpPost("verify-two-factor")]
        public async Task<IActionResult> VerifyTwoFactor([FromBody] VerifyTwoFactorDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == dto.Username.ToLower());

            if (user == null)
            {
                return NotFound("User not found");
            }

            if (user.VerificationCode != dto.Code)
            {
                return BadRequest("Invalid verification code");
            }

            user.VerificationCode = ""; // Kodu sıfırla
            await _userManager.UpdateAsync(user);

            return Ok(
                new NewUserDto
                {
                    Username = user.UserName,
                    Email = user.Email,
                    Token = _tokenService.CreateToken(user)
                }
            );
        }
        [HttpGet("user/{username}")]
        [Authorize]
        public async Task<IActionResult> GetUser(string username)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == username.ToLower());
            if (user == null) return NotFound("User not found");

            return Ok(new
            {
                user.UserName,
                user.Email,
                user.FirstName,
                user.LastName,
                user.Country
            });
        }

        [HttpPost("update-user-profile")]
        [Authorize]
        public async Task<IActionResult> UpdateUserProfile([FromForm] UpdateUserDto updateUserProfileDto)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == updateUserProfileDto.Username.ToLower());
            if (user == null) return Unauthorized("User not found");

            // Kullanıcı bilgilerini güncelle
            user.FirstName = updateUserProfileDto.FirstName;
            user.LastName = updateUserProfileDto.LastName;
            user.Country = updateUserProfileDto.Country;

            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                return StatusCode(500, updateResult.Errors);
            }

            return Ok(new
            {
                user.UserName,
                user.Email,
                user.FirstName,
                user.LastName,
                user.Country
            });
        }

        [HttpPost("report-and-contact")]
        public async Task<IActionResult> ReportAndContact([FromBody] ReportAndContactDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var subject = "Report & Contact Us";
            var message = $"First Name: {dto.FirstName}\n    Last Name: {dto.LastName}\n    Message: {dto.Message}";

            try
            {
                await SendEmailAsync("iletisimwatchhub@gmail.com", subject, message);
                return Ok("Your message has been sent successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while sending the email: {ex.Message}");
            }
        }


        [HttpDelete("delete/{username}")]
        public async Task<IActionResult> DeleteUser(string username)
        {
            var user = await _userManager.FindByNameAsync(username.ToLower());
            if (user == null) return NotFound("User not found");

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return Ok("User deleted successfully");
            }
            else
            {
                return StatusCode(500, result.Errors);
            }
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByNameAsync(changePasswordDto.Username.ToLower());
            if (user == null)
            {
                return NotFound("User not found");
            }

            // Mevcut şifreyi kontrol et
            var checkPasswordResult = await _signInManager.CheckPasswordSignInAsync(user, changePasswordDto.CurrentPassword, false);
            if (!checkPasswordResult.Succeeded)
            {
                return Unauthorized("Current password is incorrect");
            }

            // Yeni şifreyi güncelle
            var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok("Password changed successfully");
        }


        private async Task SendEmailAsync(string email, string subject, string message)
        {
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("iletisimwatchhub@gmail.com", "WatchHub");
            var to = new EmailAddress(email);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, message, message);

            // Email bilgilerini konsola yazdır
            Console.WriteLine($"Sending email to: {email}");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Message: {message}");

            var response = await client.SendEmailAsync(msg);

            // İsteğe bağlı olarak yanıtı kontrol edebilirsiniz
            if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Accepted)
            {
                var responseBody = await response.Body.ReadAsStringAsync();
                throw new Exception($"Failed to send email. Status code: {response.StatusCode}, Response body: {responseBody}");
            }
            else
            {
                Console.WriteLine("Varification Email is sent successfully.");
            }
        }

    }
}