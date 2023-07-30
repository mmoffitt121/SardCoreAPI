using SardCoreAPI.Models.Hub.Worlds;

namespace SardCoreAPI.DataAccess.Hub.Worlds
{
    public class WorldGenerator : GenericDataAccess
    {
        public async Task<int> GenerateWorld(World data)
        {
            return 2;
        }
    }
}
