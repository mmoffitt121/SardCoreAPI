using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SardCoreAPI.Attributes.Security;
using SardCoreAPI.DataAccess.Map;
using SardCoreAPI.Models.Content;
using SardCoreAPI.Models.Map.LocationType;
using SardCoreAPI.Models.Map.MapLayer;
using SardCoreAPI.Services.Context;
using SardCoreAPI.Services.Maps;
using SardCoreAPI.Utility.DataAccess;
using SardCoreAPI.Utility.Error;
using SardCoreAPI.Utility.Files;
using SardCoreAPI.Utility.Map;

namespace SardCoreAPI.Controllers.Map
{
    [ApiController]
    [Route("Map/[controller]/[action]")]
    public class MapLayerController : GenericController
    {
        private readonly ILogger<MapController> _logger;
        private readonly IDataService data;
        private readonly IMapService mapService;

        public MapLayerController(ILogger<MapController> logger, IDataService dataService, IMapService mapService)
        {
            _logger = logger;
            data = dataService;
            this.mapService = mapService;
        }

        #region Map Layer
        [HttpGet(Name = "GetMapLayers")]
        [Resource("Library.Map.Read")]
        public async Task<IActionResult> GetMapLayers([FromQuery] MapLayerSearchCriteria criteria)
        {
            return await Handle(data.Context.MapLayer
                .Where(criteria.GetQuery())
                .Include(l => l.PersistentZoomLevels)
                .Paginate(criteria)
                .OrderBy(l => l.Name)
                .ToListAsync());
        }

        [HttpGet(Name = "GetMapLayersCount")]
        [Resource("Library.Map.Read")]
        public async Task<IActionResult> GetMapLayersCount([FromQuery] MapLayerSearchCriteria criteria)
        {
            return await Handle(data.Context.MapLayer
                .Where(criteria.GetQuery())
                .Include(l => l.PersistentZoomLevels)
                .CountAsync());
        }

        [HttpPost(Name = "PostMapLayer")]
        [Resource("Library.Map")]

        public async Task<IActionResult> PostMapLayer([FromBody] MapLayer layer)
        {
            return await Handle(mapService.PutMapLayer(layer));
        }

        [HttpPut(Name = "PutMapLayer")]
        [Resource("Library.Map")]
        public async Task<IActionResult> PutMapLayer([FromBody] MapLayer layer)
        {
            return await Handle(mapService.PutMapLayer(layer));
        }

        [HttpDelete(Name = "DeleteMapLayer")]
        [Resource("Library.Map")]
        public async Task<IActionResult> DeleteMapLayer([FromQuery] int Id)
        {
            return await Handle(mapService.DeleteMapLayer(Id));
        }
        #endregion
    }
}
