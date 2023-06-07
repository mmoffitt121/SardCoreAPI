using Dapper;
using SardCoreAPI.Models.DataPoints.DataPointParameters;
using SardCoreAPI.Models.DataPoints;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.Extensions.Primitives;

namespace SardCoreAPI.DataAccess.DataPoints
{
    public class DataPointParameterDataAccess : GenericDataAccess
    {
        #region Helper Functions
        private string GetValueAlias(string typeValue)
        {
            switch (typeValue)
            {
                case "int":
                    return "IntValue";
                case "dub":
                    return "DoubleValue";
                case "str":
                    return "StringValue";
                case "sum":
                    return "SummaryValue";
                case "doc":
                    return "DocumentValue";
                case "dat":
                    return "DataPointValue";
                case "bit":
                    return "BoolValue";
                default:
                    return "";
            }
        }

        private string GetTable(string typeValue)
        {
            switch (typeValue)
            {
                case "int":
                    return "DataPointParameterInt";
                case "dub":
                    return "DataPointParameterDouble";
                case "str":
                    return "DataPointParameterString";
                case "sum":
                    return "DataPointParameterSummary";
                case "doc":
                    return "DataPointParameterDocument";
                case "dat":
                    return "DataPointParameterDataPoint";
                case "bit":
                    return "DataPointParameterBoolean";
                default:
                    return "";
            }
        }
        #endregion

        public async Task<T> GetParameter<T>(int dataPointId, int dataPointTypeParameterId, string typeValue)
        {
            string valueAlias = GetValueAlias(typeValue);
            string table = GetTable(typeValue);

            string sql = $@"SELECT DataPointId, DataPointTypeParameterId, Value as {valueAlias}
                FROM {table}
                /**where**/";

            SqlBuilder builder = new SqlBuilder();
            var template = builder.AddTemplate(sql);

            builder.Where("DataPointId = @DataPointId AND DataPointTypeParameterId = @DataPointTypeParameterId");

            return (await Query<T>(template.RawSql, new { DataPointId = dataPointId, DataPointTypeParameterId = dataPointTypeParameterId })).FirstOrDefault();
        }

        public async Task<int> PostParameter<T>(T parameter, string typeValue)
        {
            if (parameter == null) { throw new ArgumentNullException(); }
            string valueAlias = GetValueAlias(typeValue);
            string table = GetTable(typeValue);

            string sql = $@"INSERT INTO {table} (DataPointId, DataPointTypeParameterId, Value )
                VALUES (@DataPointId, @DataPointTypeParameterId, @{valueAlias})";

            return await Execute(sql, parameter);
        }

        public async Task<int> PutParameter<T>(T parameter, string typeValue)
        {
            if (parameter == null) { throw new ArgumentNullException(); }
            string valueAlias = GetValueAlias(typeValue);
            string table = GetTable(typeValue);

            string sql = $@"UPDATE {table} 
                SET Value = @{valueAlias}
                WHERE DataPointId = @DataPointId
                    AND DataPointTypeParameterId = @DataPointTypeParameterId;";

            return await Execute(sql, parameter);
        }

        public async Task<int> DeleteParameter<T>(T parameter, string typeValue)
        {
            if (parameter == null) { throw new ArgumentNullException(); }
            string valueAlias = GetValueAlias(typeValue);
            string table = GetTable(typeValue);

            string sql = $@"DELETE FROM {table}
                WHERE DataPointId = @DataPointId
                    AND DataPointTypeParameterId = @DataPointTypeParameterId;";

            return await Execute(sql, parameter);
        }
    }
}
