using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SardCoreAPI.DataAccess.Map;
using SardCoreAPI.Models.Hub.Worlds;
using SardCoreAPI.Models.Security.LibraryRoles;
using SardCoreAPI.Models.Settings;
using SardCoreAPI.Services.Security;
using SardCoreAPI.Utility.Error;

namespace SardCoreAPI.Controllers.Security.LibraryRoles
{
    [ApiController]
    [Route("Library/[controller]/[action]")]
    public class LibraryRoleController : GenericController
    {
        private ISecurityService securityService;

        [HttpGet]
        public async Task<IActionResult> GetPermissions()
        {
            try
            {
                Permission result = await securityService.GetAllPermissions(WorldInfo);
                if (result != null)
                {
                    return new OkObjectResult(result);
                }
                return new NotFoundResult();
            }
            catch (Exception ex)
            {
                return ex.Handle();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string? id)
        {
            try
            {
                List<Role> roles = await securityService.GetRoles(id, WorldInfo);
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

        [Authorize(Roles = "Administrator,Editor")]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Role data)
        {
            try
            {
                await securityService.UpdateRoles(new Role[] { data }, WorldInfo);
                return Ok();
            }
            catch (Exception ex)
            {
                return ex.Handle();
            }
        }

        [Authorize(Roles = "Administrator,Editor")]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] string Id)
        {
            try
            {
                await securityService.DeleteRole(Id, WorldInfo);
                return Ok();
            }
            catch (Exception ex)
            {
                return ex.Handle();
            }
        }

        public LibraryRoleController(ISecurityService securityService)
        {
            this.securityService = securityService;
        }
    }
}
