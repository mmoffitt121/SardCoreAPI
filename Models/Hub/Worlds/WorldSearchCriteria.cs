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
                .OrIf(Id != null, w => w.Id == Id)
                .OrIf(Query != null, w => w.Name.Contains(Query ?? ""))
                .OrIf(OwnerId != null, w => w.OwnerId == OwnerId)
                .OrIf(Location != null, w => w.Location == Location);
        }
    }
}
