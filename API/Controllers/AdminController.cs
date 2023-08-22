using API.Models.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace API.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = "Administrator")]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost("assignRole")]
        public async Task<IActionResult> AssignRole(UserRoleDto userRoleDto)
        {
            var user = await _userManager.FindByIdAsync(userRoleDto.UserId);
            if (user == null)
                return NotFound("User not found");

            var role = await _roleManager.FindByNameAsync(userRoleDto.RoleName);
            if (role == null)
                return NotFound("Role not found");

            var userHasRole = await _userManager.IsInRoleAsync(user, userRoleDto.RoleName);
            if (userHasRole)
                return BadRequest("User already has this role assigned");

            var result = await _userManager.AddToRoleAsync(user, userRoleDto.RoleName);

            if (result.Succeeded)
                return Ok("Role assigned successfully");

            return BadRequest("Failed to assign role");
        }
    }
}
