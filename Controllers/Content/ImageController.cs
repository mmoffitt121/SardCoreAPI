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
    public class ImageController
    {
        [HttpGet]
        public async Task<IActionResult> GetImage([FromQuery] ImageRequest request)
        {
            try
            {
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

        [HttpPost]
        public async Task<IActionResult> PostImage([FromForm] ImagePostRequest request)
        {
            if (request == null || request.Data == null || request.Data.Length == 0)
            {
                return new BadRequestResult();
            }

            if (await new ImageDataAccess().PostImage(request) != 0)
            {
                return new OkResult();
            }
            return new BadRequestResult();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteImage([FromForm] ImageRequest request)
        {
            int result = await new ImageDataAccess().DeleteImage(request);
            if (result == 0)
            {
                return new NotFoundResult();
            }
            return new OkResult();
        }
    }
}
