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

namespace SardCoreAPI.Controllers.Map
{
    [ApiController]
    [Route("Library/[controller]/[action]")]
    public class MapController : GenericController
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

            Console.WriteLine(WorldLocation);

            List<m.Map> result = await new MapDataAccess().GetMaps(criteria, WorldInfo);
            if (result != null)
            {
                return new OkObjectResult(result);
            }
            return new BadRequestResult();
        }

        [HttpGet(Name = "GetMapCount")]
        public async Task<IActionResult> GetMapCount([FromQuery] MapSearchCriteria criteria)
        {
            if (criteria == null) { return new BadRequestResult(); }

            int result = (await new MapDataAccess().GetMaps(criteria, WorldInfo)).Count();
            return new OkObjectResult(result);
        }

        [Authorize(Roles = "Administrator,Editor")]
        [HttpPost(Name = "PostMap")]
        public async Task<IActionResult> PostMap([FromBody] m.Map data)
        {
            if (data == null) { return new BadRequestResult(); }

            int result = await new MapDataAccess().PostMap(data, WorldInfo);

            if (result != 0)
            {
                return new OkObjectResult(result);
            }

            return new BadRequestResult();
        }

        [Authorize(Roles = "Administrator,Editor")]
        [HttpPut(Name = "PutMap")]
        public async Task<IActionResult> PutMap([FromBody] m.Map data)
        {
            if (data == null) { return new BadRequestResult(); }

            int result = await new MapDataAccess().PutMap(data, WorldInfo);

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

        [Authorize(Roles = "Administrator,Editor")]
        [HttpDelete(Name = "DeleteMap")]
        public async Task<IActionResult> DeleteMap([FromQuery] int? Id)
        {
            if (Id == null) { return new BadRequestResult(); }

            int result = await new MapDataAccess().DeleteMap((int)Id, WorldInfo);

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
                byte[] result = await new MapDataAccess().GetMapIcon(id, WorldInfo);
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

        [Authorize(Roles = "Administrator,Editor")]
        [HttpPost(Name = "PostMapIcon")]
        public async Task<IActionResult> PostMapIcon([FromForm] ImagePostRequest file)
        {
            if (file == null || file.Data.Length == 0)
            {
                return new BadRequestResult();
            }

            byte[] bytes = await new FileHandler().FormToByteArray(file.Data);
            byte[] compressed = await new FileHandler().CompressImage(bytes, 256, 256);

            try
            {
                await new MapDataAccess().UploadMapIcon(compressed, file.Id, WorldInfo);
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