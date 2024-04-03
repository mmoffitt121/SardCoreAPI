using Dapper;
using SardCoreAPI.Models.DataPoints.Queried;
using SardCoreAPI.Models.Easy;
using System.Dynamic;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Reflection;
using SardCoreAPI.Models.DataPoints;
using SardCoreAPI.Models.DataPoints.DataPointParameters;

namespace SardCoreAPI.Services.DataPoints
{
    public interface IDataPointQueryService
    {
        public (ExpandoObject, string) BuildIdQuery(DataPointSearchCriteria query);
        public (ExpandoObject, string) BuildCountQuery(DataPointSearchCriteria query);
        public string BuildDataPointQuery();
    }
    public class MySQLDataPointQueryService : IDataPointQueryService
    {

        public string BuildDataPointQuery()
        {
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
                WHERE dp.Id IN @Ids
                GROUP BY dp.Id, tp.Id, p.Value
            ";

            return sql;
        }
        public (ExpandoObject, string) BuildIdQuery(DataPointSearchCriteria criteria)
        {
            string pageSettings = "";
            if (criteria.PageNumber != null && criteria.PageSize != null)
            {
                pageSettings = $"LIMIT {criteria.PageSize} OFFSET {(criteria.PageNumber - 1) * criteria.PageSize}";
            }

            string sql = $@"SELECT * FROM DataPoints dp
                    /**where**/
                    ORDER BY Name
                    {pageSettings}
            ";

            SqlBuilder builder = new SqlBuilder();
            var template = builder.AddTemplate(sql);
            ExpandoObject valueBank = BuildValueBank(criteria);

            BuildDataPointsQuery(criteria, builder, valueBank);

            return (valueBank, template.RawSql);
        }

        public (ExpandoObject, string) BuildCountQuery(DataPointSearchCriteria criteria)
        {
            string sql = $@"SELECT COUNT(*) FROM DataPoints dp
                    /**leftjoin**/
                    /**where**/
                    ORDER BY Name
            ";

            SqlBuilder builder = new SqlBuilder();
            var template = builder.AddTemplate(sql);
            ExpandoObject valueBank = BuildValueBank(criteria);

            BuildDataPointsQuery(criteria, builder, valueBank);

            return (valueBank, template.RawSql);
        }

        private void BuildDataPointsQuery(DataPointSearchCriteria criteria, SqlBuilder builder, ExpandoObject valueBank)
        {
            if (criteria.Parameters != null && criteria.Parameters.Count() > 0 && criteria.ParameterSearchOptions != null && criteria.ParameterSearchOptions.Count() > 0)
            {
                int i = 0;
                criteria.ParameterSearchOptions.ForEach(opt =>
                {
                    var param = criteria.Parameters.FirstOrDefault(p => p.DataPointTypeParameterId == opt.DataPointTypeParameterId);
                    opt.SequenceId = i;
                    builder.Where($"dp.Id IN ({GetParameterSubquery(param, opt, valueBank)})");
                    i++;
                });
            }

            if (!string.IsNullOrEmpty(criteria.Query)) { builder.Where("dp.Name LIKE CONCAT('%', IFNULL(@Query, ''), '%')"); }
            if (criteria.TypeId != null && criteria.TypeId != -1) { builder.Where("dp.TypeId = @TypeId"); }
            if (criteria.Id != null) { builder.Where("dp.Id = @Id"); }

            if (criteria.TypeIds != null && criteria.TypeIds.Count() > 0)
            {
                builder.Where("dp.TypeId IN @TypeIds");
            }
        }

        private ExpandoObject BuildValueBank(DataPointSearchCriteria criteria)
        {
            dynamic valueBank = new ExpandoObject();
            valueBank.TypeId = criteria.TypeId;
            valueBank.TypeIds = criteria.TypeIds;
            valueBank.Id = criteria.Id;
            valueBank.Query = criteria.Query;
            valueBank.PageNumber = criteria.PageNumber;
            valueBank.PageSize = criteria.PageSize;

            return valueBank;
        }

