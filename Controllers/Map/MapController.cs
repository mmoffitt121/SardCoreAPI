using Microsoft.AspNetCore.Mvc;
using SardCoreAPI.DataAccess.DataPoints;
using SardCoreAPI.Models;
using SardCoreAPI.Models.Common;
using SardCoreAPI.Models.DataPoints.DataPointParameters;
using SardCoreAPI.Models.DataPoints;
using SardCoreAPI.DataAccess.Map;
using m = SardCoreAPI.Models.Map;
using SardCoreAPI.Models.Map.MapTile;
using SardCoreAPI.Utility.Map;
using SardCoreAPI.Utility.Files;
using SardCoreAPI.Utility.Error;
using SardCoreAPI.Models.Map;

namespace SardCoreAPI.Controllers.Map
{
    [ApiController]
    [Route("Library/[controller]/[action]")]
    public class MapController : ControllerBase
    {
        private readonly ILogger<MapController> _logger;

        public MapController(ILogger<MapController> logger)
        {
            _logger = logger;
        }

        #region Map

        [HttpGet(Name = "GetMaps")]
        public async Task<IActionResult> GetMaps([FromQuery] MapSearchCriteria criteria)
        {
            if (criteria == null) { return new BadRequestResult(); }

            List<m.Map> result = await new MapDataAccess().GetMaps(criteria);
            if (result != null)
            {
                return new OkObjectResult(result);
            }
            return new BadRequestResult();
        }

        [HttpPost(Name = "PostMap")]
        public async Task<IActionResult> PostMap([FromBody] m.Map data)
        {
            if (data == null) { return new BadRequestResult(); }

            int result = await new MapDataAccess().PostMap(data);

            if (result != 0)
            {
                return new OkObjectResult(result);
            }

            return new BadRequestResult();
        }

        [HttpPut(Name = "PutMap")]
        public async Task<IActionResult> PutMap([FromBody] m.Map data)
        {
            if (data == null) { return new BadRequestResult(); }

            int result = await new MapDataAccess().PutMap(data);

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

        [HttpDelete(Name = "DeleteMap")]
        public async Task<IActionResult> DeleteMap([FromQuery] int? Id)
        {
            if (Id == null) { return new BadRequestResult(); }

            int result = await new MapDataAccess().DeleteMap((int)Id);

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

        #region Map Icon
        [HttpGet(Name = "GetMapIcon")]
        public async Task<IActionResult> GetMapIcon(int id)
        {
            try
            {
                byte[] result = await new MapDataAccess().GetMapIcon(id);
                return new FileStreamResult(new MemoryStream(result), "image/png");
            }
            catch (IOException ex)
            {
                return ex.Handle();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Other exception");
                return new BadRequestResult();
            }
            
        }

        [HttpPost(Name = "PostMapIcon")]
        public async Task<IActionResult> PostMapIcon(IFormFile file, int id)
        {
            if (file == null || file.Length == 0)
            {
                return new BadRequestResult();
            }

            byte[] bytes = await new FileHandler().FormToByteArray(file);
            byte[] compressed = await new FileHandler().CompressImage(bytes, 256, 256);

            try
            {
                await new MapDataAccess().UploadMapIcon(compressed, id);
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