using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SardCoreAPI.Controllers.Map;
using SardCoreAPI.DataAccess.DataPoints;
using SardCoreAPI.DataAccess.Units;
using SardCoreAPI.Models.DataPoints.DataPointParameters;
using SardCoreAPI.Models.DataPoints;
using SardCoreAPI.Models.Units;
using SardCoreAPI.DataAccess.Calendars;
using SardCoreAPI.Utility.Error;
using SardCoreAPI.Models.Calendars.CalendarData;
using SardCoreAPI.Models.Common;

namespace SardCoreAPI.Controllers.Calendars
{
    [ApiController]
    [Route("Library/[controller]/[action]")]
    public class CalendarController : GenericController
    {
        private readonly ILogger<MapController> _logger;

        public CalendarController(ILogger<MapController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagedSearchCriteria? criteria)
        {
            try
            {
                List<Calendar> result = await new CalendarDataAccess().Get(criteria, WorldInfo);
                if (result != null)
                {
                    return new OkObjectResult(result);
                }
                return new BadRequestResult();
            }
            catch (Exception ex)
            {
                return ex.Handle();
            }
        }

        [HttpPut]
        [Authorize(Roles = "Administrator,Editor")]
        public async Task<IActionResult> Put(Calendar calendar)
        {
            try
            {
                await new CalendarDataAccess().Put(calendar, WorldInfo);
                return Ok();
            }
            catch (Exception ex)
            {
                return ex.Handle();
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Administrator,Editor")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await new CalendarDataAccess().Delete(id, WorldInfo);
                return Ok();
            }
            catch (Exception ex)
            {
                return ex.Handle();
            }
        }
    }
}
