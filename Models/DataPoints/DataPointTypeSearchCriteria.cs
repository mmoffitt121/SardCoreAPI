using LinqKit;
using NuGet.Packaging.Signing;
using SardCoreAPI.Models.Common;
using SardCoreAPI.Utility.DataAccess;

namespace SardCoreAPI.Models.DataPoints
{
    public class DataPointTypeSearchCriteria : PagedSearchCriteria
    {
        public int[]? DataPointTypeIds { get; set; }

        public ExpressionStarter<DataPointType> GetQuery()
        {
            return GetQuery<DataPointType>()
                .AndIf(DataPointTypeIds != null && DataPointTypeIds.Count() > 0, x => DataPointTypeIds!.Contains(x.Id))
                .AndIf(Id != null, x => x.Id.Equals(Id))
                .AndIf(Query != null, x => x.Name.Contains(Query ?? ""))
                .AndIf(true, x => true);
        }

        public override IQueryable<T> GetOrderBy<T>(IQueryable<T> queryable)
        {
            if (typeof(DataPointType).Equals(typeof(T)))
            {
                switch (OrderBy)
                {
                    case "Name":
                    default:
                        return (IQueryable<T>)((IQueryable<DataPointType>)queryable).OrderBy(x => x.Name);
                }
            }

            return queryable;
        }
    }
}
