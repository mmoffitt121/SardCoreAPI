using Dapper;
using MySqlConnector;
using SardCoreAPI.Models.Common;
using SardCoreAPI.Models.DataPoints;
using SardCoreAPI.Models.DataPoints.DataPointParameters;
using SardCoreAPI.Models.Map.Location;
using System.Collections.Generic;

namespace SardCoreAPI.DataAccess.DataPoints
{
    public class DataPointTypeDataAccess : GenericDataAccess
    {
        public async Task<List<DataPointType>> GetDataPointTypes(PagedSearchCriteria criteria)
        {
            string pageSettings = "";
            if (criteria.PageNumber != null && criteria.PageSize != null)
            {
                pageSettings = $"LIMIT {criteria.PageSize} OFFSET { (criteria.PageNumber - 1) * criteria.PageSize }";
            }

            string sql = $@"SELECT Id, Name, Summary
                FROM DataPointTypes
                /**where**/
                ORDER BY Name
                { pageSettings }
            ";

            SqlBuilder builder = new SqlBuilder();
            var template = builder.AddTemplate(sql);

            if (!string.IsNullOrEmpty(criteria.Query)) { builder.Where("Name LIKE CONCAT('%', IFNULL(@Query, ''), '%')"); }
            if (criteria.Id != null) { builder.Where("Id = @Id"); }

            builder.OrderBy("Name");

            return await Query<DataPointType>(template.RawSql, criteria);
        }

        
        public async Task<int> PostDataPointType(DataPointType data)
        {
            string sql = @"INSERT INTO DataPointTypes (Name, Summary) 
                VALUES (@Name, @Summary);
            
                SELECT LAST_INSERT_ID();";

            return (await Query<int>(sql, data)).FirstOrDefault();
        }
        
        public async Task<int> PutDataPointType(DataPointType data)
        {
            string sql = @"UPDATE DataPointTypes SET 
	                Name = @Name,
                    Summary = @Summary
                WHERE Id = @Id";

            return await Execute(sql, data);
        }
        
        public async Task<int> DeleteDataPointType(int Id)
        {
            string sql = @"DELETE FROM DataPointTypes WHERE Id = @Id;";
            
            return await Execute(sql, new { Id });
        }
    }
}
