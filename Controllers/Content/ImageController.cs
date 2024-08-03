using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SardCoreAPI.Attributes.Security;
using SardCoreAPI.DataAccess.Content;
using SardCoreAPI.DataAccess.Map;
using SardCoreAPI.Models.Common;
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
        public async Task<IActionResult> GetImages([FromQuery] PagedSearchCriteria criteria)
        {
            return await Handle(_contentService.GetImages(criteria));
        }

        [HttpGet]
        public async Task<IActionResult> GetImageCount([FromQuery] PagedSearchCriteria criteria)
        {
            return await Handle(_contentService.GetImageCount(criteria));
        }

        [HttpGet]
        public async Task<IActionResult> Image([FromQuery] string id)
        {
            return await GetImage(_contentService.Image(id));
        }

        [HttpGet]
        public async Task<IActionResult> Thumbnail([FromQuery] string id)
        {
            return await GetImage(_contentService.Thumbnail(id));
        }

        private async Task<IActionResult> GetImage(Task<(byte[], string)> task)
        {
            (byte[], string) fileTuple = await task;
            return File(fileTuple.Item1, fileTuple.Item2);
        }

        [HttpPost]
        [Resource("Library.General")]
        public async Task<IActionResult> PostImage([FromForm] ImagePostRequest request)
        {
            return await Handle(_contentService.PostImage(request));
        }

        [HttpDelete]
        [Resource("Library.General")]
        public async Task<IActionResult> DeleteImage([FromForm] string id)
        {
            return await Handle(_contentService.DeleteImage(id));
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
