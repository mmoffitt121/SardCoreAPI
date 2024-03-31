using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SardCoreAPI.Attributes.Security;
using SardCoreAPI.DataAccess.Map;
using SardCoreAPI.Models.Map.Location;
using SardCoreAPI.Models.Map.LocationType;

namespace SardCoreAPI.Controllers.Map
{
    [ApiController]
    [Route("Library/[controller]/[action]")]
    public class LocationController : GenericController
    {
        private readonly ILogger<MapController> _logger;

        public LocationController(ILogger<MapController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetLocations")]
        [Resource("Library.Location.Read")]
        public async Task<IActionResult> GetLocations([FromQuery] LocationSearchCriteria criteria)
        {
            if (criteria == null) { return new BadRequestResult(); }

            List<Location> result = await new LocationDataAccess().GetLocations(criteria, WorldInfo);
            if (result != null)
            {
                return new OkObjectResult(result);
            }
            return new BadRequestResult();
        }

        [HttpGet]
        [Resource("Library.Location.Read")]
        public async Task<IActionResult> GetLocationsCount([FromQuery] LocationSearchCriteria criteria)
        {
            if (criteria == null) { return new BadRequestResult(); }

            criteria.PageNumber = null;
            criteria.PageSize = null;

            int result = (await new LocationDataAccess().GetLocations(criteria, WorldInfo)).Count();
            return new OkObjectResult(result);
        }

        [HttpGet(Name = "GetLocation")]
        [Resource("Library.Location.Read")]
        public async Task<ActionResult> GetLocation([FromQuery] int? Id)
        {
            Location result = await new LocationDataAccess().GetLocation(Id, WorldInfo);
            if (result != null)
            {
                return new OkObjectResult(result);
            }
            return new NotFoundResult();
        }

        [HttpGet]
        [Resource("Library.Location.Read")]
        public async Task<IActionResult> GetLocationHeiarchy(int id, int depth)
        {
            List<Location> result = new List<Location>();
            LocationDataAccess dataAccess = new LocationDataAccess();
            int? currentId = id;
            for (int i = 0; i < depth; i++)
            {
                Location next = await dataAccess.GetLocation(currentId, WorldInfo);
                if (next == null) { break; }
                result.Add(next);
                currentId = next.ParentId;
                if (currentId == null) { break; }
            }
            if (result != null)
            {
                return new OkObjectResult(result);
            }
            return new BadRequestResult();
        }

        [HttpPost(Name = "PostLocation")]
        [Resource("Library.Location")]
        public async Task<IActionResult> PostLocation([FromBody] Location location)
        {
            if (location == null) { return new BadRequestResult(); }

            if (await new LocationDataAccess().PostLocation(location, WorldInfo))
            {
                return new OkResult();
            }

            return new BadRequestResult();
        }

        [HttpPut(Name = "PutLocation")]
        [Resource("Library.Location")]
        public async Task<IActionResult> PutLocation([FromBody] Location location)
        {
            int result = await new LocationDataAccess().PutLocation(location, WorldInfo);
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

        [HttpDelete(Name = "DeleteLocation")]
        [Resource("Library.Location")]
        public async Task<IActionResult> DeleteLocation([FromQuery] int? Id)
        {
            if (Id == null) { return new BadRequestResult(); }

            int result = await new LocationDataAccess().DeleteLocation((int)Id, WorldInfo);

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
