using Dapper;
using SardCoreAPI.Models.Common;
using SardCoreAPI.Models.Hub.Worlds;
using SardCoreAPI.Models.Map.Location;
using SardCoreAPI.Models.Units;

namespace SardCoreAPI.DataAccess.Units
{
    public class MeasurableDataAccess : GenericDataAccess
    {
        public async Task<List<Measurable>> GetMeasurables(PagedSearchCriteria criteria, WorldInfo info)
        {
            string pageSettings = "";
            if (criteria.PageNumber != null && criteria.PageSize != null)
            {
                pageSettings = $"LIMIT {criteria.PageSize} OFFSET {(criteria.PageNumber) * criteria.PageSize}";
            }

            string sql = $@"
                SELECT Id, Name, Summary, UnitType
                FROM Measurables
                /**where**/
                /**orderby**/
                {pageSettings}
            ";

            SqlBuilder builder = new SqlBuilder();
            var template = builder.AddTemplate(sql);

            if (!string.IsNullOrEmpty(criteria.Query)) { builder.Where("Name LIKE CONCAT('%', IFNULL(@Query, ''), '%')"); }
            if (criteria.Id != null && criteria.Id > 0) { builder.Where("Id = @Id"); }

            builder.OrderBy("CASE WHEN Name LIKE CONCAT(@Query, '%') THEN 0 ELSE 1 END, Name");

            return await Query<Measurable>(template.RawSql, criteria, info);
        }

        public async Task<bool> PostMeasurable(Measurable data, WorldInfo info)
        {
            string sql = @"INSERT INTO Measurables (Name, Summary, UnitType) 
                VALUES (@Name, @Summary, @UnitType)
            ";

            return await Execute(sql, data, info) > 0;
        }

        public async Task<int> PutMeasurable(Measurable data, WorldInfo info)
        {
            string sql = @"UPDATE Measurables SET 
	                Name = @Name,
                    Summary = @Summary,
                    UnitType = @UnitType
                WHERE Id = @Id";

            return await Execute(sql, data, info);
        }

        public async Task<int> DeleteMeasurable(int Id, WorldInfo info)
        {
            string sql = @"DELETE FROM Measurables WHERE Id = @Id;";
            return await Execute(sql, new { Id }, info);
        }
    }
}
