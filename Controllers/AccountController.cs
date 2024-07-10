using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using VicemAPI.Data;
using VicemAPI.Models.Entities;
using VicemAPI.Models.Process;
using VicemAPI.Models.ViewModels;
namespace VicemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private ApplicationDbContext _context;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager;
            _context = context;
        }
        [Authorize(Policy = nameof(SystemPermissions.GetAllUser))]
        [HttpGet("get-all-user")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userManager.Users.Select(user => new
            {
                user.Id,
                user.Email,
                Roles = _userManager.GetRolesAsync(user).Result
            }).ToListAsync();

            return Ok(users);
        }
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, FullName = model.FullName };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return Ok(new { Message = "User registered successfully" });
                }

                return BadRequest(result.Errors);
            }

            return BadRequest(ModelState);
        }
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email,
                        model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(model.Email);
                    var token = await GenerateJsonWebToken(user);
                    // return Ok(new { token = token, userName = user.UserName });
                    var refreshToken = await GenerateRefreshToken(user.Id);
                    return Ok(new { token = token, refreshToken = refreshToken.Token, userName = user.UserName });
                }

                return Unauthorized();
            }
            return BadRequest(ModelState);
        }
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { Message = "Logout successful" });
        }
        private async Task<string> GenerateJsonWebToken(ApplicationUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));
            foreach (var role in userRoles)
            {
                var roleClaims = await _roleManager.GetClaimsAsync(await _roleManager.FindByNameAsync(role));
                claims.AddRange(roleClaims.Where(c => c.Type == "Permission"));
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"], audience: _configuration["Jwt:Audience"], claims: claims,
            expires: DateTime.Now.AddSeconds(40), signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequest model)
        {
            if (model is null)
            {
                return BadRequest("Invalid client request");
            }

            var user = await _userManager.Users.Include(u => u.RefreshTokens)
                                               .FirstOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == model.RefreshToken));

            if (user is null || user.RefreshTokens.All(t => t.Token != model.RefreshToken || t.IsExpired || t.IsRevoked))
            {
                return Unauthorized();
            }

            var refreshToken = user.RefreshTokens.First(t => t.Token == model.RefreshToken);
            if (!refreshToken.IsActive)
            {
                return Unauthorized();
            }

            var newJwtToken = await GenerateJsonWebToken(user);
            var newRefreshToken = await GenerateRefreshToken(user.Id);

            refreshToken.Revoked = DateTime.UtcNow;
            _context.RefreshTokens.Update(refreshToken);
            await _context.SaveChangesAsync();

            return Ok(new { token = newJwtToken, refreshToken = newRefreshToken.Token });
        }

        private async Task<RefreshToken> GenerateRefreshToken(string userId)
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                UserId = userId
            };
            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();
            return refreshToken;
        }

    }
}