using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.Models.DTO;
using NZWalks.Repository;

namespace NZWalks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenRepository _tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            _userManager = userManager;
            _tokenRepository = tokenRepository;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDto.UserName,
                Email =  registerRequestDto.UserName
            };

            var identityResult = await _userManager.CreateAsync(identityUser,registerRequestDto.Password);

            if(identityResult.Succeeded)
            {
                if(registerRequestDto.Roles !=null && registerRequestDto.Roles.Any())
                    identityResult = await _userManager.AddToRoleAsync(identityUser, registerRequestDto.Roles);
            
                    if(identityResult.Succeeded)
                {
                    return Ok("Registered Successfully !!");
                }
            }
            return BadRequest("something went wrong");
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
          var user = await _userManager.FindByEmailAsync(loginRequestDto.UserName);

            if( user != null)
            {

                var checkPassword = _userManager.CheckPasswordAsync(user, loginRequestDto.Password);
                if(checkPassword != null)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    var jwtToken = _tokenRepository.CreateJWTToken(user, roles.ToList());

                   return Ok(jwtToken);
                }
            }
            return BadRequest("Something Went Wrong");
        }
    }
}
