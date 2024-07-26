using Microsoft.EntityFrameworkCore;
using SardCoreAPI.Models.Map;
using SardCoreAPI.Services.Context;

namespace SardCoreAPI.Services.Maps
{
    public interface IMapService
    {
        public Task<int> PostMap(Map map);
        public Task PutMap(Map map);
        public Task DeleteMap(int id);
    }

    public class MapService : IMapService
    {
        private readonly IDataService data;

        public MapService(IDataService data)
        {
            this.data = data;
        }

        public async Task<int> PostMap(Map map)
        {
            data.Context.Map.Add(map);
            await data.Context.SaveChangesAsync();
            return map.Id;
        }

        public async Task PutMap(Map map)
        {
            Map existing = await data.Context.Map.SingleAsync(m => m.Id.Equals(map.Id));
            existing.Name = map.Name;
            existing.Summary = map.Summary;
            existing.Loops = map.Loops;
            existing.DefaultZ = map.DefaultZ;
            existing.DefaultX = map.DefaultX;
            existing.DefaultY = map.DefaultY;
            existing.DefaultZ = map.DefaultZ;
            existing.MinZoom = map.MinZoom;
            existing.MaxZoom = map.MaxZoom;
            existing.IsDefault = map.IsDefault;
            existing.IconId = map.IconId;
            data.Context.Map.Update(existing);
            await data.Context.SaveChangesAsync();
        }

        public async Task DeleteMap(int id)
        {
            data.Context.Map.Where(m => m.Id.Equals(id)).ExecuteDelete();
        }
    }
}
