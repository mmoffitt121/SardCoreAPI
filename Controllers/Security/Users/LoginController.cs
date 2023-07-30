using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SardCoreAPI.Areas.Identity.Data;
using SardCoreAPI.Controllers.Map;
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
        private readonly UserManager<SardCoreAPIUser> _userManager;

        public LoginController(ILogger<MapController> logger, JwtHandler jwtHandler, UserManager<SardCoreAPIUser> userManager)
        {
            _logger = logger;
            _jwtHandler = jwtHandler;
            _userManager = userManager;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] AuthRequest authRequest)
        {
            SardCoreAPIUser user = await _userManager.FindByNameAsync(authRequest.Username);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(authRequest.Username);
            }

            try
            {
                if (user == null || !await _userManager.CheckPasswordAsync(user, authRequest.Password))
                {
                    return new UnauthorizedObjectResult("Invalid username or password.");
                }
            }
            catch (Exception ex)
            {
                return new UnauthorizedObjectResult(ex);
            }

            var signingCredentials = _jwtHandler.GetSigningCredentials();
            var claims = await _jwtHandler.GetClaims(user);
            var tokenOptions = _jwtHandler.GenerateTokenOptions(signingCredentials, claims);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return new OkObjectResult(new AuthResponse { IsAuthSuccessful = true, Token = token });
        }
    }
}
