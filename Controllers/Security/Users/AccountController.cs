﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MySqlConnector;
using SardCoreAPI.Controllers.Map;
using SardCoreAPI.DataAccess.Map;
using SardCoreAPI.DataAccess.Security.Users;
using SardCoreAPI.Models.Common;
using SardCoreAPI.Models.Map.LocationType;
using SardCoreAPI.Models.Security.Users;
using SardCoreAPI.Utility.Error;

namespace SardCoreAPI.Controllers.Security.Users
{
    [ApiController]
    [Route("Library/[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly ILogger<MapController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly IUserStore<User> _userStore;

        public AccountController(ILogger<MapController> logger, UserManager<User> userManager, IUserStore<User> userStore)
        {
            _logger = logger;
            _userManager = userManager;
            _userStore = userStore;
        }

        [HttpPost]
        public async Task<IActionResult> PostUser([FromBody]UserPostRequest postRequest)
        {
            User user = new User() { UserName = postRequest.UserName, Email = postRequest.Email };
            var validationResult = await new UserValidator().ValidateAsync(_userManager, user);
            if (!validationResult.Succeeded)
            {
                return BadRequest(validationResult.Errors.Select(e => e.Description));
            }

            try
            {
                var result = await _userStore.CreateAsync(user, new CancellationToken());
                
            }
            catch (MySqlException ex)
            {
                return ex.Handle();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }
            
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> PostPassword([FromBody]PasswordPostRequest postRequest)
        {
            User user = await _userManager.FindByNameAsync(postRequest.UserName);

            try
            {
                var passwordResult = await _userManager.AddPasswordAsync(user, postRequest.Password);
                if (!passwordResult.Succeeded)
                {
                    return BadRequest(passwordResult.Errors.Select(e => e.Description));
                }
            }
            catch (MySqlException ex)
            {
                return ex.Handle();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> PutPassword(string currentPwd, string newPwd, string userName)
        {
            User user = await _userManager.FindByNameAsync(userName);

            try
            {
                var passwordResult = await _userManager.ChangePasswordAsync(user, currentPwd, newPwd);
                if (!passwordResult.Succeeded)
                {
                    return BadRequest(passwordResult.Errors.Select(e => e.Description));
                }
            }
            catch (MySqlException ex)
            {
                return ex.Handle();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }

            return Ok();
        }

    }
}
