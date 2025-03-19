using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using SSO.Models; //  確保導入模型
using SSO.DTO;
using SSO.Data;
using Microsoft.EntityFrameworkCore;

namespace SSO.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SSOContext _dbContext;
        private readonly IConfiguration _configuration;

        public AuthController(
            SSOContext context,
            IConfiguration configuration)
        {
            _dbContext = context;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            if (_dbContext.Users.Any(u => u.UserId == model.UserId && u.SystemName == model.SystemName))
                return BadRequest("SystemName:{model.SystemName}/Username:{model.UserName} already exists");

            var user = new Users
            {
                UserId = model.UserId,
                SystemName = model.SystemName,
                UserName = model.UserName,
                IsDeleted = false,
                PasswordHash = PasswordHelper.HashPassword(model.Password),
            };

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            return Ok(new { message = "User registered successfully!" });
        }

        public static class PasswordHelper
        {
            public static string HashPassword(string password)
            {
                using (var sha256 = System.Security.Cryptography.SHA256.Create())
                {
                    var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                    return Convert.ToBase64String(hashedBytes);
                }
            }

            public static bool VerifyPassword(string password, string hashedPassword)
            {
                return HashPassword(password) == hashedPassword;
            }
        }

        [HttpGet("check-userid")]
        public async Task<IActionResult> CheckUserIdExists([FromQuery] string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return BadRequest(new { message = "UserId is required." });
            }

            bool exists = await _dbContext.Users.AnyAsync(u => u.UserId == userId);

            return Ok(new { exists });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == model.UserId);
            if (user == null || !PasswordHelper.VerifyPassword(model.Password, user.PasswordHash))
                return Unauthorized("Invalid username or password");

            var token = GenerateJwtToken(user);
            return Ok(new { token });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO model)
        {
            if (string.IsNullOrWhiteSpace(model.UserId))
            {
                return BadRequest(new { message = "UserId is required." });
            }

            // 查詢使用者
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == model.UserId);
            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            // 產生新密碼
            string newPassword = GenerateRandomPassword(10);

            // Hash 密碼
            user.PasswordHash = PasswordHelper.HashPassword(newPassword);

            // 更新資料庫
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();

            // 發送 Email（你需要實作 SendEmail 方法）
            //await _emailService.SendEmail(model.Email, "Password Reset", $"Your new password is: {newPassword}");
            //return Ok(new { message = "Password has been reset. Please check your email." });
            return Ok(new { message = $"Password :{newPassword}." });
        }

        private string GenerateRandomPassword(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }



        private string GenerateJwtToken(Users user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // 處理 UserRoles，把它轉成字串
            var roles = user.UserRoles?.Select(r => r.RoleId.ToString()).ToList() ?? new List<string>();
            var rolesString = string.Join(",", roles); // 角色用逗號分隔成字串
            // 添加 SystemName，這樣 JWT 就能區分不同系統的使用者
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim("id", user.Id.ToString()),
                new Claim("userid", user.UserId),
                new Claim("username", user.UserName),
                new Claim("system", user.SystemName ?? ""), 
                new Claim("userRoles", rolesString), // 存角色字串
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // JWT 唯一識別碼
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
