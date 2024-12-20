﻿using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MySqlConnector;
using SardCoreAPI.Controllers.Map;
using SardCoreAPI.DataAccess.Map;
using SardCoreAPI.Models.Common;
using SardCoreAPI.Models.Map.LocationType;
using SardCoreAPI.Models.Security;
using SardCoreAPI.Models.Security.Users;
using SardCoreAPI.Utility.Error;

namespace SardCoreAPI.Controllers.Security.Users
{
    [ApiController]
    [Route("Library/[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly ILogger<MapController> _logger;
        private readonly UserManager<SardCoreAPIUser> _userManager;
        private readonly IUserStore<SardCoreAPIUser> _userStore;

        public AccountController(ILogger<MapController> logger, UserManager<SardCoreAPIUser> userManager, IUserStore<SardCoreAPIUser> userStore)
        {
            _logger = logger;
            _userManager = userManager;
            _userStore = userStore;
        }

        [HttpPost]
        public async Task<IActionResult> PostUser([FromBody]UserPostRequest postRequest)
        {
            SardCoreAPIUser user = new SardCoreAPIUser() { UserName = postRequest.UserName, Email = postRequest.Email };
            var validationResult = await new UserValidator<SardCoreAPIUser>().ValidateAsync(_userManager, user);
            if (!validationResult.Succeeded)
            {
                return BadRequest(validationResult.Errors.Select(e => e.Description));
            }

            try
            {
                var result = await _userManager.CreateAsync(user, postRequest.Password);
                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description);

                    return BadRequest(new UserPostResponse { Errors = errors });
                }

                await _userManager.AddToRoleAsync(user, "Viewer");
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
            SardCoreAPIUser user = await _userManager.FindByNameAsync(postRequest.UserName);

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

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody] PasswordResetRequest request)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(request.UserName);

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                var result = await _userManager.ResetPasswordAsync(user, token, request.Password);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors.Select(e => e.Description));
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

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            List<SardCoreAPIUser> users = await _userManager.Users.ToListAsync();

            List<ViewableUser> vUsers = new List<ViewableUser>();

            foreach (var user in users)
            {
                IList<string> roles = await _userManager.GetRolesAsync(user);
                vUsers.Add(new ViewableUser() { Id = user.Id, UserName = user.UserName, Roles = roles });
            }
            
            return Ok(vUsers);
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete]
        public async Task<IActionResult> DeleteUser(string username)
        {
            SardCoreAPIUser user = await _userManager.FindByNameAsync(username);
            try
            {
                await _userManager.DeleteAsync(user);
            }
            catch
            {
                return BadRequest("This user cannot be deleted.");
            }
            
            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = "Editor,Administrator")]
        public async Task<IActionResult> ChangePassword(PasswordChangeRequest req)
        {
            SardCoreAPIUser user = await _userManager.FindByNameAsync(req.UserName);
            try
            {
                var passwordResult = await _userManager.ChangePasswordAsync(user, req.OldPassword, req.NewPassword);
                if (!passwordResult.Succeeded)
                {
                    return BadRequest(passwordResult.Errors.Select(e => e.Description));
                }
            }
            catch
            {
                return BadRequest("You cannot change your password at the current time.");
            }

            return Ok();
        }
    }
}
