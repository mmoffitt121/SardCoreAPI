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
    public class LibraryPermissionController : GenericController
    {
        [HttpGet]
        public async Task<IActionResult> GetPermissions()
        {
            try
            {
                Permission result = await new SecurityService().GetAllPermissions(WorldInfo);
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

        [Authorize(Roles = "Administrator,Editor")]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] SettingJSON data)
        {
            try
            {
                await new SettingJSONDataAccess().Put(data, WorldInfo);
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
                await new SettingJSONDataAccess().Delete(Id, WorldInfo);
                return Ok();
            }
            catch (Exception ex)
            {
                return ex.Handle();
            }
        }
    }
}
