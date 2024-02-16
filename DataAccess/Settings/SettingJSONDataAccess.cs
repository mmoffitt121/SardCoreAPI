using MySqlConnector;
using Dapper;
using SardCoreAPI.Models.Map.Location;
using SardCoreAPI.Models.Map.LocationType;
using SardCoreAPI.Models.Hub.Worlds;
using static Dapper.SqlBuilder;
using SardCoreAPI.Models.Settings;

namespace SardCoreAPI.DataAccess.Map
{
    public class SettingJSONDataAccess : GenericDataAccess
    { 
        public async Task<List<SettingJSON>> Get(string Id, WorldInfo info)
        {
            string sql = @"SELECT * FROM SettingJSON /**where**/";

            SqlBuilder builder = new SqlBuilder();
            var template = builder.AddTemplate(sql);

            builder.Where("Id = @Id");

            return await Query<SettingJSON>(template.RawSql, new { Id }, info);
        } 

        public async Task<int> Put(SettingJSON data, WorldInfo info)
        {
            string sql = @"INSERT INTO SettingJSON (Id, Setting) VALUES (@Id, @Setting) 
                ON DUPLICATE KEY UPDATE Setting = @Setting"
            ;

            return await Execute(sql, data, info);
        }

        public async Task<int> Delete(int Id, WorldInfo info)
        {
            string sql = @"DELETE FROM SettingJSON WHERE Id = @Id;";
            return await Execute(sql, new { Id }, info);
        }
    }
}
