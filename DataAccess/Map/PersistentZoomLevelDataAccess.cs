using MySqlConnector;
using Dapper;
using SardCoreAPI.Models.Map.Location;
using SardCoreAPI.Models.Map.LocationType;
using SardCoreAPI.Models.Hub.Worlds;
using static Dapper.SqlBuilder;
using SardCoreAPI.Models.Map.MapLayer;

namespace SardCoreAPI.DataAccess.Map
{
    public class PersistentZoomLevelDataAccess : GenericDataAccess
    { 
        public async Task<List<PersistentZoomLevel>> Get(int layerId, WorldInfo info)
        {
            string sql = $@"SELECT * FROM PersistentZoomLevels WHERE MapLayerId = @layerId";
            return await Query<PersistentZoomLevel>(sql, new { layerId }, info);
        }

        public async Task<bool> Post(PersistentZoomLevel data, WorldInfo info)
        {
            string sql = @"INSERT INTO PersistentZoomLevels (Zoom, MapLayerId) VALUES (@Zoom, @MapLayerId)";
            return await Execute(sql, data, info) > 0;
        }

        public async Task<int> Delete(int layerId, WorldInfo info)
        {
            string sql = @"DELETE FROM PersistentZoomLevels WHERE MapLayerId = @LayerId;";
            return await Execute(sql, new { layerId }, info);
        }
    }
}
