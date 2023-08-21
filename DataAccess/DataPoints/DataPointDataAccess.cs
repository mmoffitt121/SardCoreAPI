using Dapper;
using SardCoreAPI.Models.Common;
using SardCoreAPI.Models.DataPoints;
using SardCoreAPI.Models.Hub.Worlds;

namespace SardCoreAPI.DataAccess.DataPoints
{
    public class DataPointDataAccess : GenericDataAccess
    {
        public async Task<List<DataPoint>> GetDataPoints(DataPointSearchCriteria criteria, WorldInfo info)
        {
            string pageSettings = "";
            if (criteria.PageNumber != null && criteria.PageSize != null)
            {
                pageSettings = $"LIMIT {criteria.PageSize} OFFSET {(criteria.PageNumber - 1) * criteria.PageSize}";
            }

            string sql = $@"SELECT Id, Name, TypeId
                FROM DataPoints
                /**where**/
                ORDER BY Name
                {pageSettings}
            ";

            SqlBuilder builder = new SqlBuilder();
            var template = builder.AddTemplate(sql);

            if (!string.IsNullOrEmpty(criteria.Query)) { builder.Where("Name LIKE CONCAT('%', IFNULL(@Query, ''), '%')"); }
            if (criteria.TypeId != null) { builder.Where("TypeId = @TypeId"); }
            if (criteria.Id != null) { builder.Where("Id = @Id"); }

            return await Query<DataPoint>(template.RawSql, criteria, info);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns> Returns a task of type int, where int is the id of the created object </returns>
        public async Task<int?> PostDataPoint(DataPoint data, WorldInfo info)
        {
            string sql = @"INSERT INTO DataPoints (Name, TypeId) 
                VALUES (@Name, @TypeId);
                
                SELECT LAST_INSERT_ID();";

            return (await Query<int?>(sql, data, info)).First();
        }

        public async Task<int> PutDataPoint(DataPoint data, WorldInfo info)
        {
            string sql = @"UPDATE DataPoints SET 
	                Name = @Name
                WHERE Id = @Id";

            return await Execute(sql, data, info);
        }

        public async Task<int> DeleteDataPoint(int Id, WorldInfo info)
        {
            string sql = @"DELETE FROM DataPoints WHERE Id = @Id;";

            return await Execute(sql, new { Id }, info);
        }
    }
}
