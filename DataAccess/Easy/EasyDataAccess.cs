using SardCoreAPI.Models.Easy;
using SardCoreAPI.Models.Hub.Worlds;
using SardCoreAPI.Models.Security.LibraryRoles;
using SardCoreAPI.Services.Easy;
using SardCoreAPI.Services.WorldContext;
using static Dapper.SqlBuilder;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SardCoreAPI.DataAccess.Easy
{
    public interface IEasyDataAccess
    {
        public Task<T> First<T>(object? query = null, WorldInfo? info = null, bool global = false, QueryOptions? queryOptions = null);
        public Task<List<T>> Get<T>(object? query = null, WorldInfo? info = null, bool global = false, QueryOptions? queryOptions = null);
        public Task<int> Count<T>(object? query = null, WorldInfo? info = null, bool global = false, QueryOptions? queryOptions = null);
        public Task<int> Post<T>(T data, WorldInfo? info = null, bool global = false);
        public Task<int> Post<T>(IEnumerable<T> data, WorldInfo? info = null, bool global = false);
        public Task<int> Put<T>(T data, WorldInfo? info = null, bool insert = false, bool global = false);
        public Task<int> Delete<T>(object query, WorldInfo? info = null, bool global = false);
    }

    public class EasyDataAccess : GenericDataAccess, IEasyDataAccess
    {
        private IEasyQueryService _queryService;
        private IWorldInfoService _worldInfoService;

        public EasyDataAccess(IEasyQueryService easyDataAccessService, IWorldInfoService worldInfoService)
        {
            _queryService = easyDataAccessService;
            _worldInfoService = worldInfoService;
        }

        public async Task<T> First<T>(object? query = null, WorldInfo? info = null, bool global = false, QueryOptions? queryOptions = null)
        {
            if (info == null && !global) { info = _worldInfoService.GetWorldInfo(); }
            string sql = _queryService.BuildGet<T>(query, queryOptions);

            return await QueryFirst<T>(sql, query, info);
        }

        public async Task<List<T>> Get<T>(object? query = null, WorldInfo? info = null, bool global = false, QueryOptions? queryOptions = null)
        {
            if (info == null && !global) { info = _worldInfoService.GetWorldInfo(); }
            string sql = _queryService.BuildGet<T>(query, queryOptions);

            return await Query<T>(sql, query, info);
        }

        public async Task<int> Count<T>(object? query = null, WorldInfo? info = null, bool global = false, QueryOptions? queryOptions = null)
        {
            if (info == null && !global) { info = _worldInfoService.GetWorldInfo(); }
            string sql = _queryService.BuildGet<T>(query, queryOptions, count: true);

            return await QueryFirst<int>(sql, query, info);
        }

        public async Task<int> Post<T>(T data, WorldInfo? info = null, bool global = false)
        {
            if (info == null && !global) { info = _worldInfoService.GetWorldInfo(); }
            string sql = _queryService.BuildPost<T>();

            return await Execute(sql, data, info);
        }

        public async Task<int> Post<T>(IEnumerable<T> data, WorldInfo? info = null, bool global = false)
        {
            if (info == null && !global) { info = _worldInfoService.GetWorldInfo(); }
            string sql = _queryService.BuildPost<T>();

            return await Execute(sql, data, info);
        }

        public async Task<int> Put<T>(T data, WorldInfo? info = null, bool insert = false, bool global = false)
        {
            if (info == null && !global) { info = _worldInfoService.GetWorldInfo(); }
            string sql = _queryService.BuildPut<T>(insert);

            return await Execute(sql, data, info);
        }

        public async Task<int> Delete<T>(object query, WorldInfo? info = null, bool global = false)
        {
            if (info == null && !global) { info = _worldInfoService.GetWorldInfo(); }
            string sql = _queryService.BuildDelete<T>(query);

            return await Execute(sql, query, info);
        }
    }
}
