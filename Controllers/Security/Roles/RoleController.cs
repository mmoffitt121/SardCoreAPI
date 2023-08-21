using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MySqlConnector;
using SardCoreAPI.Areas.Identity.Data;
using SardCoreAPI.Controllers.Map;
using SardCoreAPI.DataAccess.Map;
using SardCoreAPI.Models.Common;
using SardCoreAPI.Models.Map.LocationType;
using SardCoreAPI.Models.Security.Users;
using SardCoreAPI.Utility.Error;

namespace SardCoreAPI.Controllers.Security.Roles
{
    [ApiController]
    [Route("Library/[controller]/[action]")]
    public class RoleController : Controller
    {
        private readonly ILogger<MapController> _logger;
        private readonly UserManager<SardCoreAPIUser> _userManager;
        private readonly IRoleStore<IdentityRole> _roleStore;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(ILogger<MapController> logger, UserManager<SardCoreAPIUser> userManager, IRoleStore<IdentityRole> roleStore, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _userManager = userManager;
            _roleStore = roleStore;
            _roleManager = roleManager;
        }

        [Authorize(Roles = "Administrator,Editor")]
        [HttpPost]
        public async Task<IActionResult> PostRole([FromBody] IdentityRole role)
        {
            try
            {
                var result = await _roleStore.CreateAsync(role, new CancellationToken());
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

        public class UserRole
        {
            public string UserName { get; set; }
            public string RoleName { get; set; }
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> AssignRole(UserRole userRole)
        {
            SardCoreAPIUser user = await _userManager.FindByNameAsync(userRole.UserName);
            IdentityRole role = await _roleManager.FindByNameAsync(userRole.RoleName);
            if (user != null && role != null)
            {
                await _userManager.AddToRoleAsync(user, role.Name);
                return Ok();
            }
            return BadRequest();
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> UnassignRole(UserRole userRole)
        {
            SardCoreAPIUser user = await _userManager.FindByNameAsync(userRole.UserName);
            IdentityRole role = await _roleManager.FindByNameAsync(userRole.RoleName);
            if (user != null && role != null)
            {
                await _userManager.RemoveFromRoleAsync(user, role.Name);
                return Ok();
            }
            return BadRequest();
        }
    }
}
