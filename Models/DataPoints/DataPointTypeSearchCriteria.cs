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
                .OrIf(DataPointTypeIds != null && DataPointTypeIds.Count() > 0, x => DataPointTypeIds!.Contains(x.Id))
                .OrIf(Id != null, x => x.Id.Equals(Id))
                .OrIf(Query != null, x => x.Name.Contains(Query ?? ""))
                .OrIf(true, x => true);
        }

        public override IQueryable<T> GetOrderBy<T>(IQueryable<T> queryable)
        {
            if (typeof(DataPointType).Equals(typeof(T)))
            {
                switch (OrderBy)
                {
                    case "Name":
                    default:
                        ((IQueryable<DataPointType>)queryable).OrderBy(x => x.Name);
                        break;
                }
            }

            return queryable;
        }
    }
}
