using Dapper;
using MySqlConnector;
using SardCoreAPI.Models.Common;
using SardCoreAPI.Models.DataPoints;
using SardCoreAPI.Models.DataPoints.DataPointParameters;
using SardCoreAPI.Models.Hub.Worlds;
using SardCoreAPI.Models.Map.Location;
using System.Collections.Generic;

namespace SardCoreAPI.DataAccess.DataPoints
{
    public class DataPointTypeDataAccess : GenericDataAccess
    {
        public async Task<List<DataPointType>> GetDataPointTypes(DataPointTypeSearchCriteria criteria, WorldInfo info)
        {
            string pageSettings = "";
            if (criteria.PageNumber != null && criteria.PageSize != null)
            {
                pageSettings = $"LIMIT {criteria.PageSize} OFFSET { (criteria.PageNumber - 1) * criteria.PageSize }";
            }

            string sql = $@"SELECT Id, Name, Summary, Settings
                FROM DataPointTypes
                /**where**/
                ORDER BY Name
                { pageSettings }
            ";

            SqlBuilder builder = new SqlBuilder();
            var template = builder.AddTemplate(sql);

            if (!string.IsNullOrEmpty(criteria.Query)) { builder.Where("Name LIKE CONCAT('%', IFNULL(@Query, ''), '%')"); }
            if (criteria.Id != null) { builder.Where("Id = @Id"); }
            if (criteria.DataPointTypeIds != null && criteria.DataPointTypeIds.Length > 0) { builder.Where("Id IN @DataPointTypeIds"); }

            builder.OrderBy("Name");

            return await Query<DataPointType>(template.RawSql, criteria, info);
        }

        public async Task<int> GetDataPointTypesCount(PagedSearchCriteria criteria, WorldInfo info)
        {
            string sql = $@"SELECT Count(*)
                FROM DataPointTypes
                /**where**/
            ";

            SqlBuilder builder = new SqlBuilder();
            var template = builder.AddTemplate(sql);

            if (!string.IsNullOrEmpty(criteria.Query)) { builder.Where("Name LIKE CONCAT('%', IFNULL(@Query, ''), '%')"); }
            if (criteria.Id != null) { builder.Where("Id = @Id"); }

            builder.OrderBy("Name");

            return await QueryFirst<int>(template.RawSql, criteria, info);
        }


        public async Task<int> PostDataPointType(DataPointType data, WorldInfo info)
        {
            string sql = @"INSERT INTO DataPointTypes (Name, Summary, Settings) 
                VALUES (@Name, @Summary, @Settings);
            
                SELECT LAST_INSERT_ID();";

            return (await Query<int>(sql, data, info)).FirstOrDefault();
        }
        
        public async Task<int> PutDataPointType(DataPointType data, WorldInfo info)
        {
            string sql = @"UPDATE DataPointTypes SET 
	                Name = @Name,
                    Summary = @Summary,
                    Settings = @Settings
                WHERE Id = @Id";

            return await Execute(sql, data, info);
        }
        
        public async Task<int> DeleteDataPointType(int Id, WorldInfo info)
        {
            string sql = @"DELETE FROM DataPointTypes WHERE Id = @Id;";
            
            return await Execute(sql, new { Id }, info);
        }
    }
}
