using Microsoft.AspNetCore.Mvc;
using SardCoreAPI.DataAccess.Map;
using SardCoreAPI.Models.Map.MapTile;
using SardCoreAPI.Utility.Map;
using Microsoft.AspNetCore.SignalR;
using SardCoreAPI.Utility.Progress;
using Microsoft.AspNetCore.Authorization;
using SardCoreAPI.Attributes.Security;
using SardCoreAPI.Services.Maps;

namespace SardCoreAPI.Controllers.Map
{
    [ApiController]
    [Route("Map/[controller]/[action]")]
    public class TileProviderController : GenericController
    {
        private readonly ILogger<MapController> _logger;
        private readonly IMapTileService _mapTileService;

        public TileProviderController(ILogger<MapController> logger, IMapTileService mapTileService)
        {
            _logger = logger;
            _mapTileService = mapTileService;
        }

        [HttpGet(Name = "GetTile")]
        public async Task<IActionResult> GetTile(int z, int x, int y, int layerId, string worldLocation)
        {
            Response.Headers["Cache-Control"] = "public,max-age=" + 10000;
            return await _mapTileService.GetTile(z, x, y, layerId, worldLocation);
        }

        [HttpPost]
        [Resource("Library.Map")]
        public async Task<IActionResult> UploadTile([FromForm] TileUploadRequest request)
        {
            return await Handle(_mapTileService.PostTile(request));
        }
    }
}
