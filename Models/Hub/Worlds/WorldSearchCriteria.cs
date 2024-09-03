using LinqKit;
using Newtonsoft.Json;
using SardCoreAPI.Models.Common;
using SardCoreAPI.Utility.DataAccess;

namespace SardCoreAPI.Models.Hub.Worlds
{
    public class WorldSearchCriteria : PagedSearchCriteria
    {
        public string? OwnerId { get; set; }
        public string? OwnerName { get; set; }
        public string? Location { get; set; }

        public ExpressionStarter<World> GetQuery()
        {
            return GetQuery<World>()
                .AndIf(Id != null, w => w.Id == Id)
                .AndIf(Query != null, w => w.Name.Contains(Query ?? ""))
                .AndIf(OwnerId != null, w => w.OwnerId == OwnerId)
                .AndIf(Location != null, w => w.Location == Location)
                .AndIf(true, w => true);
        }
    }
}
