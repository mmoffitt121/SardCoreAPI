using SardCoreAPI.Models.Hub.Worlds;

namespace SardCoreAPI.DataAccess.Hub.Worlds
{
    public class WorldGenerator : GenericDataAccess
    {
        public async Task<World> GenerateWorld(World data)
        {
            string createDBSQL = "CREATE SCHEMA " + data.Location;
            await ExecuteBase(createDBSQL, data);
            string tableSQL = File.ReadAllText("./Database/DDL/SardLibraryDDL.sql");
            await Execute(tableSQL, data, data.Location, false);
            return data;
        }
    }
}
