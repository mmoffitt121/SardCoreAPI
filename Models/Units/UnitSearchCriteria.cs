using LinqKit;
using Microsoft.IdentityModel.Tokens;
using NuGet.Packaging.Signing;
using SardCoreAPI.Models.Common;
using SardCoreAPI.Utility.DataAccess;

namespace SardCoreAPI.Models.Units
{
    public class UnitSearchCriteria : PagedSearchCriteria
    {
        public int? MeasurableId { get; set; }

        public ExpressionStarter<Unit> GetQuery()
        {
            return GetQuery<Unit>()
                .AndIf(MeasurableId != null, m => m.MeasurableId.Equals(MeasurableId))
                .AndIf(Query != null && !string.IsNullOrEmpty(Query), m => m.Name.Contains(Query ?? ""))
                .AndIf(Id != null, m => m.Id.Equals(Id))
                .AndIf(true, x => true);
        }
    }
}
