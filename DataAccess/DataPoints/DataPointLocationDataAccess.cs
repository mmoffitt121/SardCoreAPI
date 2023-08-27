using Dapper;
using SardCoreAPI.Models.Common;
using SardCoreAPI.Models.DataPoints;
using SardCoreAPI.Models.Hub.Worlds;
using SardCoreAPI.Models.Map.Location;

namespace SardCoreAPI.DataAccess.DataPoints
{
    public class DataPointLocationDataAccess : GenericDataAccess
    {
        public async Task<List<DataPoint>> GetDataPointsFromLocationId(PagedSearchCriteria criteria, WorldInfo info)
        {
            string pageSettings = "";
            if (criteria.PageNumber != null && criteria.PageSize != null)
            {
                pageSettings = $"LIMIT {criteria.PageSize} OFFSET {(criteria.PageNumber - 1) * criteria.PageSize}";
            }

            string sql = $@"SELECT *
                FROM DataPointLocations dpl
                    LEFT JOIN DataPoints dp ON dpl.DataPointId = dp.Id
                /**where**/
                ORDER BY Name
                {pageSettings}
            ";

            SqlBuilder builder = new SqlBuilder();
            var template = builder.AddTemplate(sql);

            if (!string.IsNullOrEmpty(criteria.Query)) { builder.Where("Name LIKE CONCAT('%', IFNULL(@Query, ''), '%')"); }
            if (criteria.Id != null) { builder.Where("dpl.LocationId = @Id"); }

            return await Query<DataPoint>(template.RawSql, criteria, info);
        }

        public async Task<int> GetDataPointsFromLocationIdCount(PagedSearchCriteria criteria, WorldInfo info)
        {
            string sql = $@"SELECT COUNT(*)
                FROM DataPointLocations dpl
                    LEFT JOIN DataPoints dp ON dpl.DataPointId = dp.Id
                /**where**/
                ORDER BY Name
            ";

            SqlBuilder builder = new SqlBuilder();
            var template = builder.AddTemplate(sql);

            if (!string.IsNullOrEmpty(criteria.Query)) { builder.Where("Name LIKE CONCAT('%', IFNULL(@Query, ''), '%')"); }
            if (criteria.Id != null) { builder.Where("dpl.LocationId = @Id"); }

            return await QueryFirst<int>(template.RawSql, criteria, info);
        }

        public async Task<List<Location>> GetLocationsFromDataPointId(PagedSearchCriteria criteria, WorldInfo info)
        {
            string pageSettings = "";
            if (criteria.PageNumber != null && criteria.PageSize != null)
            {
                pageSettings = $"LIMIT {criteria.PageSize} OFFSET {(criteria.PageNumber - 1) * criteria.PageSize}";
            }

            string sql = $@"SELECT *
                FROM DataPointLocations dpl
                    LEFT JOIN Locations dp ON dpl.LocationId = dp.Id
                /**where**/
                ORDER BY Name
                {pageSettings}
            ";

            SqlBuilder builder = new SqlBuilder();
            var template = builder.AddTemplate(sql);

            if (!string.IsNullOrEmpty(criteria.Query)) { builder.Where("Name LIKE CONCAT('%', IFNULL(@Query, ''), '%')"); }
            if (criteria.Id != null) { builder.Where("dpl.DataPointId = @Id"); }

            return await Query<Location>(template.RawSql, criteria, info);
        }

        public async Task<int> GetLocationsFromDataPointIdCount(PagedSearchCriteria criteria, WorldInfo info)
        {
            string sql = $@"SELECT COUNT(*)
                FROM DataPointLocations dpl
                    LEFT JOIN Locations dp ON dpl.LocationId = dp.Id
                /**where**/
                ORDER BY Name
            ";

            SqlBuilder builder = new SqlBuilder();
            var template = builder.AddTemplate(sql);

            if (!string.IsNullOrEmpty(criteria.Query)) { builder.Where("Name LIKE CONCAT('%', IFNULL(@Query, ''), '%')"); }
            if (criteria.Id != null) { builder.Where("dpl.DataPointId = @Id"); }

            return await QueryFirst<int>(template.RawSql, criteria, info);
        }

        public async Task<int?> PostDataPointLocation(DataPointLocation data, WorldInfo info)
        {
            string sql = @"INSERT INTO DataPointLocations (LocationId, DataPointId) 
                VALUES (@LocationId, @DataPointId);";

            return (await QueryFirst<int?>(sql, data, info));
        }

        public async Task<int> DeleteDataPointLocation(DataPointLocation location, WorldInfo info)
        {
            string sql = @"DELETE FROM DataPointLocations WHERE LocationId = @LocationId AND DataPointId = @DataPointId;";

            return await Execute(sql, location, info);
        }
    }
}
