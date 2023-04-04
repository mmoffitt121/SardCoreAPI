using Microsoft.AspNetCore.Mvc;

namespace SardCoreAPI.Controllers.Map
{
    [ApiController]
    [Route("Map/[controller]/[action]")]
    public class TileProviderController
    {
        private readonly ILogger<MapController> _logger;

        public TileProviderController(ILogger<MapController> logger)
        {
            _logger = logger;
        }

        /*[HttpGet(Name = "GetMaps")]
        public IEnumerable<Map> GetMaps([FromQuery] MapSearchCriteria criteria)
        {
            return MapCode.GetMaps(criteria);
        }*/

        [HttpGet(Name = "GetTile")]
        public FileResult GetTile(int z, int x, int y)
        {
            string path = x % 2 == 0 ? "C:\\Users\\Matthew\\Pictures\\Icons\\VabarikaMap.png" : "C:\\Users\\Matthew\\Pictures\\sun.jpg";
            return new FileStreamResult(new FileStream(path, FileMode.Open), "image/jpeg");
        }
    }
}
