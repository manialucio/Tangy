using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Tangy_Common;
using Tangy_DataAccess;
using Tangy_Models;
using TangyWeb_Api.Helper;

namespace TangyWeb_Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly APISettings _apiSettings;
        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
          IOptions<APISettings> options)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _apiSettings = options.Value;
        }
        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequestDto signUpRequest)
        {
            if (signUpRequest == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = new ApplicationUser
            {
                UserName = signUpRequest.Email,
                Email = signUpRequest.Email,
                Name = signUpRequest.Name,
                PhoneNumber = signUpRequest.PhoneNumber,
                EmailConfirmed = true

            };
            var result = await _userManager.CreateAsync(user, signUpRequest.Password);
            if (!result.Succeeded)
            {
                return BadRequest(new SignUpResponseDto()
                {
                    IsRegistrationSuccessful = false,
                    Errors = result.Errors.Select(e => e.Description)
                });
            }
            var roleResult = await _userManager.AddToRoleAsync(user, SD.Roles.Customer);
            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                return BadRequest(new SignUpResponseDto()
                {
                    IsRegistrationSuccessful = false,
                    Errors = roleResult.Errors.Select(e => e.Description)
                });
            }
            return new CreatedResult("User",
                                      new SignUpResponseDto() { IsRegistrationSuccessful = true }
                                      );
        }


        [HttpPost]
        public async Task<IActionResult> SignIn([FromBody] SignInRequestDto signInRequest)
        {
            if (signInRequest == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _signInManager.PasswordSignInAsync(signInRequest.Name, signInRequest.Password, false, false);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(signInRequest.Name);
                if (user == null)
                {
                    return Unauthorized(new SignInResponseDto
                    {
                        IsAuthSuccessful = false,
                        ErrorMessage = "not authentication"
                    });
                }
                var signinCredentials = GetSigningCredentials();
                var claims = await GetClaimsAsync(user);
                var tokenOptions = new JwtSecurityToken(
                    issuer: _apiSettings.ValidIssuer,
                    audience: _apiSettings.ValidAudience,
                    claims: claims,
                    signingCredentials: signinCredentials,
                    expires: DateTime.Now.AddDays(30)
                    );
                var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                return Ok(new SignInResponseDto
                {
                    IsAuthSuccessful = true,
                    User = new UserDto
                    {
                        Name = user.Name,
                        Email = user.Email,
                        Id = user.Id,
                        PhoneNumber = user.PhoneNumber,
                    },
                    Token = token,
                });

            }   
            else
            {
                return Unauthorized(new SignInResponseDto
                {
                    IsAuthSuccessful = false,
                    ErrorMessage = "not authenticated"
                });
            }
        }
        private SigningCredentials GetSigningCredentials()
        {
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_apiSettings.SecretKey));
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaimsAsync(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
            };
            var roles = await _userManager.GetRolesAsync(await _userManager.FindByEmailAsync(user.Email));
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }
    }
}
