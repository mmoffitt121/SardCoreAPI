using Microsoft.AspNetCore.Mvc;
using SardCoreAPI.Attributes.Security;
using SardCoreAPI.Models.Common;
using SardCoreAPI.Models.Content;
using SardCoreAPI.Models.Hub.Worlds;
using SardCoreAPI.Services.Content;
using SardCoreAPI.Services.Context;
using SardCoreAPI.Services.Security;

namespace SardCoreAPI.Controllers.Content
{
    [ApiController]
    [Route("Library/[controller]/[action]")]
    public class ImageController : GenericController
    {
        private IContentService _contentService;
        private IDataService data;

        public ImageController(IContentService contentService, IDataService data) 
        { 
            _contentService = contentService;
            this.data = data;
        }

        [HttpGet]
        [Resource("Library.General.Read")]
        public async Task<IActionResult> GetImages([FromQuery] PagedSearchCriteria criteria)
        {
            return await Handle(_contentService.GetImages(criteria));
        }

        [HttpGet]
        [Resource("Library.General.Read")]
        public async Task<IActionResult> GetImageCount([FromQuery] PagedSearchCriteria criteria)
        {
            return await Handle(_contentService.GetImageCount(criteria));
        }

        [HttpGet]
        [Resource("Library.General.Read")]
        public async Task<IActionResult> Image([FromQuery] string id)
        {
            Response.Headers["Cache-Control"] = "public,max-age=" + 10000;
            return await GetImage(_contentService.Image(id));
        }

        [HttpGet]
        [Resource("Library.General.Read")]
        public async Task<IActionResult> Thumbnail([FromQuery] string id)
        {
            Response.Headers["Cache-Control"] = "public,max-age=" + 10000;
            return await GetImage(_contentService.Thumbnail(id));
        }

        [HttpGet]
        public async Task<IActionResult> Icon([FromQuery] string id, [FromQuery] string world)
        {
            Response.Headers["Cache-Control"] = "public,max-age=" + 10000;
            await data.StartUsingWorldContext(new WorldInfo(world));
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
        public async Task<IActionResult> DeleteImage([FromQuery] string id)
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
