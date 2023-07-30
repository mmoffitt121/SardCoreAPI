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

        public RoleController(ILogger<MapController> logger, UserManager<SardCoreAPIUser> userManager, IRoleStore<IdentityRole> roleStore)
        {
            _logger = logger;
            _userManager = userManager;
            _roleStore = roleStore;
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
    }
}
