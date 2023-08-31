using Dapper;
using SardCoreAPI.Models.Hub.Worlds;
using SardCoreAPI.Models.Map.MapLayer;
using SardCoreAPI.Models.Map.Region;
using SardCoreAPI.Utility.Files;

namespace SardCoreAPI.DataAccess.Map
{
    public class RegionDataAccess : GenericDataAccess
    {
        public async Task<List<Region>> GetRegions(RegionSearchCriteria criteria, WorldInfo info)
        {
            string pageSettings = "";
            if (criteria.PageNumber != null && criteria.PageSize != null)
            {
                pageSettings = $"LIMIT {criteria.PageSize} OFFSET {(criteria.PageNumber) * criteria.PageSize}";
            }

            string sql = $@"SELECT *
                    FROM Regions 
                    /**where**/ 
                    ORDER BY Name
                    {pageSettings}";

            SqlBuilder builder = new SqlBuilder();
            var template = builder.AddTemplate(sql);

            if (!string.IsNullOrEmpty(criteria.Query)) { builder.Where("Name LIKE CONCAT('%', IFNULL(@Query, ''), '%')"); }
            if (criteria.Id != null) { builder.Where("Id = @Id"); }
            if (criteria.LocationId != null) { builder.Where("LocationId = @LocationId"); }

            return await Query<Region>(template.RawSql, criteria, info);
        }

        public async Task<int> PostRegion(Region region, WorldInfo info)
        {
            string sql = @"INSERT INTO Regions (LocationId, Name, Shape, ShowByDefault) 
                    VALUES (@LocationId, @Name, @Shape, @ShowByDefault);

                SELECT LAST_INSERT_ID();
            ";

            return await QueryFirst<int>(sql, region, info);
        }

        public async Task<int> PutRegion(Region region, WorldInfo info)
        {
            string sql = @"UPDATE Regions SET
                    LocationId = @LocationId,
                    Name = @Name,
                    Shape = @Shape,
                    ShowByDefault = @ShowByDefault
                WHERE Id = @Id;
            ";

            return await Execute(sql, region, info);
        }

        public async Task<int> DeleteRegion(int Id, WorldInfo info)
        {
            string sql = @"DELETE FROM Regions
                WHERE
                    Id = @Id;
            ";

            return await Execute(sql, new { Id }, info);
        }
    }
}
