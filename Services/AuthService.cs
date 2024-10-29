using Azure;
using CRM.Contexts;
using CRM.DTOs.Account;
using CRM.Entities;
using CRM.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CRM.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _config;
        private readonly AppDbContext _appDbContext;

        public AuthService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration config, AppDbContext appDbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
            _appDbContext = appDbContext;
        }

        public async Task<(int, string)> Register(RegisterDTO dto, string role)
        {
            var existedUser = await _userManager.FindByNameAsync(dto.Name);
            if(existedUser != null)
            {
                return (0, "User already exist!");
            }

            User user = new User()
            {
                Name = dto.Name,
                Email = dto.Email,
                UserName = dto.Name,
                Role = RoleType.User,
            };

            var createUserResult = await _userManager.CreateAsync(user, dto.Password);
            if (!createUserResult.Succeeded)
            {
                var errors = createUserResult.Errors;
                var message = string.Join(", ", errors.Select(x => "Code " + x.Code + " Description" + x.Description));
                return (0, message);
            }

            if(!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }

            if(await _roleManager.RoleExistsAsync(role))
            {
                await _userManager.AddToRoleAsync(user, role);
            }

            _appDbContext.AppUsers.Add(user);
            _appDbContext.SaveChanges();

            return (1, $"{dto.Name} registered succesfully");
        }

        public async Task<(int, string, string)> Login(LoginDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if(user is null)
            {
                return (0, "", "User not found!");
            }

            if(!await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                return (0, "", "Password is invalid");
            }

            var authClaims = new List<Claim>
            {
               new Claim("Email", user.Email),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var userRoles = await _userManager.GetRolesAsync(user);

            foreach(var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            string token = GenerateToken(authClaims);

            return (1, token, "User Logged in successfully!");
        }

        private string GenerateToken(IEnumerable<Claim> claims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SecurityKey"]));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _config["JWT:ValidIssuer"],
                Audience = _config["JWT:ValidAudience"],
                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(claims)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
