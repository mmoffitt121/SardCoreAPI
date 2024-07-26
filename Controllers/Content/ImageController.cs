using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SardCoreAPI.Attributes.Security;
using SardCoreAPI.DataAccess.Content;
using SardCoreAPI.DataAccess.Map;
using SardCoreAPI.Models.Content;
using SardCoreAPI.Models.Hub.Worlds;
using SardCoreAPI.Models.Map.MapTile;
using SardCoreAPI.Services.Content;
using SardCoreAPI.Utility.Map;

namespace SardCoreAPI.Controllers.Content
{
    [ApiController]
    [Route("Library/[controller]/[action]")]
    public class ImageController : GenericController
    {
        private IContentService _contentService;

        public ImageController(IContentService contentService) 
        { 
            _contentService = contentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetImage([FromQuery] ImageRequest request)
        {
            try
            {
                request.WorldInfo = GetWorldInfo(request);
                byte[] result = await _contentService.GetImage(request);
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
        [Resource("Library.General")]
        public async Task<IActionResult> PostImage([FromForm] ImagePostRequest request)
        {
            if (request == null || request.Data == null || request.Data.Length == 0)
            {
                return new BadRequestResult();
            }

            request.WorldInfo = GetWorldInfo(request);
            string id = await _contentService.PostImage(request);
            await _contentService.PutImageUrl(request);

            return Ok(id);
        }

        [HttpDelete]
        [Resource("Library.General")]
        public async Task<IActionResult> DeleteImage([FromForm] ImageRequest request)
        {
            request.WorldInfo = GetWorldInfo(request);
            int result = await _contentService.DeleteImage(request);
            if (result == 0)
            {
                return new NotFoundResult();
            }
            return new OkResult();
        }

        private WorldInfo GetWorldInfo(ImageRequest request)
        {
            if (!string.IsNullOrEmpty(request.WorldPath))
            {
                return new WorldInfo(request.WorldPath);
            }
            return WorldInfo;
        }
    }
}
