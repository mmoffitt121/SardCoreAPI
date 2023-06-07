using Microsoft.AspNetCore.Mvc;
using SardCoreAPI.Controllers.Map;
using SardCoreAPI.DataAccess.DataPoints;
using SardCoreAPI.Models.Common;
using SardCoreAPI.Models.DataPoints.DataPointParameters;
using SardCoreAPI.Models.DataPoints;

namespace SardCoreAPI.Controllers.DataPoints
{
    [ApiController]
    [Route("Library/[controller]/[action]")]
    public class DataPointController : Controller
    {
        private readonly ILogger<MapController> _logger;

        public DataPointController(ILogger<MapController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetDataPoints")]
        public async Task<IActionResult> GetDataPoints([FromQuery] DataPointSearchCriteria criteria)
        {
            if (criteria == null) { return new BadRequestResult(); }

            List<DataPoint> result = await new DataPointDataAccess().GetDataPoints(criteria);
            if (result != null)
            {
                return new OkObjectResult(result);
            }
            return new BadRequestResult();
        }

        [HttpGet(Name = "GetDataPoint")]
        public async Task<IActionResult> GetDataPoint([FromQuery] int? Id)
        {
            if (Id == null) { return new BadRequestResult(); }

            // Get Data Point
            DataPoint? result = (await new DataPointDataAccess().GetDataPoints(new DataPointSearchCriteria() { Id = Id })).FirstOrDefault();

            if (result == null) { return new NotFoundResult(); }

            // Attach Type
            DataPointType? type = (await new DataPointTypeDataAccess().GetDataPointTypes(new PagedSearchCriteria() { Id = result.TypeId })).FirstOrDefault();

            if (type == null) { return new OkObjectResult(result); }

            // Attach Type Parameters
            List<DataPointTypeParameter> parameters = (await new DataPointTypeParameterDataAccess().GetDataPointTypeParameters(type.Id)).ToList();
            type.TypeParameters = parameters;

            if (type.TypeParameters == null) { return new OkObjectResult(result); }

            // Attach Parameters
            result.Parameters = new List<DataPointParameter>();
            DataPointParameterDataAccess da = new DataPointParameterDataAccess();
            foreach (DataPointTypeParameter tp in type.TypeParameters)
            {
                if (tp.Id == null) { continue; }
                DataPointParameter p = await da.GetParameter<DataPointParameter>(result.Id, (int)tp.Id, tp.TypeValue);
                result.Parameters.Add(p);
            }

            return new OkObjectResult(result);
        }

        [HttpPost(Name = "PostDataPoint")]
        public async Task<IActionResult> PostDataPoint([FromBody] DataPoint data)
        {
            if (data == null) { return new BadRequestResult(); }

            if (await new DataPointDataAccess().PostDataPoint(data) != null)
            {

                return new OkResult();
            }

            return new BadRequestResult();
        }

        [HttpPut(Name = "PutDataPoint")]
        public async Task<IActionResult> PutDataPoint([FromBody] DataPoint data)
        {
            return null;
        }

        [HttpDelete(Name = "DeleteDataPoint")]
        public async Task<IActionResult> DeleteDataPoint([FromQuery] int? Id)
        {
            if (Id == null) { return new BadRequestResult(); }

            int result = await new DataPointDataAccess().DeleteDataPoint((int)Id);

            if (result > 0)
            {
                return new OkResult();
            }
            else if (result == 0)
            {
                return new NotFoundResult();
            }
            else
            {
                return new BadRequestResult();
            }
        }
    }
}
