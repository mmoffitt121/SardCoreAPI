using Dapper;
using Humanizer;
using Microsoft.IdentityModel.Tokens;
using SardCoreAPI.Models.Common;
using SardCoreAPI.Models.DataPoints;
using SardCoreAPI.Models.DataPoints.DataPointParameters;
using SardCoreAPI.Models.Hub.Worlds;
using System.Drawing.Printing;
using System.Runtime.Intrinsics.Arm;

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

            string sql = $@"SELECT * FROM DataPoints dp
                    /**leftjoin**/
                    /**where**/
                    ORDER BY Name
                    {pageSettings}
            ";

            SqlBuilder builder = new SqlBuilder();
            var template = builder.AddTemplate(sql);

            if (criteria.Parameters != null && criteria.Parameters.Count() > 0)
            {
                for (int i = 0; i < Math.Min(criteria.Parameters.Count(), 20); i++)
                {
                    if (criteria.Parameters[i].GetType() == typeof(DataPointParameterBoolean))
                    {
                        criteria.SetParam(i, ((DataPointParameterBoolean)criteria.Parameters[i]).BoolValue ? "1" : "0");
                        builder.LeftJoin($"DataPointParameterBoolean p{i} ON p{i}.DataPointId = dp.Id");
                        builder.Where($"p{i}.Value = @Param{i}");
                    }
                    if (criteria.Parameters[i].GetType() == typeof(DataPointParameterDataPoint))
                    {
                        criteria.SetParam(i, ((DataPointParameterDataPoint)criteria.Parameters[i]).DataPointValueId.ToString());
                        builder.LeftJoin($"DataPointParameterDataPoint p{i} ON p{i}.DataPointId = dp.Id");
                        builder.Where($"p{i}.Value = @Param{i}");
                    }
                    if (criteria.Parameters[i].GetType() == typeof(DataPointParameterDouble) && ((DataPointParameterDouble)criteria.Parameters[i]).DoubleValue != null)
                    {
                        criteria.SetParam(i, ((DataPointParameterDouble)criteria.Parameters[i]).DoubleValue?.ToString() ?? "");
                        builder.LeftJoin($"DataPointParameterDouble p{i} ON p{i}.DataPointId = dp.Id");
                        builder.Where($"p{i}.Value = @Param{i}");
                    }
                    if (criteria.Parameters[i].GetType() == typeof(DataPointParameterInt) && ((DataPointParameterInt)criteria.Parameters[i]).IntValue != null)
                    {
                        criteria.SetParam(i, ((DataPointParameterInt)criteria.Parameters[i]).IntValue?.ToString() ?? "");
                        builder.LeftJoin($"DataPointParameterInt p{i} ON p{i}.DataPointId = dp.Id");
                        builder.Where($"p{i}.Value = @Param{i}");
                    }
                    if (criteria.Parameters[i].GetType() == typeof(DataPointParameterString) && !((DataPointParameterString)criteria.Parameters[i]).StringValue.IsNullOrEmpty())
                    {
                        criteria.SetParam(i, "%" + ((DataPointParameterString)criteria.Parameters[i]).StringValue + "%");
                        builder.LeftJoin($"DataPointParameterString p{i} ON p{i}.DataPointId = dp.Id");
                        builder.Where($"p{i}.Value LIKE @Param{i}");
                    }
                    if (criteria.Parameters[i].GetType() == typeof(DataPointParameterSummary) && !((DataPointParameterSummary)criteria.Parameters[i]).SummaryValue.IsNullOrEmpty())
                    {
                        criteria.SetParam(i, "%" + ((DataPointParameterSummary)criteria.Parameters[i]).SummaryValue + "%");
                        builder.LeftJoin($"DataPointParameterSummary p{i} ON p{i}.DataPointId = dp.Id");
                        builder.Where($"p{i}.Value LIKE @Param{i}");
                    }
                }
            }

            if (!string.IsNullOrEmpty(criteria.Query)) { builder.Where("dp.Name LIKE CONCAT('%', IFNULL(@Query, ''), '%')"); }
            if (criteria.TypeId != null && criteria.TypeId != -1) { builder.Where("dp.TypeId = @TypeId"); }
            if (criteria.Id != null) { builder.Where("dp.Id = @Id"); }

            if (criteria.TypeIds != null && criteria.TypeIds.Count() > 0)
            {
                builder.Where("dp.TypeId IN @TypeIds");
            }

            return await Query<DataPoint>(template.RawSql, criteria, info);
        }

        public async Task<int> GetDataPointsCount(DataPointSearchCriteria criteria, WorldInfo info)
        {
            string sql = $@"SELECT COUNT(*) FROM DataPoints dp
                    /**leftjoin**/
                    /**where**/
                    ORDER BY Name
            ";

            SqlBuilder builder = new SqlBuilder();
            var template = builder.AddTemplate(sql);

            if (criteria.Parameters != null && criteria.Parameters.Count() > 0)
            {
                for (int i = 0; i < Math.Min(criteria.Parameters.Count(), 20); i++)
                {
                    if (criteria.Parameters[i].GetType() == typeof(DataPointParameterBoolean))
                    {
                        criteria.SetParam(i, ((DataPointParameterBoolean)criteria.Parameters[i]).BoolValue ? "1" : "0");
                        builder.LeftJoin($"DataPointParameterBoolean p{i} ON p{i}.DataPointId = dp.Id");
                        builder.Where($"p{i}.Value = @Param{i}");
                    }
                    if (criteria.Parameters[i].GetType() == typeof(DataPointParameterDataPoint))
                    {
                        criteria.SetParam(i, ((DataPointParameterDataPoint)criteria.Parameters[i]).DataPointValueId.ToString());
                        builder.LeftJoin($"DataPointParameterDataPoint p{i} ON p{i}.DataPointId = dp.Id");
                        builder.Where($"p{i}.Value = @Param{i}");
                    }
                    if (criteria.Parameters[i].GetType() == typeof(DataPointParameterDouble) && ((DataPointParameterDouble)criteria.Parameters[i]).DoubleValue != null)
                    {
                        criteria.SetParam(i, ((DataPointParameterDouble)criteria.Parameters[i]).DoubleValue?.ToString() ?? "");
                        builder.LeftJoin($"DataPointParameterDouble p{i} ON p{i}.DataPointId = dp.Id");
                        builder.Where($"p{i}.Value = @Param{i}");
                    }
                    if (criteria.Parameters[i].GetType() == typeof(DataPointParameterInt) && ((DataPointParameterInt)criteria.Parameters[i]).IntValue != null)
                    {
                        criteria.SetParam(i, ((DataPointParameterInt)criteria.Parameters[i]).IntValue?.ToString() ?? "");
                        builder.LeftJoin($"DataPointParameterInt p{i} ON p{i}.DataPointId = dp.Id");
                        builder.Where($"p{i}.Value = @Param{i}");
                    }
                    if (criteria.Parameters[i].GetType() == typeof(DataPointParameterString) && !((DataPointParameterString)criteria.Parameters[i]).StringValue.IsNullOrEmpty())
                    {
                        criteria.SetParam(i, "%" + ((DataPointParameterString)criteria.Parameters[i]).StringValue + "%");
                        builder.LeftJoin($"DataPointParameterString p{i} ON p{i}.DataPointId = dp.Id");
                        builder.Where($"p{i}.Value LIKE @Param{i}");
                    }
                    if (criteria.Parameters[i].GetType() == typeof(DataPointParameterSummary) && !((DataPointParameterSummary)criteria.Parameters[i]).SummaryValue.IsNullOrEmpty())
                    {
                        criteria.SetParam(i, "%" + ((DataPointParameterSummary)criteria.Parameters[i]).SummaryValue + "%");
                        builder.LeftJoin($"DataPointParameterSummary p{i} ON p{i}.DataPointId = dp.Id");
                        builder.Where($"p{i}.Value LIKE @Param{i}");
                    }
                }
            }

            if (!string.IsNullOrEmpty(criteria.Query)) { builder.Where("dp.Name LIKE CONCAT('%', IFNULL(@Query, ''), '%')"); }
            if (criteria.TypeId != null && criteria.TypeId != -1) { builder.Where("dp.TypeId = @TypeId"); }
            if (criteria.Id != null) { builder.Where("dp.Id = @Id"); }

            if (criteria.TypeIds != null && criteria.TypeIds.Count() > 0)
            {
                builder.Where("dp.TypeId IN @TypeIds");
            }

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
