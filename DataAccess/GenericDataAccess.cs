using MySqlConnector;
using Dapper;
using SardCoreAPI.Models.DataPoints.DataPointParameters;
using static Dapper.SqlBuilder;
using SardCoreAPI.Models.Hub.Worlds;
using SardCoreAPI.Models.Security.Users;
using System.Linq.Expressions;
using SardCoreAPI.Models.Common;
using Microsoft.CodeAnalysis;

namespace SardCoreAPI.DataAccess
{
    public class GenericDataAccess
    {
        public async Task<List<T>> Query<T>(string sql, object? data, WorldInfo? info, bool globalConnection = false)
        {
            return await Query<T>(sql, data, info?.WorldLocation, globalConnection);
        }

        public async Task<List<T>> Query<T>(string sql, object? data, string? location, bool globalConnection = false)
        {
            if (location == null) globalConnection = true;
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

        public async Task<string> QueryStr(string sql, object? data, string? location, bool globalConnection = false)
        {
            string connectionString = globalConnection ? Connection.GetGlobalConnectionString() : Connection.GetConnectionString(location);
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    List<GenericTuple> result = (await connection.QueryAsync<GenericTuple>(sql, data)).ToList();
                    return result.First()?.Value;
                }
            }
            catch (MySqlException s)
            {
                Console.WriteLine(s);
                throw s;
            }
        }

        public async Task<List<string>> QueryStrList(string sql, object? data, string? location, bool globalConnection = false)
        {
            string connectionString = globalConnection ? Connection.GetGlobalConnectionString() : Connection.GetConnectionString(location);
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    List<GenericTuple> result = (await connection.QueryAsync<GenericTuple>(sql, data)).ToList();
                    List<string> values = new List<string>();
                    result.ForEach(t =>
                    {
                        values.Add(t.Value);
                    });
                    return values;
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
            if (location == null) globalConnection = true;
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
