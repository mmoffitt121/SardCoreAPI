using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SardCoreAPI.DataAccess.Map;
using SardCoreAPI.Models.Common;
using SardCoreAPI.Models.Map.Location;
using SardCoreAPI.Models.Map.LocationType;
using SardCoreAPI.Models.Map.MapTile;
using SardCoreAPI.Utility.Map;
using System.Reflection.Emit;

namespace SardCoreAPI.Controllers.Map
{
    [ApiController]
    [Route("Library/[controller]/[action]")]
    public class LocationTypeController
    {
        private readonly ILogger<MapController> _logger;

        public LocationTypeController(ILogger<MapController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetLocationTypes")]
        public async Task<IActionResult> GetLocationTypes([FromQuery] PagedSearchCriteria criteria)
        {
            List<LocationType> locationTypes = await new LocationTypeDataAccess().GetLocationTypes(criteria);

            if (locationTypes != null)
            {
                return new OkObjectResult(locationTypes);
            }
            return new BadRequestResult();
        }


        [HttpGet]
        public async Task<IActionResult> GetLocationTypeCount([FromQuery] PagedSearchCriteria criteria)
        {
            List<LocationType> locationTypes = await new LocationTypeDataAccess().GetLocationTypes(criteria);

            if (locationTypes != null)
            {
                return new OkObjectResult(locationTypes.Count());
            }
            return new BadRequestResult();
        }


        [HttpGet(Name = "GetLocationType")]
        public async Task<IActionResult> GetLocationType(int? id)
        {
            if (id == null) { return new BadRequestResult(); }

            LocationType? locationType = (await new LocationTypeDataAccess().GetLocationTypes(new PagedSearchCriteria() { Id = id.Value })).FirstOrDefault();

            if (locationType != null)
            {
                return new OkObjectResult(locationType);
            }
            return new NotFoundResult();
        }

        [Authorize(Roles = "Administrator,Editor")]
        [HttpPost(Name = "PostLocationType")]
        public async Task<IActionResult> PostLocationType([FromBody] LocationType data)
        {
            if (data == null) { return new BadRequestResult(); }

            int result = await new LocationTypeDataAccess().PostLocationType(data);

            if (result != 0)
            {
                return new OkObjectResult(result);
            }

            return new BadRequestResult();
        }

        [Authorize(Roles = "Administrator,Editor")]
        [HttpPut(Name = "PutLocationType")]
        public async Task<IActionResult> PutLocationType([FromBody] LocationType data)
        {
            if (data == null) { return new BadRequestResult(); }

            int result = await new LocationTypeDataAccess().PutLocationType(data);

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

        [Authorize(Roles = "Administrator,Editor")]
        [HttpDelete]
        public async Task<IActionResult> DeleteLocationType(int? Id)
        {
            if (Id == null) { return new BadRequestResult(); }

            int result = await new LocationTypeDataAccess().DeleteLocationType((int)Id);

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
