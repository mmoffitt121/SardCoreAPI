using Microsoft.AspNetCore.Mvc;
using SardCoreAPI.DataAccess.Content;
using SardCoreAPI.DataAccess.Map;
using SardCoreAPI.Models.Content;
using SardCoreAPI.Models.Map.MapTile;
using SardCoreAPI.Utility.Map;

namespace SardCoreAPI.Controllers.Content
{
    [ApiController]
    [Route("Map/[controller]/[action]")]
    public class ImageController
    {
        /*[HttpGet]
        public async Task<IActionResult> GetImage()
        {
            MapTile result = await new MapTileDataAccess().GetTile();
            return new FileStreamResult(new MemoryStream(result.Tile), "image/png");
        }*/

        [HttpPost]
        public async Task<IActionResult> PostImage([FromForm] ImageRequest request)
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
        public async Task<IActionResult> DeleteImage(int z, int x, int y, int layerId)
        {
            int result = await new MapTileDataAccess().DeleteTile(z, x, y, layerId);
            if (result == 0)
            {
                return new NotFoundResult();
            }
            return new OkResult();
        }
    }
}
