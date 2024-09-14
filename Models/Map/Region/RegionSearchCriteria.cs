using LinqKit;
using SardCoreAPI.Models.Common;
using SardCoreAPI.Utility.DataAccess;

namespace SardCoreAPI.Models.Map.Region
{
    public class RegionSearchCriteria : PagedSearchCriteria
    {
        public int? LocationId { get; set; }

        public ExpressionStarter<Region> GetQuery()
        {
            return GetQuery<Region>()
                .AndIf(Id != null, x => x.Id.Equals(Id))
                .AndIf(Query != null, x => x.Name.Contains(Query ?? ""))
                .AndIf(LocationId != null, x => x.LocationId.Equals(LocationId))
                .AndIf(true, x => true);
        }
    }
}
