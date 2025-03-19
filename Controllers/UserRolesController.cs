using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSO.Data;
using SSO.Models;

namespace SSO.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserRolesController : ControllerBase
    {
        private readonly SSOContext _context;

        public UserRolesController(SSOContext context)
        {
            _context = context;
        }

        [HttpGet("ByoleId/{roleId}")]
        public async Task<ActionResult<IEnumerable<UserRoles>>> GetUserRolesByRoleId(int roleId)
        {
            var userRole = await _context.UserRoles.Where(x => x.RoleId == roleId).ToListAsync();
            return userRole;
        }

        [HttpGet("{userId}/{roleId}")]
        public async Task<ActionResult<UserRoles>> GetUserRole(int userId, int roleId)
        {
            var userRole = await _context.UserRoles
                .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);

            if (userRole == null)
            {
                return NotFound();
            }

            return userRole;
        }

        [HttpPost]
        public async Task<ActionResult<UserRoles>> CreateUserRole(UserRoles userRole)
        {
            _context.UserRoles.Add(userRole);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserRole), 
                new { userId = userRole.UserId, roleId = userRole.RoleId }, userRole);
        }

        [HttpPut("{userId}/{roleId}")]
        public async Task<IActionResult> UpdateUserRole(int userId, int roleId, UserRoles userRole)
        {
            if (userId != userRole.UserId || roleId != userRole.RoleId)
            {
                return BadRequest();
            }

            _context.Entry(userRole).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserRoleExists(userId, roleId))
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

        [HttpDelete("{userId}/{roleId}")]
        public async Task<IActionResult> DeleteUserRole(int userId, int roleId)
        {
            var userRole = await _context.UserRoles
                .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);

            if (userRole == null)
            {
                return NotFound();
            }

            _context.UserRoles.Remove(userRole);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserRoleExists(int userId, int roleId)
        {
            return _context.UserRoles.Any(ur => ur.UserId == userId && ur.RoleId == roleId);
        }
    }
}