using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SardCoreAPI.DataAccess.Map;
using SardCoreAPI.Models.Hub.Worlds;
using SardCoreAPI.Models.Map;
using SardCoreAPI.Models.Map.MapLayer;
using SardCoreAPI.Services.Context;
using SardCoreAPI.Utility.DataAccess;
using SardCoreAPI.Utility.Error;

namespace SardCoreAPI.Services.Maps
{
    public interface IMapService
    {
        Task<int> PostMap(Map map);
        Task PutMap(Map map);
        Task DeleteMap(int id);
        Task PutMapLayer(MapLayer layer);
        Task DeleteMapLayer(int id);
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
            data.Context.MapLayer.Add(new MapLayer() { Name = "Base Layer", MapId = map.Id, IsBaseLayer = true, IsIconLayer = false });
            data.Context.MapLayer.Add(new MapLayer() { Name = "Icon Layer", MapId = map.Id, IsBaseLayer = true, IsIconLayer = true });
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
            List<MapLayer> mapLayers = data.Context.MapLayer.Where(m => m.MapId == id).ToList();
            Map map = data.Context.Map.Single(m => m.Id.Equals(id));

            data.Context.MapLayer.RemoveRange(mapLayers);
            data.Context.Map.Remove(map);
            await data.Context.SaveChangesAsync();
        }

        public async Task PutMapLayer(MapLayer layer)
        {
            MapLayer? existing = await data.Context.MapLayer.Include(l => l.PersistentZoomLevels).SingleOrDefaultAsync(l => l.Id.Equals(layer.Id));
            if (existing != null)
            {
                if (existing.PersistentZoomLevels != null)
                {
                    data.Context.PersistentZoomLevel.RemoveRange(existing.PersistentZoomLevels);
                }
                if (layer.PersistentZoomLevels != null)
                {
                    data.Context.PersistentZoomLevel.AddRange(layer.PersistentZoomLevels);
                }
                existing.Name = layer.Name;
                existing.Summary = layer.Summary;
                existing.MapId = layer.MapId;
                existing.IsBaseLayer = layer.IsBaseLayer;
                existing.IsIconLayer = layer.IsIconLayer;
                existing.IconURL = layer.IconURL;
                existing.PersistentZoomLevels = layer.PersistentZoomLevels;
                data.Context.MapLayer.Update(existing);
            }
            else
            {
                data.Context.MapLayer.Add(layer);
            }

            if (layer.IsBaseLayer == true)
            {
                List<MapLayer> mapLayers = await data.Context.MapLayer.Where(l => l.MapId.Equals(layer.MapId) && l.IsIconLayer.Equals(layer.IsIconLayer) && !l.Id.Equals(layer.Id)).ToListAsync();
                foreach (var item in mapLayers)
                {
                    item.IsBaseLayer = false;
                }
                data.Context.MapLayer.UpdateRange(mapLayers);
            }
            await data.Context.SaveChangesAsync();
        }

        public async Task DeleteMapLayer(int id)
        {
            await data.Context.MapLayer.Where(l => l.Id.Equals(id)).ExecuteDeleteAsync();
        }
    }
}
