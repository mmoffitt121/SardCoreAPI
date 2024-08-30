using LinqKit;
using SardCoreAPI.Models.Common;
using SardCoreAPI.Models.DataPoints;
using SardCoreAPI.Utility.DataAccess;

namespace SardCoreAPI.Models.Map
{
    public class MapSearchCriteria : PagedSearchCriteria
    {
        public bool? IsDefault { get; set; }

        public ExpressionStarter<Map> GetQuery()
        {
            return GetQuery<Map>()
                .OrIf(IsDefault != null, x => x.IsDefault == IsDefault)
                .OrIf(Id != null, x => x.Id.Equals(Id))
                .OrIf(Query != null, x => x.Name.Contains(Query ?? ""))
                .AndIf(true, x => true);
        }
    }
}
