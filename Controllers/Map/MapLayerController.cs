﻿using Microsoft.AspNetCore.Mvc;
using SardCoreAPI.DataAccess.Map;
using SardCoreAPI.Models.Common;
using SardCoreAPI.Models.Map.LocationType;
using SardCoreAPI.Models.Map.MapLayer;
using SardCoreAPI.Utility.Error;
using SardCoreAPI.Utility.Files;
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

        #region Map Layer
        [HttpGet(Name = "GetMapLayers")]
        public async Task<IActionResult> GetMapLayers([FromQuery] MapLayerSearchCriteria criteria)
        {
            List<MapLayer>? mapLayers = await new MapLayerDataAccess().GetMapLayers(criteria);

            if (mapLayers != null)
            {
                return new OkObjectResult(mapLayers);
            }
            return new BadRequestResult();
        }

        [HttpGet(Name = "GetMapLayersCount")]
        public async Task<IActionResult> GetMapLayersCount([FromQuery] MapLayerSearchCriteria criteria)
        {
            List<MapLayer>? mapLayers = await new MapLayerDataAccess().GetMapLayers(criteria);

            if (mapLayers != null)
            {
                return new OkObjectResult(mapLayers.Count());
            }
            return new BadRequestResult();
        }

        [HttpPost(Name = "PostMapLayer")]
        public async Task<IActionResult> PostMapLayer(MapLayer layer)
        {
            if (layer == null || string.IsNullOrEmpty(layer.Name))
            {
                return new BadRequestResult();
            }

            int id = await new MapLayerDataAccess().PostMapLayer(layer);

            if (id > 0)
            {
                return new OkObjectResult(id);
            }
            return new BadRequestResult();
        }

        [HttpPut(Name = "PutMapLayer")]
        public async Task<IActionResult> PutMapLayer([FromBody] MapLayer data)
        {
            if (data == null) { return new BadRequestResult(); }

            int result = await new MapLayerDataAccess().PutMapLayer(data);

            if (result > 0)
            {
                return new OkResult();
            }
            else if (result == 0)
            {
                return new NotFoundResult();
            }
            else
            {
                return new BadRequestResult();
            }
        }

        [HttpDelete(Name = "DeleteMapLayer")]
        public async Task<IActionResult> DeleteMapLayer([FromQuery] int? Id)
        {
            if (Id == null) { return new BadRequestResult(); }

            int result = await new MapLayerDataAccess().DeleteMapLayer((int)Id);

            if (result > 0)
            {
                return new OkResult();
            }
            else if (result == 0)
            {
                return new NotFoundResult();
            }
            else
            {
                return new BadRequestResult();
            }
        }
        #endregion

        #region Map Layer Icon
        [HttpGet(Name = "GetMapLayerIcon")]
        public async Task<IActionResult> GetMapLayerIcon(int id)
        {
            try
            {
                byte[] result = await new MapLayerDataAccess().GetMapLayerIcon(id);
                return new FileStreamResult(new MemoryStream(result), "image/png");
            }
            catch (FileNotFoundException ex)
            {
                return new OkObjectResult(null);
            }
            catch (DirectoryNotFoundException ex)
            {
                return new OkObjectResult(null);
            }
            catch (IOException ex)
            {
                return ex.Handle();
            }
            catch (Exception ex)
            {
                return new BadRequestResult();
            }
        }

        [HttpPost(Name = "PostMapLayerIcon")]
        public async Task<IActionResult> PostMapLayerIcon([FromForm] ImageUploadRequest file)
        {
            if (file == null || file.Data.Length == 0)
            {
                return new BadRequestResult();
            }

            byte[] bytes = await new FileHandler().FormToByteArray(file.Data);
            byte[] compressed = await new FileHandler().CompressImage(bytes, 256, 256);

            try
            {
                await new MapLayerDataAccess().PostMapLayerIcon(compressed, file.Id);
                return new OkResult();
            }
            catch (IOException ex)
            {
                return ex.Handle();
            }
        }
        #endregion
    }
}
