using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using SardCoreAPI.DataAccess.Map;
using SardCoreAPI.Models.Map.MapTile;
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

        [HttpGet(Name = "GetTile")]
        public async Task<IActionResult> GetTile(int z, int x, int y, int layerId)
        {
            MapTile result = await new MapTileDataAccess().GetTile(z, x, y, layerId);
            return new FileStreamResult(new MemoryStream(result.Tile), "image/png");
        }

        [HttpPost]
        public async Task<IActionResult> UploadTile(IFormFile file, int rootZ, int rootX, int rootY, int layerId)
        {
            if (file == null || file.Length == 0)
            {
                return new BadRequestResult();
            }

            MapTile[] mapTiles = MapTileCutter.Slice(file, rootZ, rootX, rootY, layerId);

            if (await new MapTileDataAccess().PostTiles(mapTiles) != 0)
            {
                return new OkResult();
            }
            return new BadRequestResult();
        }
    }
}
