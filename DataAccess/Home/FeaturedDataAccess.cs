using SardCoreAPI.Models.DataPoints;
using SardCoreAPI.Models.Home;
using SardCoreAPI.Models.Hub.Worlds;

namespace SardCoreAPI.DataAccess.Home
{
    public class FeaturedDataAccess : GenericDataAccess
    {
        public async Task<List<DataPoint>> GetFeatured(WorldInfo worldInfo)
        {
            string sql = @"SELECT d.Id, d.Name, d.TypeId FROM Featured f
                    LEFT JOIN DataPoints d ON d.Id = f.DataPointId
               ORDER BY f.Id";

            return await Query<DataPoint>(sql, null, worldInfo);
        }

        public async Task<int> UpdateFeatured(List<Featured> data, WorldInfo worldInfo)
        {
            string sql = "INSERT INTO Featured (Id, DataPointId) VALUES (@Id, @DataPointId)";

            return await Execute(sql, data, worldInfo);
        }

        public async Task<int> PurgeFeatured(WorldInfo worldInfo)
        {
            string sql = "DELETE FROM Featured WHERE id = id;";

            return await Execute(sql, null, worldInfo);
        }
    }
}
