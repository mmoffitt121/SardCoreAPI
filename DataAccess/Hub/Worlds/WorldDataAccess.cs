using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SardCoreAPI.Models.Hub.Worlds;
using SardCoreAPI.Models.Map;

namespace SardCoreAPI.DataAccess.Hub.Worlds
{
    public class WorldDataAccess : GenericDataAccess
    {
        public async Task<List<World>> GetWorlds(WorldSearchCriteria criteria)
        {
            string pageSettings = "";
            if (criteria.PageNumber != null && criteria.PageSize != null)
            {
                pageSettings = $"LIMIT {criteria.PageSize} OFFSET {criteria.PageNumber * criteria.PageSize}";
            }

            string sql = $@"SELECT * FROM Worlds /**where**/
                ORDER BY {criteria.OrderByString}
                {pageSettings};
            ";

            SqlBuilder builder = new SqlBuilder();
            var template = builder.AddTemplate(sql);

            if (!string.IsNullOrEmpty(criteria.Query)) { builder.Where("Name LIKE CONCAT('%', IFNULL(@Query, ''), '%')"); }
            if (!string.IsNullOrEmpty(criteria.OwnerId)) { builder.Where("OwnerId LIKE @OwnerId"); }
            if (criteria.StartDate != null) { builder.Where("CreatedDate >= @StartDate"); }
            if (criteria.EndDate != null) { builder.Where("CreatedDate <= @EndDate"); }
            if (criteria.Id != null) { builder.Where("Id = @Id"); }
            if (criteria.Location != null) { builder.Where("Location LIKE @Location"); }

            return await Query<World>(template.RawSql, criteria, "", true);
        }

        public async Task<int> PostWorld(World data)
        {
            data.CreatedDate = DateTime.Now;

            string sql = $@"
                INSERT INTO Worlds (
                    Id,
                    OwnerId,
                    Location,
                    Name,
                    Summary,   
                    CreatedDate
                ) 
                VALUES (
                    @Id,
                    @OwnerId,
                    @Location,
                    @Name,
                    @Summary,   
                    @CreatedDate
                );
            
                SELECT LAST_INSERT_ID();";

            return (await Query<int>(sql, data, "", true)).FirstOrDefault();
        }

        public async Task<int> PutWorld(World data)
        {
            string updateDefaults = "";

            string sql = $@"
                {updateDefaults}
                UPDATE Worlds SET 
                    OwnerId = @OwnerId,
                    Location = @Location,
                    Name = @Name,
                    Summary = @Summary
                WHERE Id = @Id";

            return await Execute(sql, data, "", true);
        }
    }
}
