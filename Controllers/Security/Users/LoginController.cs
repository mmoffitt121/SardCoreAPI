using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SardCoreAPI.Controllers.Map;
using SardCoreAPI.DataAccess.Security.Users;
using SardCoreAPI.Models.Security.JWT;
using SardCoreAPI.Models.Security.Users;
using SardCoreAPI.Utility.JwtHandler;
using System.IdentityModel.Tokens.Jwt;

namespace SardCoreAPI.Controllers.Security.Users
{
    [ApiController]
    [Route("Library/[controller]/[action]")]
    public class LoginController
    {
        private readonly ILogger<MapController> _logger;
        private readonly JwtHandler _jwtHandler;
        private readonly UserManager<User> _userManager;

        public LoginController(ILogger<MapController> logger, JwtHandler jwtHandler, UserManager<User> userManager)
        {
            _logger = logger;
            _jwtHandler = jwtHandler;
            _userManager = userManager;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] AuthRequest authRequest)
        {
            User user = await _userManager.FindByNameAsync(authRequest.Username);
            try
            {
                if (user == null || !await _userManager.CheckPasswordAsync(user, authRequest.Password))
                {
                    return new UnauthorizedResult();
                }
            }
            catch (Exception ex)
            {
                return new UnauthorizedObjectResult(ex);
            }

            var signingCredentials = _jwtHandler.GetSigningCredentials();
            var claims = _jwtHandler.GetClaims(user);
            var tokenOptions = _jwtHandler.GenerateTokenOptions(signingCredentials, claims);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return new OkObjectResult(new AuthResponse { IsAuthSuccessful = true, Token = token });
        }
    }
}
