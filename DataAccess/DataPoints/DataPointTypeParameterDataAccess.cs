using Dapper;
using MySqlConnector;
using SardCoreAPI.Models.DataPoints.DataPointParameters;
using SardCoreAPI.Models.Map.Location;
using System.Linq;

namespace SardCoreAPI.DataAccess.DataPoints
{
    public class DataPointTypeParameterDataAccess : GenericDataAccess
    {
        public async Task<List<DataPointTypeParameter>> GetDataPointTypeParameters(int Id)
        {
            string sql = @"SELECT Id, Name, Summary, DataPointTypeId, TypeValue, Sequence 
                    FROM DataPointTypeParameter
                    /**where**/
                    ORDER BY Sequence";

            SqlBuilder builder = new SqlBuilder();
            var template = builder.AddTemplate(sql);

            builder.Where("DataPointTypeId = @Id");

            return await Query<DataPointTypeParameter>(template.RawSql, new { Id });
        }

        public async Task<int> PostDataPointTypeParameter(DataPointTypeParameter data)
        {
            string sql = @"INSERT INTO DataPointTypeParameter (Name, Summary, DataPointTypeId, TypeValue, Sequence)
                    VALUES (@Name, @Summary, @DataPointTypeId, @TypeValue, @Sequence)";

            SqlBuilder builder = new SqlBuilder();
            var template = builder.AddTemplate(sql);

            builder.Where("DataPointTypeId = @Id");

            return await Execute(template.RawSql, data);
        }

        public async Task<int> PutDataPointTypeParameter(DataPointTypeParameter data)
        {
            string sql = @"UPDATE DataPointTypeParameter
                SET
                    Name = @Name,
                    Summary = @Summary,
                    DataPointTypeId = @DataPointTypeId,
                    Typevalue = @TypeValue,
                    Sequence = @Sequence
                WHERE Id = @Id";

            return await Execute(sql, data);
        }

        public async Task<int> DeleteDataPointTypeParameter(DataPointTypeParameter data)
        {
            string sql = @"DELETE FROM DataPointTypeParameter
                WHERE Id = @Id";

            return await Execute(sql, data);
        }
    }
}
