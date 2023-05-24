using Microsoft.AspNetCore.Mvc;
using SardCoreAPI.Controllers.Map;
using SardCoreAPI.DataAccess.DataPoint;
using SardCoreAPI.DataAccess.DataPoints;
using SardCoreAPI.DataAccess.Map;
using SardCoreAPI.Models.Common;
using SardCoreAPI.Models.DataPoints;
using SardCoreAPI.Models.DataPoints.DataPointParameters;
using SardCoreAPI.Models.Map.Location;

namespace SardCoreAPI.Controllers.DataPoint
{
    [ApiController]
    [Route("Library/[controller]/[action]")]
    public class DataPointTypeController
    {
        private readonly ILogger<MapController> _logger;

        public DataPointTypeController(ILogger<MapController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetDataPointTypes")]
        public async Task<IActionResult> GetDataPointTypes([FromQuery] PagedSearchCriteria criteria)
        {
            if (criteria == null) { return new BadRequestResult(); }

            List<DataPointType> result = await new DataPointTypeDataAccess().GetDataPointTypes(criteria);
            if (result != null)
            {
                return new OkObjectResult(result);
            }
            return new BadRequestResult();
        }

        [HttpGet(Name = "GetDataPointType")]
        public async Task<IActionResult> GetDataPointType([FromQuery] int? Id)
        {
            if (Id == null) { return new BadRequestResult(); }

            DataPointType? result = (await new DataPointTypeDataAccess().GetDataPointTypes(new PagedSearchCriteria() { Id = Id })).FirstOrDefault();

            if (result == null) { return new NotFoundResult(); }

            List<DataPointTypeParameter> parameters = (await new DataPointTypeParameterDataAccess().GetDataPointTypeParameters(result.Id)).ToList();
            result.TypeParameters = parameters;

            return new OkObjectResult(result);
        }

        [HttpPost(Name = "PostDataPointType")]
        public IActionResult PostDataPointType([FromBody] DataPointType data)
        {
            if (data == null) { return new BadRequestResult(); }

            if (new DataPointTypeDataAccess().PostDataPointType(data))
            {
                return new OkResult();
            }

            return new BadRequestResult();
        }
        /*
        [HttpPut(Name = "PutDataPointType")]
        public async Task<IActionResult> PutDataPointType([FromBody] Location location)
        {
            int result = await new LocationDataAccess().PutLocation(location);
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

        [HttpDelete(Name = "DeleteDataPointType")]
        public async Task<IActionResult> DeleteDataPointType([FromQuery] int? Id)
        {
            if (Id == null) { return new BadRequestResult(); }

            int result = await new LocationDataAccess().DeleteLocation((int)Id);

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
        }*/
    }
}
