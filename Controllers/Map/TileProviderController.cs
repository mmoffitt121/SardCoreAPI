using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using SardCoreAPI.DataAccess.Map;
using SardCoreAPI.Models.Map;
using SardCoreAPI.Utility.Map;

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
        public async Task<IActionResult> GetTile(int z, int x, int y)
        {
            MapTile result = new MapTileDataAccess().GetTile(z, x, y);
            return new FileStreamResult(new MemoryStream(result.Tile), "image/png");
        }

        [HttpPost]
        public async Task<IActionResult> UploadTile(IFormFile file, int rootZ, int rootX, int rootY)
        {
            if (file == null || file.Length == 0)
            {
                return new BadRequestResult();
            }

            MapTile[] mapTiles = MapTileCutter.Slice(file, rootZ, rootX, rootY);

            if (new MapTileDataAccess().PostTiles(mapTiles))
            {
                return new OkResult();
            }
            return new BadRequestResult();
        }
    }
}
