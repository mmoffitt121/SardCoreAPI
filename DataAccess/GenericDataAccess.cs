using MySqlConnector;
using Dapper;
using SardCoreAPI.Models.DataPoints.DataPointParameters;
using static Dapper.SqlBuilder;
using SardCoreAPI.Models.Hub.Worlds;

namespace SardCoreAPI.DataAccess
{
    public class GenericDataAccess
    {
        public async Task<List<T>> Query<T>(string sql, object data, WorldInfo? info, bool globalConnection = false)
        {
            return await Query<T>(sql, data, info?.WorldLocation, globalConnection);
        }

        public async Task<List<T>> Query<T>(string sql, object data, string? location, bool globalConnection = false)
        {
            string connectionString = globalConnection ? Connection.GetGlobalConnectionString() : Connection.GetConnectionString(location);
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

        public async Task<T> QueryFirst<T>(string sql, object data, WorldInfo? info, bool globalConnection = false)
        {
            return await QueryFirst<T>(sql, data, info?.WorldLocation, globalConnection);
        }

        public async Task<T> QueryFirst<T>(string sql, object data, string? location, bool globalConnection = false)
        {
            List<T> result = await Query<T>(sql, data, location, globalConnection);
            if (result.Count > 0)
            {
                return result[0];
            }
            else
            {
                return default;
            }
        }

        public async Task<int> Execute(string sql, object data, WorldInfo? info, bool globalConnection = false)
        {
            return await Execute(sql, data, info?.WorldLocation, globalConnection);
        }

        public async Task<int> Execute(string sql, object data, string? location, bool globalConnection = false)
        {
            string connectionString = globalConnection ? Connection.GetGlobalConnectionString() : Connection.GetConnectionString(location);

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

        public async Task<int> ExecuteBase(string sql, object data)
        {
            string connectionString = Connection.GetBaseConnectionString();

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
