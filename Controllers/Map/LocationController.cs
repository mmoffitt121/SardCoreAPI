using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SardCoreAPI.DataAccess.Map;
using SardCoreAPI.Models.Map.Location;
using SardCoreAPI.Models.Map.LocationType;

namespace SardCoreAPI.Controllers.Map
{
    [ApiController]
    [Route("Library/[controller]/[action]")]
    public class LocationController
    {
        private readonly ILogger<MapController> _logger;

        public LocationController(ILogger<MapController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetLocations")]
        public IActionResult GetLocations([FromQuery] LocationSearchCriteria criteria)
        {
            if (criteria == null) { return new BadRequestResult(); }

            List<Location> result = new LocationDataAccess().GetLocations(criteria);
            if (result != null)
            {
                return new OkObjectResult(result);
            }
            return new BadRequestResult();
        }

        [HttpGet(Name = "GetLocation")]
        public async Task<ActionResult> GetLocation([FromQuery] int? Id)
        {
            Location result = await new LocationDataAccess().GetLocation(Id);
            if (result != null)
            {
                return new OkObjectResult(result);
            }
            return new NotFoundResult();
        }

        [Authorize(Roles = "Administrator,Editor")]
        [HttpGet]
        public async Task<IActionResult> GetLocationHeiarchy(int id, int depth)
        {
            List<Location> result = new List<Location>();
            LocationDataAccess dataAccess = new LocationDataAccess();
            int? currentId = id;
            for (int i = 0; i < depth; i++)
            {
                Location next = await dataAccess.GetLocation(currentId);
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

        [Authorize(Roles = "Administrator,Editor")]
        [HttpPost(Name = "PostLocation")]
        public IActionResult PostLocation([FromBody] Location location)
        {
            if (location == null) { return new BadRequestResult(); }

            if (new LocationDataAccess().PostLocation(location))
            {
                return new OkResult();
            }

            return new BadRequestResult();
        }

        [Authorize(Roles = "Administrator,Editor")]
        [HttpPut(Name = "PutLocation")]
        public async Task<IActionResult> PutLocation([FromBody] Location location)
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

        [Authorize(Roles = "Administrator,Editor")]
        [HttpDelete(Name = "DeleteLocation")]
        public async Task<IActionResult> DeleteLocation([FromQuery] int? Id)
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
        }
    }
}
