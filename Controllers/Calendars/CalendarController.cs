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
using SardCoreAPI.Utility.Validation;
using SardCoreAPI.DataAccess.Easy;
using SardCoreAPI.Models.Calendars;
using SardCoreAPI.Attributes.Security;
using Humanizer.Localisation;
using SardCoreAPI.Services.Security;

namespace SardCoreAPI.Controllers.Calendars
{
    [ApiController]
    [Route("Library/[controller]/[action]")]
    public class CalendarController : GenericController
    {
        private readonly ILogger<MapController> _logger;
        private readonly IEasyDataAccess _dataAccess;

        public CalendarController(ILogger<MapController> logger, IEasyDataAccess dataAccess)
        {
            _logger = logger;
            _dataAccess = dataAccess;
        }

        [HttpGet]
        [Resource("Library.General.Read")]
        public async Task<IActionResult> Get([FromQuery] PagedSearchCriteria? criteria)
        {
            try
            {
                List<CalendarDataAccessWrapper> wrappers = await _dataAccess.Get<CalendarDataAccessWrapper>(new { Id = criteria?.Id }, queryOptions: criteria);
                List<Calendar> result = new List<Calendar>();
                foreach (var item in wrappers)
                {
                    result.Add(item.Calendar);
                }
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                return ex.Handle();
            }
        }

        [HttpPut]
        [Validate]
        [Resource("Library.Setup.Calendars")]
        public async Task<IActionResult> Put(Calendar calendar)
        {
            try
            {
                await _dataAccess.Put(new CalendarDataAccessWrapper(calendar), insert: true);
                return Ok();
            }
            catch (Exception ex)
            {
                return ex.Handle();
            }
        }

        [HttpDelete]
        [Resource("Library.Setup.Calendars")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _dataAccess.Delete<CalendarDataAccessWrapper>(new { id });
                return Ok();
            }
            catch (Exception ex)
            {
                return ex.Handle();
            }
        }
    }
}
