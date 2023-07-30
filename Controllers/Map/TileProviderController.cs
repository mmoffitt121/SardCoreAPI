using Microsoft.AspNetCore.Mvc;
using SardCoreAPI.DataAccess.Map;
using SardCoreAPI.Models.Map.MapTile;
using SardCoreAPI.Utility.Map;
using Microsoft.AspNetCore.SignalR;
using SardCoreAPI.Utility.Progress;
using Microsoft.AspNetCore.Authorization;

namespace SardCoreAPI.Controllers.Map
{
    [ApiController]
    [Route("Map/[controller]/[action]")]
    public class TileProviderController
    {
        private readonly ILogger<MapController> _logger;
        private readonly IHubContext<ProgressManager> _progressHubContext;

        public TileProviderController(ILogger<MapController> logger, IHubContext<ProgressManager> hubContext)
        {
            _logger = logger;
            _progressHubContext = hubContext;
        }

        [HttpGet(Name = "GetTile")]
        public async Task<IActionResult> GetTile(int z, int x, int y, int layerId)
        {
            MapTile result = await new MapTileDataAccess().GetTile(z, x, y, layerId);
            return new FileStreamResult(new MemoryStream(result.Tile), "image/png");
        }

        [Authorize(Roles = "Administrator,Editor")]
        [HttpPost]
        public async Task<IActionResult> UploadTile([FromForm] TileUploadRequest request)
        {
            if (request == null || request.Data == null || request.Data.Length == 0)
            {
                return new BadRequestResult();
            }

            await _progressHubContext.Clients.All.SendAsync("ProgressUpdate", 10, "Slicing tiles...");

            MapTile[] mapTiles = await MapTileCutter.Slice(request.Data, request.Z, request.X, request.Y, request.LayerId, _progressHubContext);

            if (request.ReplaceRoot != null && !(bool)request.ReplaceRoot)
            {
                mapTiles = mapTiles.Where(m => !m.Equals(new MapTile() { Z = request.Z, X = request.X, Y = request.Y, LayerId = request.LayerId })).ToArray();
            }

            await _progressHubContext.Clients.All.SendAsync("ProgressUpdate", 95, "Comparing Tiles...");

            // Remove tiles that already exist if option is set.
            if (request.ReplaceMode != null && request.ReplaceMode.Equals("fill"))
            {
                MapTile[] existing = await new MapTileDataAccess().GetTiles(request.Z, request.X, request.Y, mapTiles.Last().Z, request.LayerId);
                mapTiles = mapTiles.Where(m => !existing.Contains(m)).ToArray();
                if (mapTiles.Length == 0)
                {
                    return new OkResult();
                }
            }

            await _progressHubContext.Clients.All.SendAsync("ProgressUpdate", 98, "Saving Tiles...");

            if (await new MapTileDataAccess().PostTiles(mapTiles) != 0)
            {
                return new OkResult();
            }
            return new BadRequestResult();
        }

        [Authorize(Roles = "Administrator,Editor")]
        [HttpDelete]
        public async Task<IActionResult> DeleteTile(int z, int x, int y, int layerId)
        {
            int result = await new MapTileDataAccess().DeleteTile(z, x, y, layerId);
            if (result == 0)
            {
                return new NotFoundResult();
            }
            return new OkResult();
        }

        [Authorize(Roles = "Administrator,Editor")]
        [HttpDelete]
        public async Task<IActionResult> DeleteTilesOfLayer(int layerId)
        {
            int result = await new MapTileDataAccess().DeleteTiles(layerId);
            if (result == 0)
            {
                return new NotFoundResult();
            }
            return new OkResult();
        }

        [Authorize(Roles = "Administrator,Editor")]
        [HttpDelete]
        public async Task<IActionResult> DeleteTilesOfMap(int mapId)
        {
            int result = await new MapTileDataAccess().DeleteTilesOfMap(mapId);
            if (result == 0)
            {
                return new NotFoundResult();
            }
            return new OkResult();
        }
    }
}
