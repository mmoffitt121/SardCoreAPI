using SardCoreAPI.Models.Administration.Database;

namespace SardCoreAPI.DataAccess.Administration.Database
{
    public class DatabaseDataAccess : GenericDataAccess
    {
        public async Task<string> GetServerVersion()
        {
            string sql = "SELECT Version() AS Value;";
            return await QueryStr(sql, null, null, true);
        }

        public async Task<List<DatabaseInfo>> GetDatabases()
        {
            string sql = @"SELECT table_schema AS Name, ROUND(SUM(data_length + index_length) / 1024 / 1024, 1) 
                    AS Size FROM information_schema.tables 
                    GROUP BY table_schema;";
            return await Query<DatabaseInfo>(sql, null, "", true);
        }

        public async Task UpdateDatabase()
        {
            string createDBSQL = "CREATE DATABASE IF NOT EXISTS libraries_of; ";
            await ExecuteBase(createDBSQL, new { });
            string tableSQL = File.ReadAllText("./Database/DDL/SardCoreDDL.sql");
            await Execute(tableSQL, data, data.Location, false);
            return data;
        }
    }
}
