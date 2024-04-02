using Dapper;
using SardCoreAPI.Models.DataPoints.Queried;
using SardCoreAPI.Models.Easy;
using System.Dynamic;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Reflection;

namespace SardCoreAPI.Services.DataPoints
{
    public interface IDataPointQueryService
    {
        public Tuple<ExpandoObject, string> BuildGet(DataPointQuery query);
    }
    public class MySQLDataPointQueryService : IDataPointQueryService
    {
        public Tuple<ExpandoObject, string> BuildGet(DataPointQuery query)
        {
            string pageSettings = "";
            if (query?.PageNumber != null && query.PageSize != null)
            {
                pageSettings = $"LIMIT {query.PageSize} OFFSET {query.PageNumber * query.PageSize}";
            }

            string orderBy = "";
            if (query?.OrderBy != null)
            {
                orderBy = @$"ORDER BY {query.OrderBy} {(query.Descending ?? false ? "DESC" : "ASC")}";
            }

            // Build Query
            string sql = @$"
                SELECT dp.Id, dp.Name, dp.TypeId, dp.Settings, dpt.Name AS TypeName, dpt.Summary AS TypeSummary, dpt.Settings AS TypeSettings,
	                tp.Id AS TypeParameterId, tp.Summary AS TypeParameterSummary, tp.TypeValue AS TypeParameterTypeValue, tp.Sequence as TypeParameterSequence, tp.DataPointTypeReferenceId, tp.Settings AS TypeParameterSettings,
                    p.Value
                FROM DataPoints dp
	                LEFT JOIN DataPointTypes dpt ON dpt.Id = dp.TypeId
	                LEFT JOIN DataPointTypeParameter tp ON tp.DataPointTypeId = dpt.Id
                    LEFT JOIN (   SELECT DataPointId, DataPointTypeParameterId, IF(Value, 'true', 'false') AS Value, ""Bool"" FROM DataPointParameterBoolean
		                UNION ALL SELECT DataPointId, DataPointTypeParameterId, CAST(Value AS CHAR) AS Value, ""DataPoint"" FROM DataPointParameterDataPoint
		                UNION ALL SELECT DataPointId, DataPointTypeParameterId, CAST(Value AS CHAR) AS Value, ""String"" FROM DataPointParameterDocument
		                UNION ALL SELECT DataPointId, DataPointTypeParameterId, CAST(Value AS CHAR) AS Value, ""String"" FROM DataPointParameterDouble
		                UNION ALL SELECT DataPointId, DataPointTypeParameterId, CAST(Value AS CHAR) AS Value, ""String"" FROM DataPointParameterInt
		                UNION ALL SELECT DataPointId, DataPointTypeParameterId, CAST(Value AS CHAR) AS Value, ""String"" FROM DataPointParameterString
		                UNION ALL SELECT DataPointId, DataPointTypeParameterId, CAST(Value AS CHAR) AS Value, ""String"" FROM DataPointParameterSummary
		                UNION ALL SELECT DataPointId, DataPointTypeParameterId, CAST(Value AS CHAR) AS Value, ""String"" FROM DataPointParameterTime
		                UNION ALL SELECT DataPointId, DataPointTypeParameterId, CAST(Value AS CHAR) AS Value, ""String"" FROM DataPointParameterUnit
                    ) p ON p.DataPointId = dp.Id AND p.DataPointTypeParameterId = tp.Id 

                GROUP BY dp.Id, tp.Id, p.Value
                {orderBy}
                {pageSettings}
            ";

            //SqlBuilder builder = new SqlBuilder();
            //var template = builder.AddTemplate(sql);

            /*foreach (PropertyInfo p in queryProperties)
            {
                if (p.GetValue(query) != null)
                {
                    if (typeof(string).Equals(p.PropertyType))
                    {
                        builder.Where($"{GetColumnName(p)} LIKE @{p.Name}");
                    }
                    else
                    {
                        builder.Where($"{GetColumnName(p)} = @{p.Name}");
                    }
                }

            }*/

            return new Tuple<ExpandoObject, string>(null, sql); // template.RawSql;
        }
    }
}
