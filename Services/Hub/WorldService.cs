using SardCoreAPI.Models.Hub.Worlds;
using SardCoreAPI.Services.Context;
using SardCoreAPI.Services.Database;
using SardCoreAPI.Services.Security;

namespace SardCoreAPI.Services.Hub
{
    public interface IWorldService
    {
        public Task<int> CreateWorld(World world);
    }

    public class WorldService : IWorldService
    {
        IDataService _dataService;
        IDatabaseService _databaseService;
        ISecurityService _securityService;

        public WorldService(IDataService dataService, IDatabaseService databaseService, ISecurityService securityService) 
        {
            this._dataService = dataService;
            this._databaseService = databaseService;
            this._securityService = securityService;
        }

        public async Task<int> CreateWorld(World world)
        {
            world.Normalize();

            world.CreatedDate = DateTime.Now;

            _dataService.CoreContext.Add(world);
            _dataService.CoreContext.SaveChanges();

            await _databaseService.UpdateWorldDatabase(world);
            await _securityService.InitializeWorldWithDefaultRoles(new WorldInfo(world.Location));

            return world.Id;
        }
    }

    public class WorldGenerationException : Exception {
        public WorldGenerationException(string message) : base(message) { }
    }
}
