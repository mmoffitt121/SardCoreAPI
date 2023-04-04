using Microsoft.AspNetCore.Mvc;
using SardCoreAPI.Models;
using SardCoreAPI.CodeStore;
using SardCoreAPI.Models.Document.SearchCriteria;

namespace SardCoreAPI.Controllers.Map
{
    [ApiController]
    [Route("Library/[controller]/[action]")]
    public class MapController : ControllerBase
    {
        private readonly ILogger<MapController> _logger;

        public MapController(ILogger<MapController> logger)
        {
            _logger = logger;
        }

        /*[HttpGet(Name = "GetMaps")]
        public IEnumerable<Map> GetMaps([FromQuery] MapSearchCriteria criteria)
        {
            return MapCode.GetMaps(criteria);
        }

        [HttpGet(Name = "GetMap")]
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