using ImageMagick;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using SardCoreAPI.DataAccess.Map;
using SardCoreAPI.Models.Common;
using SardCoreAPI.Models.Hub.Worlds;
using SardCoreAPI.Models.Map.MapTile;
using SardCoreAPI.Services.Content;
using SardCoreAPI.Services.Context;
using SardCoreAPI.Services.Tasks;
using SardCoreAPI.Services.WorldContext;
using SardCoreAPI.Utility.Files;
using SardCoreAPI.Utility.Map;

namespace SardCoreAPI.Services.Maps
{
    public interface IMapTileService
    {
        Task<MessageResponse> PostTile(TileUploadRequest request);
        Task<FileStreamResult> GetTile(int z, int x, int y, int layerId, string worldLocation);
    }

    public class MapTileService : IMapTileService
    {
        IServiceScopeFactory serviceScopeFactory;
        TaskService taskService;
        IContentService contentService;
        IWorldInfoService worldInfoService;
        IDataService data;

        public MapTileService(IServiceScopeFactory serviceScopeFactory, TaskService taskService, IContentService contentService, IWorldInfoService worldInfoService, IDataService data)
        {
            this.serviceScopeFactory = serviceScopeFactory;
            this.taskService = taskService;
            this.contentService = contentService;
            this.worldInfoService = worldInfoService;
            this.data = data;
        }

        public async Task<MessageResponse> PostTile([FromForm] TileUploadRequest request)
        {
            string id = (await contentService.PostImage(new Models.Content.ImagePostRequest()
            {
                Data = request.Data,
                Description = $"Map Upload for layer {request.LayerId} at {request.X} {request.Y} {request.Z}"
            })).Id;

            taskService.Schedule(new PutMapTilesTask(worldInfoService.WorldLocation, id, request));

            return new MessageResponse("Map tile upload scheduled successfully.");
        }

        public async Task<FileStreamResult> GetTile(int z, int x, int y, int layerId, string worldLocation)
        {
            await data.StartUsingWorldContext(new WorldInfo(worldLocation));
            try
            {
                MapTile? tile = data.Context.MapTile.Where(tile => tile.Z.Equals(z) && tile.X.Equals(x) && tile.Y.Equals(y) && tile.LayerId.Equals(layerId)).SingleOrDefault();
                if (tile == null)
                {
                    return new FileStreamResult(new MemoryStream(Array.Empty<byte>()), "image/png");
                }
                else
                {
                    byte[] bytes = await new FileHandler().LoadImage(ImagePath(new WorldInfo(worldLocation)) + tile.FileName);
                    return new FileStreamResult(new MemoryStream(bytes), "image/png");
                }
            }
            finally
            {
                await data.EndUsingWorldContext();
            }
        }

        private string ImagePath(WorldInfo info)
        {
            return "./storage/" + info.WorldLocation + "/maptiles/";
        }
    }
}
