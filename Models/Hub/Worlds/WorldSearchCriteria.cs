using Newtonsoft.Json;
using SardCoreAPI.Models.Common;

namespace SardCoreAPI.Models.Hub.Worlds
{
    public class WorldSearchCriteria : PagedSearchCriteria
    {
        public string? OwnerId { get; set; }
        public string? OwnerName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public OrderOptions? OrderBy { get; set; }
        public bool? OrderDesc { get; set; }

        public enum OrderOptions
        {
            CreatedDate,
            Name
        }

        [JsonIgnore]
        public string OrderByString
        {
            get
            {
                string orderDirection = (OrderDesc == null || OrderDesc == false) ? ("ASC") : ("DESC");
                string orderBy;
                if (OrderBy == null)
                {
                    orderBy = "Name";
                }
                else
                {
                    switch (OrderBy)
                    {
                        case OrderOptions.Name: 
                            orderBy = "Name"; 
                            break;
                        case OrderOptions.CreatedDate:
                            orderBy = "CreatedDate";
                            break;
                        default:
                            orderBy = "Name"; 
                            break;
                    }
                }

                return orderBy + " " + orderDirection;
            }
        }
    }
}
