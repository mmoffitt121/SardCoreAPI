using Microsoft.AspNetCore.Mvc;
using SardCoreAPI.DataAccess.DataPoints;
using SardCoreAPI.Models.DataPoints;
using SardCoreAPI.Utility.Validation;
using SardCoreAPI.Attributes.Security;
using SardCoreAPI.Services.Context;
using SardCoreAPI.Services.DataPoints;
using SardCoreAPI.Models.DataPoints.Queried;

namespace SardCoreAPI.Controllers.DataPoints
{
    [ApiController]
    [Route("Library/[controller]/[action]")]
    public class DataPointController : GenericController
    {
        private readonly IDataService data;
        private readonly IDataPointService dpService;

        public DataPointController(IDataService data, IDataPointService dpService)
        {
            this.data = data;
            this.dpService = dpService;
        }

        [HttpGet]
        [Validate]
        [Resource("Library.Document.Read")]
        public async Task<IActionResult> GetEmptyDataPoint([FromQuery] int typeId)
        {
            return await Handle(dpService.GetEmpty(typeId));
        }

        [HttpPost(Name = "GetDataPoints")]
        [Validate]
        [Resource("Library.Document.Read")]
        public async Task<IActionResult> GetDataPoints([FromBody] DataPointSearchCriteria criteria)
        {
            return await Handle(dpService.GetDataPoints(criteria));
        }

        [HttpGet]
        [Resource("Library.Document.Read")]
        public async Task<IActionResult> GetDataPointsReferencingDataPoint(int? id)
        {
            // TODO: Do we need this?
            if (id == null) { return new BadRequestResult(); }

            List<DataPoint> result = await new DataPointDataAccess().GetDataPointsReferencingDataPoint((int)id, WorldInfo);
            if (result != null)
            {
                return new OkObjectResult(result);
            }
            return new BadRequestResult();
        }

        [HttpPut(Name = "PutDataPoint")]
        [Resource("Library.Document")]
        public async Task<IActionResult> PutDataPoint([FromBody] QueriedDataPoint dp)
        {
            return await Handle(dpService.PutDataPoint(new DataPoint(dp)));
        }

        [HttpDelete(Name = "DeleteDataPoint")]
        [Resource("Library.Document")]
        public async Task<IActionResult> DeleteDataPoint([FromQuery] int Id)
        {
            return await Handle(dpService.DeleteDataPoint(Id));
        }
    }
}
