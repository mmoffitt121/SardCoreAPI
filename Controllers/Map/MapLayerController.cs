using Microsoft.AspNetCore.Mvc;
using SardCoreAPI.DataAccess.Map;
using SardCoreAPI.Models.Common;
using SardCoreAPI.Models.Map.LocationType;
using SardCoreAPI.Models.Map.MapLayer;
using SardCoreAPI.Utility.Map;

namespace SardCoreAPI.Controllers.Map
{
    [ApiController]
    [Route("Map/[controller]/[action]")]
    public class MapLayerController
    {
        private readonly ILogger<MapController> _logger;

        public MapLayerController(ILogger<MapController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetMapLayers")]
        public IActionResult GetMapLayers([FromQuery] DatedSearchCriteria criteria)
        {
            List<MapLayer>? mapLayers = new MapLayerDataAccess().GetMapLayers(criteria);

            if (mapLayers != null)
            {
                return new OkObjectResult(mapLayers);
            }
            return new BadRequestResult();
        }

        [HttpGet(Name = "GetMapLayer")]
        public IActionResult GetMapLayer(int id)
        {
            MapLayer? mapLayer = new MapLayerDataAccess().GetMapLayer(id);

            if (mapLayer != null)
            {
                return new OkObjectResult(mapLayer);
            }
            return new NotFoundResult();
        }

        [HttpPost]
        public IActionResult PostMapLayer(MapLayer layer)
        {
            if (layer == null || string.IsNullOrEmpty(layer.Name))
            {
                return new BadRequestResult();
            }

            if (new MapLayerDataAccess().PostMapLayer(layer))
            {
                return new OkResult();
            }
            return new BadRequestResult();
        }
    }
}
