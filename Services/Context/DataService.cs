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
        public string WorldLocation { get; }
        /// <summary>
        /// IMPORTANT! Be very careful using this, as allowing user-input world strings is vulnerable to SQL injection.
        /// </summary>
        /// <param name="world"></param>
        /// <returns></returns>
        public SardLibraryDBContext CreateContext(string world);
        public Task UsingWorldContext(WorldInfo worldInfo, Action action);
        public Task StartUsingWorldContext(WorldInfo worldInfo);
        public Task EndUsingWorldContext();
    }

    public class DataService : IDataService
    {
        IConfiguration _config;
        ILogger<IDataService> _logger;

        private SardLibraryDBContext _dbContext;
        private SardCoreDBContext _dbContextCore;

        private string _currentWorldLocation;

        private SardLibraryDBContext _storedLibraryContext;
        private string _storedWorldLocation;

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

        public string WorldLocation
        {
            get
            {
                return _currentWorldLocation;
            }
        }

        public DataService(IWorldInfoService worldInfoService, IConfiguration config, ILogger<IDataService> logger) 
        {
            _config = config;
            _currentWorldLocation = worldInfoService.WorldLocation;
            _logger = logger;

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
            string previousWorldLocation = _currentWorldLocation;
            _currentWorldLocation = worldInfo.WorldLocation;
            _dbContext = CreateContext(worldInfo.WorldLocation);
            try
            {
                action();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
            finally
            {
                _dbContext = previousContext;
                _currentWorldLocation = previousWorldLocation;
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

        public async Task StartUsingWorldContext(WorldInfo worldInfo)
        {
            World? world = await _dbContextCore.World.Where(w => w.Location == worldInfo.WorldLocation).FirstOrDefaultAsync();
            if (world == null)
            {
                throw new ArgumentException("Tried to get context of non-existent world.");
            }

            _storedLibraryContext = _dbContext;
            string previousWorldLocation = _currentWorldLocation;
            _currentWorldLocation = worldInfo.WorldLocation;
            _dbContext = CreateContext(worldInfo.WorldLocation);
        }

        public async Task EndUsingWorldContext()
        {
            _dbContext = _storedLibraryContext;
            _currentWorldLocation = _storedWorldLocation;

            _storedLibraryContext = null!;
            _storedWorldLocation = null!;
        }
    }
}
