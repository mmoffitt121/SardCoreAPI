using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SardCoreAPI.Models.Security;
using SardCoreAPI.Controllers.Security.Users;
using SardCoreAPI.DataAccess;
using SardCoreAPI.Models.Administration.Database;
using SardCoreAPI.Models.Hub.Worlds;
using SardCoreAPI.Services.Context;
using System.Data.Common;

namespace SardCoreAPI.Services.Database
{
    public interface IDatabaseService
    {
        public Task<string> GetServerVersion();
        public Task<List<DatabaseInfo>> GetDatabases();
        public Task UpdateDatabase();
        public Task UpdateWorldDatabases();
        public Task UpdateWorldDatabase(World world);
    }

    public class DatabaseService : GenericDataAccess, IDatabaseService
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
            await Execute(tableSQL, null, "", true);
        }

        public async Task UpdateWorldDatabases()
        {
            string worldSql = "SELECT * FROM Worlds";
            List<World> worlds = await Query<World>(worldSql, null, "", true);

            string tableSQL = File.ReadAllText("./Database/DDL/SardLibraryDDL.sql");
            foreach (World world in worlds)
            {
                await Execute(tableSQL, world, world.Location, false);
            }
        }

        public async Task UpdateWorldDatabase(World world)
        {
            throw new NotImplementedException();
        }
    }

    public class EFCoreDatabaseService : GenericDataAccess, IDatabaseService
    {
        private IDataService _dataService;
        public EFCoreDatabaseService(IDataService service)
        {
            _dataService = service;
        }

        public async Task<string> GetServerVersion()
        {
            DatabaseFacade db = _dataService.CoreContext.Database;
            return $"{db.GetDbConnection().Database}{db.GetAppliedMigrations().LastOrDefault()} Latest: {db.GetMigrations().LastOrDefault()}";
        }

        public async Task<List<DatabaseInfo>> GetDatabases()
        {
            return _dataService.CoreContext.World.Select(w => new DatabaseInfo(w.Name, -999)).ToList();
        }

        public async Task UpdateDatabase()
        {
            await _dataService.CoreContext.Database.MigrateAsync();
        }

        public async Task UpdateWorldDatabases()
        {
            foreach (World w in _dataService.CoreContext.World.ToList())
            {
                await UpdateWorldDatabase(w);
            }
        }

        public async Task UpdateWorldDatabase(World world)
        {
            await _dataService.CreateContext(world.Location).Database.MigrateAsync();
        }
    }
}
