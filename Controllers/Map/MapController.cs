using Microsoft.AspNetCore.Mvc;
using SardCoreAPI.DataAccess.DataPoints;
using SardCoreAPI.Models;
using SardCoreAPI.Models.DataPoints.DataPointParameters;
using SardCoreAPI.Models.DataPoints;
using SardCoreAPI.DataAccess.Map;
using m = SardCoreAPI.Models.Map;
using SardCoreAPI.Models.Map.MapTile;
using SardCoreAPI.Utility.Map;
using SardCoreAPI.Utility.Files;
using SardCoreAPI.Utility.Error;
using SardCoreAPI.Models.Map;
using SardCoreAPI.Models.Map.MapLayer;
using SardCoreAPI.Models.Content;
using Microsoft.AspNetCore.Authorization;
using SardCoreAPI.Utility.Auth;
using SardCoreAPI.Attributes.Security;
using SardCoreAPI.Services.Context;
using Microsoft.EntityFrameworkCore;
using SardCoreAPI.Utility.DataAccess;
using SardCoreAPI.Services.Maps;

namespace SardCoreAPI.Controllers.Map
{
    [ApiController]
    [Route("Library/[controller]/[action]")]
    public class MapController : GenericController
    {
        private readonly ILogger<MapController> _logger;
        private readonly IDataService data;
        private readonly IMapService mapService;

        public MapController(ILogger<MapController> logger, IDataService data, IMapService mapService)
        {
            _logger = logger;
            this.data = data;
            this.mapService = mapService;
        }

        #region Map

        [HttpGet(Name = "GetMaps")]
        [Resource("Library.Map.Read")]
        public async Task<IActionResult> GetMaps([FromQuery] MapSearchCriteria criteria)
        {
            return await Handle(data.Context.Map.Where(criteria.GetQuery()).Paginate(criteria).ToListAsync());
        }

        [HttpGet(Name = "GetMapCount")]
        [Resource("Library.Map.Read")]
        public async Task<IActionResult> GetMapCount([FromQuery] MapSearchCriteria criteria)
        {
            return await Handle(data.Context.Map.Where(criteria.GetQuery()).CountAsync());
        }

        [HttpPost(Name = "PostMap")]
        [Resource("Library.Map")]
        public async Task<IActionResult> PostMap([FromBody] m.Map map)
        {
            return await Handle(mapService.PostMap(map));
        }

        [HttpPut(Name = "PutMap")]
        [Resource("Library.Map")]
        public async Task<IActionResult> PutMap([FromBody] m.Map map)
        {
            return await Handle(mapService.PutMap(map));
        }

        [HttpDelete(Name = "DeleteMap")]
        [Resource("Library.Map")]

        public async Task<IActionResult> DeleteMap([FromQuery] int Id)
        {
            return await Handle(mapService.DeleteMap(Id));
        }
        #endregion
    }
}