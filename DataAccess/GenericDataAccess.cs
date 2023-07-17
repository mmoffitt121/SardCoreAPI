using MySqlConnector;
using Dapper;
using SardCoreAPI.Models.DataPoints.DataPointParameters;
using static Dapper.SqlBuilder;

namespace SardCoreAPI.DataAccess
{
    public class GenericDataAccess
    {
        public async Task<List<T>> Query<T>(string sql, object data, bool globalConnection = false)
        {
            string connectionString = globalConnection ? Connection.GetGlobalConnectionString() : Connection.GetConnectionString();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    List<T> result = (await connection.QueryAsync<T>(sql, data)).ToList();
                    return result;
                }
            }
            catch (MySqlException s)
            {
                Console.WriteLine(s);
                throw s;
            }
        }

        public async Task<T> QueryFirst<T>(string sql, object data, bool globalConnection = false)
        {
            List<T> result = await Query<T>(sql, data, globalConnection);
            if (result.Count > 0)
            {
                return result[0];
            }
            else
            {
                return default;
            }
        }

        public async Task<int> Execute(string sql, object data, bool globalConnection = false)
        {
            string connectionString = globalConnection ? Connection.GetGlobalConnectionString() : Connection.GetConnectionString();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    return await connection.ExecuteAsync(sql, data);
                }
            }
            catch (MySqlException s)
            {
                Console.WriteLine(s);
                throw s;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw e;
            }
        }
    }
}
