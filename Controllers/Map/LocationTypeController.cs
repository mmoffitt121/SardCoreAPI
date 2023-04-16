using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SardCoreAPI.DataAccess.Map;
using SardCoreAPI.Models.Document.SearchCriteria;
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
        public IActionResult GetLocationTypes(string? query)
        {
            List<LocationType> locationTypes = new LocationTypeDataAccess().GetLocationTypes(query);

            if (locationTypes != null)
            {
                return new OkObjectResult(locationTypes);
            }
            return new BadRequestResult();
        }

        [HttpGet(Name = "GetLocationType")]
        public IActionResult GetLocationType(int? id)
        {
            if (id == null) { return new BadRequestResult(); }

            LocationType? locationType = new LocationTypeDataAccess().GetLocationType(id.Value);

            if (locationType != null)
            {
                return new OkObjectResult(locationType);
            }
            return new NotFoundResult();
        }
    }
}
