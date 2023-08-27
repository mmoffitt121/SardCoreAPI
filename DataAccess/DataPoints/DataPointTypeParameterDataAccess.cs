using Dapper;
using MySqlConnector;
using SardCoreAPI.Models.DataPoints;
using SardCoreAPI.Models.DataPoints.DataPointParameters;
using SardCoreAPI.Models.Hub.Worlds;
using SardCoreAPI.Models.Map.Location;
using System.Linq;

namespace SardCoreAPI.DataAccess.DataPoints
{
    public class DataPointTypeParameterDataAccess : GenericDataAccess
    {
        public async Task<List<DataPointTypeParameter>> GetDataPointTypeParameters(int Id, WorldInfo info)
        {
            string sql = @"SELECT Id, Name, Summary, DataPointTypeId, TypeValue, Sequence, DataPointTypeReferenceId
                    FROM DataPointTypeParameter
                    /**where**/
                    ORDER BY Sequence";

            SqlBuilder builder = new SqlBuilder();
            var template = builder.AddTemplate(sql);

            builder.Where("DataPointTypeId = @Id");

            return await Query<DataPointTypeParameter>(template.RawSql, new { Id }, info);
        }

        public async Task<int> PostDataPointTypeParameter(DataPointTypeParameter data, WorldInfo info)
        {
            if (data.DataPointTypeReferenceId == -1) data.DataPointTypeReferenceId = null;

            string sql = @"INSERT INTO DataPointTypeParameter (Name, Summary, DataPointTypeId, TypeValue, Sequence, DataPointTypeReferenceId)
                    VALUES (@Name, @Summary, @DataPointTypeId, @TypeValue, @Sequence, @DataPointTypeReferenceId)";

            SqlBuilder builder = new SqlBuilder();
            var template = builder.AddTemplate(sql);

            builder.Where("DataPointTypeId = @Id");

            return await Execute(template.RawSql, data, info);
        }

        public async Task<int> PutDataPointTypeParameter(DataPointTypeParameter data, WorldInfo info)
        {
            if (data.DataPointTypeReferenceId == -1) data.DataPointTypeReferenceId = null;

            string sql = @"UPDATE DataPointTypeParameter
                SET
                    Name = @Name,
                    Summary = @Summary,
                    DataPointTypeId = @DataPointTypeId,
                    Typevalue = @TypeValue,
                    Sequence = @Sequence,
                    DataPointTypeReferenceId = @DataPointTypeReferenceId
                WHERE Id = @Id";

            return await Execute(sql, data, info);
        }

        public async Task<int> DeleteDataPointTypeParameter(DataPointTypeParameter data, WorldInfo info)
        {
            string sql = @"DELETE FROM DataPointTypeParameter
                WHERE Id = @Id";

            return await Execute(sql, data, info);
        }

        public async Task<int> DeleteDataPointTypeParametersOfDataType(int id, WorldInfo info)
        {
            string sql = @"DELETE FROM DataPointTypeParameter
                WHERE DataPointTypeId = @Id";

            return await Execute(sql, new { id }, info);
        }
    }
}
