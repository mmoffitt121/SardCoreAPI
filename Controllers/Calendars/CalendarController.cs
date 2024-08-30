using Microsoft.AspNetCore.Mvc;
using SardCoreAPI.Utility.Error;
using SardCoreAPI.Models.Calendars.CalendarData;
using SardCoreAPI.Models.Common;
using SardCoreAPI.Utility.Validation;
using SardCoreAPI.Models.Calendars;
using SardCoreAPI.Attributes.Security;
using SardCoreAPI.Services.Context;
using SardCoreAPI.Utility.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace SardCoreAPI.Controllers.Calendars
{
    [ApiController]
    [Route("Library/[controller]/[action]")]
    public class CalendarController : GenericController
    {
        private readonly IDataService _dataService;
        public CalendarController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet]
        [Resource("Library.General.Read")]
        public async Task<IActionResult> Get([FromQuery] PagedSearchCriteria? criteria)
        {
            try
            {
                List<CalendarDataAccessWrapper> wrappers = _dataService.Context.Calendar
                    .Where(c => criteria == null || criteria.Id == null || c.Id.Equals(criteria.Id))
                    .Skip(criteria?.PageNumber ?? 0 * criteria?.PageSize ?? 0)
                    .Take(criteria?.PageSize ?? 36)
                    .ToList();

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
            _dataService.Context.Calendar.Put(new CalendarDataAccessWrapper(calendar), c => c.Id == calendar.Id);
            return await Handle(_dataService.Context.SaveChangesAsync());
        }

        [HttpDelete]
        [Resource("Library.Setup.Calendars")]
        public async Task<IActionResult> Delete(int id)
        {
            return await Handle(_dataService.Context.Calendar.Where(c => c.Id == id).ExecuteDeleteAsync());
        }
    }
}
