using Microsoft.AspNetCore.Mvc;
using SardCoreAPI.Attributes.Security;
using SardCoreAPI.DataAccess.DataPoints;
using SardCoreAPI.Models.DataPoints;
using SardCoreAPI.Models.DataPoints.Queried;
using SardCoreAPI.Services.DataPoints;
using SardCoreAPI.Utility.Validation;

namespace SardCoreAPI.Controllers.DataPoints
{
    [ApiController]
    [Route("Library/[controller]/[action]")]
    public class DataPointQueryController : GenericController
    {
        private IDataPointService _dataPointService;

        public DataPointQueryController(IDataPointService dataPointService)
        {
            _dataPointService = dataPointService;
        }

        [HttpPost]
        [Validate]
        [Resource("Library.Document.Read")]
        public async Task<IActionResult> Get([FromBody] DataPointSearchCriteria criteria)
        {
            if (criteria == null) { return new BadRequestResult(); }

            DataPointQueryResult result = await _dataPointService.GetDataPoints(criteria);
            if (result != null)
            {
                return new OkObjectResult(result);
            }
            return new BadRequestResult();
        }
    }
}
