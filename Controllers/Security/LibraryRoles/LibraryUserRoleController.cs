using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SardCoreAPI.DataAccess.Map;
using SardCoreAPI.Models.Hub.Worlds;
using SardCoreAPI.Models.Security.LibraryRoles;
using SardCoreAPI.Models.Security.Users;
using SardCoreAPI.Models.Settings;
using SardCoreAPI.Services.Security;
using SardCoreAPI.Utility.Error;

namespace SardCoreAPI.Controllers.Security.LibraryRoles
{
    [ApiController]
    [Route("Library/[controller]/[action]")]
    public class LibraryUserRoleController : GenericController
    {
        private ISecurityService securityService;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                ViewableLibraryUser[] roles = await securityService.GetUsersWithRoles(WorldInfo);
                if (roles != null)
                {
                    return new OkObjectResult(roles);
                }
                return new NotFoundResult();
            }
            catch (Exception ex)
            {
                return ex.Handle();
            }
        }

        //[Authorize(Roles = "Administrator,Editor")]
        [HttpPut]
        public async Task<IActionResult> Put([FromQuery] string user, [FromBody] string[] roles)
        {
            try
            {
                await securityService.UpdateUserRoles(user, roles, WorldInfo);
                return Ok();
            }
            catch (Exception ex)
            {
                return ex.Handle();
            }
        }

        public LibraryUserRoleController(ISecurityService securityService)
        {
            this.securityService = securityService;
        }
    }
}
