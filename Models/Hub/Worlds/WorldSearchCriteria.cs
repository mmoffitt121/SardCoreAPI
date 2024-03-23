using Newtonsoft.Json;
using SardCoreAPI.Models.Common;

namespace SardCoreAPI.Models.Hub.Worlds
{
    public class WorldSearchCriteria : PagedSearchCriteria
    {
        public string? OwnerId { get; set; }
        public string? OwnerName { get; set; }
        public string? Location { get; set; }
    }
}