        private string GetParameterSubquery(DataPointParameter param, ParameterSearchOptions searchOptions, ExpandoObject valueBank)
        {
            string sql = $"SELECT DataPointId FROM {GetTableFromParam(param)} /**where**/";

            SqlBuilder builder = new SqlBuilder();
            var template = builder.AddTemplate(sql);

            switch (searchOptions.FilterMode)
            {
                case ParameterSearchOptions.FilterModeEnum.Equals:
                    string equals = $"@Parameter{searchOptions.SequenceId}";
                    builder.Where($"Value = {equals}");
                    AddParamToBank(valueBank, equals, param);
                    break;
                case ParameterSearchOptions.FilterModeEnum.Contains:
                case ParameterSearchOptions.FilterModeEnum.StartsWith:
                case ParameterSearchOptions.FilterModeEnum.EndsWith:
                    string like = $"@Parameter{searchOptions.SequenceId}";
                    bool startWildcard = searchOptions.FilterMode == ParameterSearchOptions.FilterModeEnum.Contains || searchOptions.FilterMode == ParameterSearchOptions.FilterModeEnum.EndsWith;
                    bool endWildcard = searchOptions.FilterMode == ParameterSearchOptions.FilterModeEnum.Contains || searchOptions.FilterMode == ParameterSearchOptions.FilterModeEnum.StartsWith;
                    builder.Where($"Value LIKE {like}");
                    AddParamToBank(valueBank, like, param, startWildcard: startWildcard, endWildcard: endWildcard);
                    break;
                case ParameterSearchOptions.FilterModeEnum.GreaterThan:
                    string greaterThan = $"@Parameter{searchOptions.SequenceId}";
                    builder.Where($"Value > {greaterThan}");
                    AddParamToBank(valueBank, greaterThan, param);
                    break;
                case ParameterSearchOptions.FilterModeEnum.LessThan:
                    string lessThan = $"@Parameter{searchOptions.SequenceId}";
                    builder.Where($"Value < {lessThan}");
                    AddParamToBank(valueBank, lessThan, param);
                    break;
            }

            return template.RawSql;
        }

        private string GetTableFromParam(DataPointParameter param)
        {
            if (param.GetType() == typeof(DataPointParameterBoolean)) { return "DataPointParameterBoolean"; }
            if (param.GetType() == typeof(DataPointParameterDataPoint)) { return "DataPointParameterDataPoint"; }
            if (param.GetType() == typeof(DataPointParameterDouble)) { return "DataPointParameterDouble"; }
            if (param.GetType() == typeof(DataPointParameterInt)) { return "DataPointParameterInt"; }
            if (param.GetType() == typeof(DataPointParameterString)) { return "DataPointParameterString"; }
            if (param.GetType() == typeof(DataPointParameterSummary)) { return "DataPointParameterSummary"; }
            if (param.GetType() == typeof(DataPointParameterUnit)) { return "DataPointParameterUnit"; }
            if (param.GetType() == typeof(DataPointParameterTime)) { return "DataPointParameterTime"; }

            return "DataPointParameterString";
        }

        private void AddParamToBank(ExpandoObject valueBank, string key, DataPointParameter param, bool startWildcard = false, bool endWildcard = false)
        {
            if (param.GetType() == typeof(DataPointParameterBoolean)) valueBank.TryAdd(key, ((DataPointParameterBoolean)param).BoolValue);
            if (param.GetType() == typeof(DataPointParameterDataPoint)) valueBank.TryAdd(key, ((DataPointParameterDataPoint)param).DataPointValueId);
            if (param.GetType() == typeof(DataPointParameterDouble)) valueBank.TryAdd(key, ((DataPointParameterDouble)param).DoubleValue);
            if (param.GetType() == typeof(DataPointParameterInt)) valueBank.TryAdd(key, ((DataPointParameterInt)param).IntValue);
            if (param.GetType() == typeof(DataPointParameterString))
            {
                string val = ((DataPointParameterString)param).StringValue;
                if (startWildcard) val = "%" + val;
                if (endWildcard) val = val + "%";
                valueBank.TryAdd(key, val);
            }
            if (param.GetType() == typeof(DataPointParameterSummary))
            {
                string val = ((DataPointParameterSummary)param).SummaryValue;
                if (startWildcard) val = "%" + val;
                if (endWildcard) val = val + "%";
                valueBank.TryAdd(key, val);
            }
            if (param.GetType() == typeof(DataPointParameterUnit)) valueBank.TryAdd(key, ((DataPointParameterUnit)param).UnitValue);
            if (param.GetType() == typeof(DataPointParameterTime)) valueBank.TryAdd(key, ((DataPointParameterTime)param).TimeValueString);
        }
    }
}
