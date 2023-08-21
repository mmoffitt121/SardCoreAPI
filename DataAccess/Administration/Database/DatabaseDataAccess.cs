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
    }
}
