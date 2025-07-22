using loginSystem.Dtos;
using loginSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace loginSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration config;

        public AccountController(UserManager<ApplicationUser> userManager, IConfiguration config)
        {
            this.userManager = userManager;
            this.config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Registe(RegisterDto userDTO)
        {
            if (ModelState.IsValid)
            {
                var isFirstUser = !userManager.Users.Any();
                ApplicationUser AppUser = new ApplicationUser()
                {
                    UserName = userDTO.UserName,
                    Email = userDTO.Email,
                    PasswordHash = userDTO.Password,
                };
                IdentityResult Result = await userManager.CreateAsync(AppUser, userDTO.Password);
                if (Result.Succeeded)
                {
                    if (isFirstUser)
                    {
                        await userManager.AddToRoleAsync(AppUser, "admin"); // Primer usuario
                    }
                    else
                    {
                        await userManager.AddToRoleAsync(AppUser, "user"); // Los demás
                    }
                    return Ok("Account Created");
                }
                return BadRequest(Result.Errors);
            }
            return BadRequest(ModelState);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto userDTO)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser? UserFromDB = await userManager.FindByNameAsync(userDTO.UserName);
                if (UserFromDB != null)
                {
                    bool found = await userManager.CheckPasswordAsync(UserFromDB, userDTO.Password);
                    if (found)
                    {
                        //Create Token
                        List<Claim> myclaims = new List<Claim>();
                        myclaims.Add(new Claim(ClaimTypes.Name, UserFromDB.UserName));
                        myclaims.Add(new Claim(ClaimTypes.NameIdentifier, UserFromDB.Id));
                        myclaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

                        var roles = await userManager.GetRolesAsync(UserFromDB);
                        foreach (var role in roles)
                        {
                            myclaims.Add(new Claim(ClaimTypes.Role, role));
                        }

                        var SignKey = new SymmetricSecurityKey(
                           Encoding.UTF8.GetBytes(config["JWT:Key"]));

                        SigningCredentials signingCredentials =
                            new SigningCredentials(SignKey, SecurityAlgorithms.HmacSha256);

                        JwtSecurityToken mytoken = new JwtSecurityToken(
                           issuer: config["JWT:Issuer"],//provider create token
                           audience: config["JWT:Audience"],//cousumer url
                        expires: DateTime.Now.AddHours(1),
                           claims: myclaims,
                           signingCredentials: signingCredentials);
                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(mytoken),
                            expired = mytoken.ValidTo
                        });
                    }
                }
                return BadRequest("Invalid Request");
            }
            return BadRequest(ModelState);
        }
    }
}
