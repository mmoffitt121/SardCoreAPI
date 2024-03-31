using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SardCoreAPI.Attributes.Security;
using SardCoreAPI.DataAccess.Calendars;
using SardCoreAPI.DataAccess.Map;
using SardCoreAPI.Models.Map.Location;
using SardCoreAPI.Models.Map.LocationType;
using SardCoreAPI.Models.Settings;
using SardCoreAPI.Services.Setting;
using SardCoreAPI.Utility.Error;

namespace SardCoreAPI.Controllers.Map
{
    [ApiController]
    [Route("Library/[controller]/[action]")]
    public class SettingJSONController : GenericController
    {
        private readonly ILogger<MapController> _logger;
        private readonly ISettingJSONService _settingService;

        public SettingJSONController(ILogger<MapController> logger, ISettingJSONService settingService)
        {
            _logger = logger;
            _settingService = settingService;
        }

        [HttpGet]
        [Resource("Library.Setup.Settings.Read")]
        public async Task<IActionResult> Get([FromQuery] string Id)
        {
            try
            {
                List<SettingJSON> result = await _settingService.Get(Id);
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

        [HttpPut]
        [Resource("Library.Setup.Settings")]
        public async Task<IActionResult> Put([FromBody] SettingJSON data)
        {
            try
            {
                await _settingService.Put(data);
                return Ok();
            }
            catch (Exception ex)
            {
                return ex.Handle();
            }
        }

        [HttpDelete]
        [Resource("Library.Setup.Settings")]
        public async Task<IActionResult> Delete([FromQuery] string Id)
        {
            try
            {
                await _settingService.Delete(Id);
                return Ok();
            }
            catch (Exception ex)
            {
                return ex.Handle();
            }
        }
    }
}
