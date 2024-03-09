using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SardCoreAPI.DataAccess.Calendars;
using SardCoreAPI.DataAccess.Map;
using SardCoreAPI.Models.Map.Location;
using SardCoreAPI.Models.Map.LocationType;
using SardCoreAPI.Models.Settings;
using SardCoreAPI.Utility.Error;

namespace SardCoreAPI.Controllers.Map
{
    [ApiController]
    [Route("Library/[controller]/[action]")]
    public class SettingJSONController : GenericController
    {
        private readonly ILogger<MapController> _logger;

        public SettingJSONController(ILogger<MapController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string Id)
        {
            try
            {
                List<SettingJSON> result = await new SettingJSONDataAccess().Get(Id, WorldInfo);
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
