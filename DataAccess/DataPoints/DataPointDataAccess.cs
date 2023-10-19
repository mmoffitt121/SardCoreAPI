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

            string sql;
            if (criteria.Parameters != null && criteria.Parameters.Count() > 0)
            {
                sql = $@"SELECT * FROM DataPoints dp
                            LEFT JOIN DataPointParameterBoolean bdp ON bdp.DataPointId = dp.Id
                            LEFT JOIN DataPointParameterDataPoint dpdp ON dpdp.DataPointId = dp.Id
                            LEFT JOIN DataPointParameterDocument ddp ON ddp.DataPointId = dp.Id
                            LEFT JOIN DataPointParameterDouble dbdp ON dbdp.DataPointId = dp.Id
                            LEFT JOIN DataPointParameterInt idp ON idp.DataPointId = dp.Id
                            LEFT JOIN DataPointParameterString sdp ON sdp.DataPointId = dp.Id
                            LEFT JOIN DataPointParameterSummary sudp ON sudp.DataPointId = dp.Id
                        /**where**/ 
                        ORDER BY Name";
            }
            else
            {
                sql = $@"SELECT *
                    FROM DataPoints
                    /**where**/
                    ORDER BY Name
                    {pageSettings}
                ";
            }

            SqlBuilder builder = new SqlBuilder();
            var template = builder.AddTemplate(sql);

            if (!string.IsNullOrEmpty(criteria.Query)) { builder.Where("Name LIKE CONCAT('%', IFNULL(@Query, ''), '%')"); }
            if (criteria.TypeId != null && criteria.TypeId != -1) { builder.Where("TypeId = @TypeId"); }
            if (criteria.Id != null) { builder.Where("Id = @Id"); }

            return await Query<DataPoint>(template.RawSql, criteria, info);
        }

        public async Task<int> GetDataPointsCount(DataPointSearchCriteria criteria, WorldInfo info)
        {
            string sql = $@"SELECT COUNT(*)
                FROM DataPoints
                /**where**/
                ORDER BY Name
            ";

            SqlBuilder builder = new SqlBuilder();
            var template = builder.AddTemplate(sql);

            if (!string.IsNullOrEmpty(criteria.Query)) { builder.Where("Name LIKE CONCAT('%', IFNULL(@Query, ''), '%')"); }
            if (criteria.TypeId != null && criteria.TypeId != -1) { builder.Where("TypeId = @TypeId"); }
            if (criteria.Id != null) { builder.Where("Id = @Id"); }

            return await QueryFirst<int>(template.RawSql, criteria, info);
        }

        public async Task<List<DataPoint>> GetDataPointsReferencingDataPoint(int id, WorldInfo info)
        {
            string sql = $@"SELECT *
                FROM DataPointParameterDataPoint dppdp
                    LEFT JOIN DataPoints dp ON dp.Id = dppdp.DataPointId
                WHERE Value = @Id
                ORDER BY Name
            ";

            SqlBuilder builder = new SqlBuilder();
            var template = builder.AddTemplate(sql);

            return await Query<DataPoint>(template.RawSql, new { id }, info);

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
