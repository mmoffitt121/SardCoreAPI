using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SardCoreAPI.Attributes.Security;
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
        [Resource("Library.Setup.Security.Read")]
        public async Task<IActionResult> GetPermissions()
        {
            try
            {
                return Ok(await securityService.GetAllPermissions());
            }
            catch (Exception ex)
            {
                return ex.Handle();
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUserPermissions()
        {
            try
            {
                return Ok(await securityService.GetUserPermissions());
            }
            catch (Exception ex)
            {
                return ex.Handle();
            }
        }

        [HttpGet]
        [Resource("Library.Setup.Security.Read")]
        public async Task<IActionResult> Get([FromQuery] string? id)
        {
            try
            {
                List<Role> roles = await securityService.GetRoles(id);
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

        [HttpPut]
        [Resource("Library.Setup.Security")]
        public async Task<IActionResult> Put([FromBody] Role data)
        {
            try
            {
                await securityService.UpdateRoles([data]);
                return Ok();
            }
            catch (Exception ex)
            {
                return ex.Handle();
            }
        }

        [HttpDelete]
        [Resource("Library.Setup.Security")]
        public async Task<IActionResult> Delete([FromQuery] string Id)
        {
            try
            {
                await securityService.DeleteRole(Id);
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
