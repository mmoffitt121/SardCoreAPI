using SardCoreAPI.Models.Easy;
using SardCoreAPI.Models.Hub.Worlds;
using SardCoreAPI.Models.Security.LibraryRoles;
using SardCoreAPI.Services.Easy;
using static Dapper.SqlBuilder;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SardCoreAPI.DataAccess.Easy
{
    public interface IEasyDataAccess
    {
        public Task<List<T>> Get<T>(object? query = null, WorldInfo? info = null, IEnumerable<object>? subqueries = null, QueryOptions? queryOptions = null);
        public Task<int> Count<T>(object? query = null, WorldInfo? info = null, IEnumerable<object>? subqueries = null, QueryOptions? queryOptions = null);
        public Task<int> Post<T>(T data, WorldInfo? info = null);
        public Task<int> Post<T>(IEnumerable<T> data, WorldInfo? info = null);
        public Task<int> Put<T>(T data, WorldInfo? info = null, bool insert = false);
        public Task<int> Delete<T>(object query, WorldInfo? info = null);
    }

    public class EasyDataAccess : GenericDataAccess, IEasyDataAccess
    {
        private IEasyQueryService service;

        public async Task<List<T>> Get<T>(object? query = null, WorldInfo? info = null, IEnumerable<object>? subqueries = null, QueryOptions? queryOptions = null)
        {
            string sql = service.BuildGet<T>(query, subqueries, queryOptions);

            return await Query<T>(sql, query, info);
        }

        public async Task<int> Count<T>(object? query = null, WorldInfo? info = null, IEnumerable<object>? subqueries = null, QueryOptions? queryOptions = null)
        {
            string sql = service.BuildGet<T>(query, subqueries, queryOptions, count: true);

            return await QueryFirst<int>(sql, query, info);
        }

        public async Task<int> Post<T>(T data, WorldInfo? info = null)
        {
            string sql = service.BuildPost<T>();

            return await Execute(sql, data, info);
        }

        public async Task<int> Post<T>(IEnumerable<T> data, WorldInfo? info = null)
        {
            string sql = service.BuildPost<T>();

            return await Execute(sql, data, info);
        }

        public async Task<int> Put<T>(T data, WorldInfo? info = null, bool insert = false)
        {
            string sql = service.BuildPut<T>(insert);

            return await Execute(sql, data, info);
        }

        public async Task<int> Delete<T>(object query, WorldInfo? info = null)
        {
            string sql = service.BuildDelete<T>(query);

            return await Execute(sql, query, info);
        }

        public EasyDataAccess(IEasyQueryService easyDataAccessService)
        {
            service = easyDataAccessService;
        }
    }
}
