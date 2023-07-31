using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SardCoreAPI.DataAccess.Content;
using SardCoreAPI.DataAccess.Map;
using SardCoreAPI.Models.Content;
using SardCoreAPI.Models.Map.MapTile;
using SardCoreAPI.Utility.Map;

namespace SardCoreAPI.Controllers.Content
{
    [ApiController]
    [Route("Library/[controller]/[action]")]
    public class ImageController : GenericController
    {
        [HttpGet]
        public async Task<IActionResult> GetImage([FromQuery] ImageRequest request)
        {
            try
            {
                request.WorldInfo = WorldInfo;
                byte[] result = await new ImageDataAccess().GetImage(request);
                return new FileStreamResult(new MemoryStream(result), "image/png");
            }
            catch (FileNotFoundException e)
            {
                return new OkObjectResult(null);
            }
            catch (DirectoryNotFoundException e)
            {
                return new OkObjectResult(null);
            }
        }

        [Authorize(Roles = "Administrator,Editor")]
        [HttpPost]
        public async Task<IActionResult> PostImage([FromForm] ImagePostRequest request)
        {
            if (request == null || request.Data == null || request.Data.Length == 0)
            {
                return new BadRequestResult();
            }

            request.WorldInfo = WorldInfo;

            if (await new ImageDataAccess().PostImage(request) != 0)
            {
                await new ImageDataAccess().PutImageUrl(request);
                return new OkResult();
            }
            return new BadRequestResult();
        }

        [Authorize(Roles = "Administrator,Editor")]
        [HttpDelete]
        public async Task<IActionResult> DeleteImage([FromForm] ImageRequest request)
        {
            request.WorldInfo = WorldInfo;
            int result = await new ImageDataAccess().DeleteImage(request);
            if (result == 0)
            {
                return new NotFoundResult();
            }
            return new OkResult();
        }
    }
}
