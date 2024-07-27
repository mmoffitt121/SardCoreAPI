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
            return await Handle(_contentService.GetImage(request));
        }

        [HttpPost]
        [Resource("Library.General")]
        public async Task<IActionResult> PostImage([FromForm] ImagePostRequest request)
        {
            return await Handle(_contentService.PostImage(request));
        }

        [HttpDelete]
        [Resource("Library.General")]
        public async Task<IActionResult> DeleteImage([FromForm] int id)
        {
            return Handle(await _contentService.DeleteImage(id));
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
