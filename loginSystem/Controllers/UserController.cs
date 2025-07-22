using loginSystem.Dtos;
using loginSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace loginSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateUser(RegisterDto userDTO)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = userDTO.UserName,
                    Email = userDTO.Email,
                    PasswordHash = userDTO.Password,
                };

                IdentityResult result = await userManager.CreateAsync(user, userDTO.Password);
                if (result.Succeeded)
                {
                    return Ok($"User {user.UserName} created successfully");
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult GetUsers()
        {
            var users = userManager.Users.ToList();
            return Ok(users);
        }

        [HttpDelete]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteUser(string Name)
        {
            ApplicationUser user = await userManager.FindByNameAsync(Name);
            if (user != null)
            {
                IdentityResult result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return Ok($"User {Name} deleted successfully");
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            return NotFound($"User with Name {Name} not found");
        }

        [HttpPut]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateUser(string userName, RegisterDto userDTO)
        {
            ApplicationUser user = await userManager.FindByNameAsync(userName);
            if (user != null)
            {
                user.UserName = userDTO.UserName;
                user.PasswordHash = userDTO.Password;
                user.Email = userDTO.Email;

                IdentityResult result = await userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return Ok($"User {userName} updated successfully");
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            return NotFound($"User with Name {userName} not found");
        }
    }
}
