using Dapper;
using MySqlConnector;
using SardCoreAPI.Models.DataPoints.DataPointParameters;
using SardCoreAPI.Models.Map.Location;
using System.Linq;

namespace SardCoreAPI.DataAccess.DataPoint
{
    public class DataPointTypeParameterDataAccess
    {
        public async Task<List<DataPointTypeParameter>> GetDataPointTypeParameters(int? Id)
        {
            if (Id == null) return null;

            string sql = @"SELECT Id, Name, Summary, DataPointTypeId, TypeValue, Sequence 
                    FROM datapointtypeparameter
                    /**where**/
                    ORDER BY Sequence";

            SqlBuilder builder = new SqlBuilder();
            var template = builder.AddTemplate(sql);

            builder.Where("DataPointTypeId = @Id");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(Connection.GetConnectionString()))
                {
                    connection.Open();
                    try
                    {
                        List<DataPointTypeParameter> result = (await connection.QueryAsync<DataPointTypeParameter>(template.RawSql, new { Id })).ToList();
                        return result;
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            catch (MySqlException s)
            {
                Console.WriteLine(s);
                return null;
            }
        }
    }
}
