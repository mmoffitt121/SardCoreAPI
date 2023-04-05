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
        public FileResult GetTile(int z, int x, int y)
        {
            string path = x % 2 == 0 ? "C:\\Users\\Matthew\\Pictures\\Icons\\VabarikaMap.png" : "C:\\Users\\Matthew\\Pictures\\sun.jpg";
            return new FileStreamResult(new FileStream(path, FileMode.Open), "image/jpeg");
        }

        [HttpPost]
        public async Task<IActionResult> UploadTile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return new BadRequestResult();
            }

            MapTile[] mapTiles = MapTileCutter.Slice(file);

            using (var outputStream = new MemoryStream())
                for (int i = 0; i < mapTiles.Length; i++)
                {
                    mapTiles[i].Tile.Write($"Output {i}.png");
                }

            /*if (MapTileDataAccess.PostTiles(mapTiles))
            {
                return new OkResult();
            }
            Console.WriteLine(file.GetType());*/
            return new BadRequestResult();
        }
    }
}
