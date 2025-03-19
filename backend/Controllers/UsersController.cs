using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSO.Data;
using SSO.DTO;
using SSO.Models;
using static SSO.Controllers.AuthController;
using SSO.Helper;

namespace SSO.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly SSOContext _context;

        public UsersController(SSOContext context)
        {
            _context = context;
        }

        [HttpGet("GetAllUsers")]
        public async Task<ActionResult<IEnumerable<Users>>> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Users>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPost]
        public async Task<ActionResult<Users>> CreateUser([FromBody] UserCreateDTO model )
        {
            if (_context.Users.Any(u => u.UserId == model.UserId))
                return BadRequest("Username:{model.UserName} already exists");

            var user = new Users
            {
                UserId = model.UserId,
                UserName = model.UserName,
                IsDeleted = false,
                PasswordHash = PasswordHelper.HashPassword(model.Password),
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateDTO model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            // 查詢用戶
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // 驗證舊密碼是否正確
            if (!PasswordHelper.VerifyPassword(model.oldPassword, user.PasswordHash))
            {
                return BadRequest("舊密碼不正確");
            }

            // 檢查新密碼與確認密碼是否一致
            if (model.newPassword != model.checkPassword)
            {
                return BadRequest("新密碼與確認密碼不一致");
            }
            // 更新用戶資料
            user.UserName = model.UserName;
            // 如果新密碼有變更，則更新密碼
            if (!string.IsNullOrEmpty(model.newPassword))
            {
                user.PasswordHash = PasswordHelper.HashPassword(model.newPassword);
            }
            
            // 標記為已更改的實體
            _context.Entry(user).State = EntityState.Modified;
            try
            {
                // 儲存更改
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            user.IsDeleted = true;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
