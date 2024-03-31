using API_Lab2.DTOs;
using API_Lab2.Entity;
using API_Lab2.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static System.Net.WebRequestMethods;

namespace API_Lab2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UniContext db;
        private readonly IConfiguration config;
        private readonly UserManager<ApplicationUser> userManager;

        public AccountController(UniContext _db, UserManager<ApplicationUser> _userManager, IConfiguration _config)
        {
            db = _db;
            userManager = _userManager;
            config = _config;
        }

        //create account "Registeration"
        [HttpPost("Registration")]
        public async Task<IActionResult> Registration(RegisterUserDTO userDTO)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser();
                user.Email = userDTO.Email;
                user.UserName = userDTO.UserName;
                IdentityResult result = await userManager.CreateAsync(user,userDTO.Password);
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user,"Admin");
                    return Ok("User Added Successfully !");
                }
                return BadRequest(result.Errors.FirstOrDefault());
            }
            return BadRequest(ModelState);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginUserDTO userDTO)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await userManager.FindByNameAsync(userDTO.UserName);
                if (user == null)
                {
                    return Unauthorized();
                }
                bool found = await userManager.CheckPasswordAsync(user, userDTO.Passwrod);
                if (found)
                {
                    var Claims = new List<Claim>();
                    Claims.Add(new Claim(ClaimTypes.Name, user.UserName));
                    Claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                    Claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                    var roles = await userManager.GetRolesAsync(user);
                    foreach (var role in roles)
                    {
                        Claims.Add(new Claim(ClaimTypes.Role, role));
                    }
                    SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:SecruityKey"]));
                    SigningCredentials signingCred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    JwtSecurityToken myToken = new JwtSecurityToken(
                        issuer: config["JWT:ValiadteIssure"],
                        audience: config["JWT:ValidateAud"],
                        claims:Claims,
                        expires:DateTime.Now.AddHours(1),
                        signingCredentials: signingCred
                        );
                    return Ok(new
                    {
                        msg = "Valid User",
                        token = new JwtSecurityTokenHandler().WriteToken(myToken),
                        expiration = myToken.ValidTo
                    });
                }
            }
            return BadRequest();
        }
    }
}
