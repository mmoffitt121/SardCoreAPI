using Microsoft.AspNetCore.Mvc;

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

        /*[HttpGet(Name = "GetLocations")]
        public IEnumerable<Location> GetMaps([FromQuery] MapSearchCriteria criteria)
        {
            return MapCode.GetMaps(criteria);
        }*/

        /*[HttpGet(Name = "GetMap")]
        public Map? GetMap(int mapid)
        {
            return MapCode.GetMap(mapid);
        }

        [HttpPost(Name = "PostMap")]
        public string PostMap([FromBody] Map map)
        {
            if (MapCode.PostMap(map))
            {
                return "Operation was a success";
            }

            return "Operation failed";
        }

        [HttpPut(Name = "PutMap")]
        public string PutMaps(Map map)
        {
            if (MapCode.PutMap(map))
            {
                return "Operation was a success";
            }

            return "Operation failed";
        }

        [HttpDelete(Name = "DeleteMap")]
        public string DeleteMap(int id)
        {
            if (MapCode.DeleteMap(id))
            {
                return "Operation was a success";
            }

            return "Operation failed";
        }*/
    }
}
