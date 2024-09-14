using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SardCoreAPI.Models.DataPoints.DataPointParameters;
using SardCoreAPI.Models.Easy;
using SardCoreAPI.Models.Hub.Worlds;
using System.Linq.Expressions;

namespace SardCoreAPI.Utility.DataAccess
{
    public static class QueryExtensions
    {
        public static ExpressionStarter<T> Query<T>(this DbSet<T> dbSet) where T : class
        {
            var predicate = PredicateBuilder.New<T>();

            return predicate;
        }

        public static EntityEntry<T> Put<T>(this DbSet<T> dbSet, T obj, Expression<Func<T, bool>> predicate) where T : class
        {
            if (!dbSet.Any(predicate))
            {
                return dbSet.Add(obj);
            }
            else
            {
                return dbSet.Update(obj);
            }
        }

        public static ExpressionStarter<T> OrIf<T>(this ExpressionStarter<T> predicate, bool condition, Expression<Func<T, bool>> func)
        {
            if (condition)
            {
                predicate.Or(func);
            }
            return predicate;
        }

        public static ExpressionStarter<T> AndIf<T>(this ExpressionStarter<T> predicate, bool condition, Expression<Func<T, bool>> func)
        {
            if (condition)
            {
                predicate.And(func);
            }
            return predicate;
        }

        public static IQueryable<T> Paginate<T>(this IQueryable<T> query, QueryOptions options)
        {
            if (options.PageNumber != null && options.PageSize != null)
            {
                int pageNumber = ((int)options.PageNumber) - 1;
                query = query.Skip((int)(pageNumber * options.PageSize)).Take((int)options.PageSize);
            }

            return query;
        }

        public static IQueryable<T> Sort<T>(this IQueryable<T> query, QueryOptions options)
        {
            return options.GetOrderBy(query);
        }
    }
}
