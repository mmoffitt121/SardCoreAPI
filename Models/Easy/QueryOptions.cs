using LinqKit;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq.Expressions;

namespace SardCoreAPI.Models.Easy
{
    public class QueryOptions
    {
        public string? OrderBy { get; set; }
        public bool? Descending { get; set; }
        public int? PageNumber { get; set; } = 0;
        public int? PageSize { get; set; }

        public QueryOptions() { }

        // Consumes query options of passed in variable. Useful for converting query objects
        public QueryOptions(QueryOptions queryOptions)
        {
            OrderBy = queryOptions.OrderBy;
            Descending = queryOptions.Descending;
            PageNumber = queryOptions.PageNumber;
            PageSize = queryOptions.PageSize;

            queryOptions.OrderBy = null;
            queryOptions.Descending = null;
            queryOptions.PageNumber = null;
            queryOptions.PageSize = null;
        }

        public virtual ExpressionStarter<T> GetQuery<T>()
        {
            var predicate = PredicateBuilder.New<T>();

            return predicate;
        }

        public virtual IQueryable<T> GetOrderBy<T>(IQueryable<T> queryable)
        {
            return queryable;
        }
    }
}
