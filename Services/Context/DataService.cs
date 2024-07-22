using Microsoft.EntityFrameworkCore;
using SardCoreAPI.Database.DBContext;
using SardCoreAPI.Models.Hub.Worlds;
using SardCoreAPI.Services.Hub;
using SardCoreAPI.Services.WorldContext;
using System.Runtime.CompilerServices;

namespace SardCoreAPI.Services.Context
{
    public interface IDataService
    {
        public SardLibraryDBContext Context { get; }
        public SardCoreDBContext CoreContext { get; }
        /// <summary>
        /// IMPORTANT! Be very careful using this, as allowing user-input world strings is vulnerable to SQL injection.
        /// </summary>
        /// <param name="world"></param>
        /// <returns></returns>
        public SardLibraryDBContext CreateContext(string world);
        public Task UsingWorldContext(WorldInfo worldInfo, Action action);
    }

    public class DataService : IDataService
    {
        IConfiguration _config;

        private SardLibraryDBContext _dbContext;
        private SardCoreDBContext _dbContextCore;

        public SardLibraryDBContext Context
        {
            get
            {
                if (_dbContext == null)
                {
                    throw new ThreadStateException("World context requested when not currently in world.");
                }
                return _dbContext;
            }
        }

        public SardCoreDBContext CoreContext
        {
            get
            {
                return _dbContextCore;
            }
        }

        public DataService(IWorldInfoService worldInfoService, IConfiguration config) 
        {
            _config = config;

            if (worldInfoService.WorldLocation != null)
            {
                _dbContext = CreateContext(worldInfoService.WorldLocation);
            }

            DbContextOptionsBuilder<SardCoreDBContext> coreBuilder = new DbContextOptionsBuilder<SardCoreDBContext>();
            string coreConnectionString = _config.GetConnectionString("SardCoreAPIContextConnection") ?? throw new ArgumentException("Core connection string not found");
            coreBuilder.UseMySql(coreConnectionString, new MySqlServerVersion(new Version(8, 0, 32)));

            _dbContextCore = new SardCoreDBContext(coreBuilder.Options);
        }

        public async Task UsingWorldContext(WorldInfo worldInfo, Action action)
        {
            World? world = await _dbContextCore.World.Where(w => w.Location == worldInfo.WorldLocation).FirstOrDefaultAsync();
            if (world == null)
            {
                throw new ArgumentException("Tried to get context of non-existent world.");
            }

            SardLibraryDBContext previousContext = _dbContext;
            _dbContext = CreateContext(worldInfo.WorldLocation);
            try
            {
                action();
            }
            finally
            {
                _dbContext = previousContext;
            }
        }

        /// <summary>
        /// IMPORTANT! Be very careful using this, as allowing user-input world strings is vulnerable to SQL injection.
        /// </summary>
        /// <param name="world"></param>
        /// <returns></returns>
        public SardLibraryDBContext CreateContext(string world)
        {
            DbContextOptionsBuilder<SardLibraryDBContext> builder = new DbContextOptionsBuilder<SardLibraryDBContext>();
            string connectionString = $"{_config.GetConnectionString("SardCoreAPIWorldContextConnection")}Database={world};";
            builder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 32)));

            return new SardLibraryDBContext(builder.Options);
        }
    }
}
