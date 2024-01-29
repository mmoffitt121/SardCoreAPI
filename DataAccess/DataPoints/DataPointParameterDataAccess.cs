using Dapper;
using SardCoreAPI.Models.DataPoints.DataPointParameters;
using SardCoreAPI.Models.DataPoints;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.Extensions.Primitives;
using SardCoreAPI.Models.Hub.Worlds;
using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data.Common;
using Microsoft.AspNetCore.Mvc;

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
                    return "DataPointValueId";
                case "bit":
                    return "BoolValue";
                case "uni":
                    return "UnitValue";
                case "tim":
                    return "TimeValue";
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
                case "uni":
                    return "DataPointParameterUnit";
                case "tim":
                    return "DataPointParameterTime";
                default:
                    return "";
            }
        }

        public List<string> GetTables()
        {
            List<string> tables = new List<string>
            {
                GetTable("int"),
                GetTable("dub"),
                GetTable("str"),
                GetTable("sum"),
                GetTable("doc"),
                GetTable("dat"),
                GetTable("bit"),
                GetTable("uni"),
                GetTable("tim")
            };

            return tables;
        }
        #endregion

        public async Task<T> GetParameter<T>(int dataPointId, int dataPointTypeParameterId, string typeValue, WorldInfo info)
        {
            string valueAlias = GetValueAlias(typeValue);
            string table = GetTable(typeValue);

            string sql = $@"SELECT DataPointId, DataPointTypeParameterId, Value as {valueAlias}
                FROM {table}
                /**where**/";

            SqlBuilder builder = new SqlBuilder();
            var template = builder.AddTemplate(sql);

            builder.Where("DataPointId = @DataPointId AND DataPointTypeParameterId = @DataPointTypeParameterId");

            return (await Query<T>(template.RawSql, new { DataPointId = dataPointId, DataPointTypeParameterId = dataPointTypeParameterId }, info)).FirstOrDefault();
        }

        public async Task<int> PutParameter<T>(T parameter, string typeValue, WorldInfo info)
        {
            if (parameter == null) { throw new ArgumentNullException(); }
            string valueAlias = GetValueAlias(typeValue);
            string table = GetTable(typeValue);

            string sql = $@"REPLACE INTO {table} (DataPointId, DataPointTypeParameterId, Value )
                VALUES (@DataPointId, @DataPointTypeParameterId, @{valueAlias})";

            return await Execute(sql, parameter, info);
        }

        public async Task<int> DeleteParameter<T>(T parameter, string typeValue, WorldInfo info)
        {
            if (parameter == null) { throw new ArgumentNullException(); }
            string valueAlias = GetValueAlias(typeValue);
            string table = GetTable(typeValue);

            string sql = $@"DELETE FROM {table}
                WHERE DataPointId = @DataPointId
                    AND DataPointTypeParameterId = @DataPointTypeParameterId;";

            return await Execute(sql, parameter, info);
        }

        public async Task<int> DeleteParameters(int? dataPointId, WorldInfo info)
        {
            if (dataPointId == null) { throw new ArgumentNullException(); }

            List<string> tables = GetTables();

            List<Task> tasks = new List<Task>();
            foreach (string table in tables)
            {
                string sql = $@"DELETE FROM {table} WHERE DataPointId = @DataPointId";
                tasks.Add(Execute(sql, new { dataPointId }, info));
            }

            await Task.WhenAll(tasks);

            return 1;
        }
    }
}
