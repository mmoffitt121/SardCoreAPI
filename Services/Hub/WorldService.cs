using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SardCoreAPI.Models.Hub.Worlds;
using SardCoreAPI.Models.Security;
using SardCoreAPI.Services.Context;
using SardCoreAPI.Services.Database;
using SardCoreAPI.Services.Security;
using SardCoreAPI.Utility.Validation;

namespace SardCoreAPI.Services.Hub
{
    public interface IWorldService
    {
        public Task<int> CreateWorld(World world);
        public Task<ViewableWorld> GetWorld(string location);
        public Task UpdateWorld(World world);
    }

    public class WorldService : IWorldService
    {
        IDataService _dataService;
        IDatabaseService _databaseService;
        ISecurityService _securityService;
        UserManager<SardCoreAPIUser> _userManager;

        public WorldService(IDataService dataService, IDatabaseService databaseService, ISecurityService securityService, UserManager<SardCoreAPIUser> userManager) 
        {
            this._dataService = dataService;
            this._databaseService = databaseService;
            this._securityService = securityService;
            this._userManager = userManager;
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

        public async Task<ViewableWorld> GetWorld(string location)
        {
            World world = await _dataService.CoreContext.World.Where(w => w.Location.Equals(location)).SingleAsync();
            SardCoreAPIUser? user = await _userManager.FindByIdAsync(world.OwnerId);
            return new ViewableWorld(world, user?.UserName ?? "(No User Found)");
        }

        public async Task UpdateWorld(World world)
        {
            World existing = await _dataService.CoreContext.World.SingleAsync(x => x.Id.Equals(world.Id));
            existing.Name = world.Name;
            existing.Location = world.Location;
            existing.Summary = world.Summary;
            existing.OwnerId = world.OwnerId;
            existing.IconId = world.IconId;
            _dataService.CoreContext.Update(existing);
            await _dataService.CoreContext.SaveChangesAsync();
        }
    }

    public class WorldGenerationException : Exception {
        public WorldGenerationException(string message) : base(message) { }
    }
}
