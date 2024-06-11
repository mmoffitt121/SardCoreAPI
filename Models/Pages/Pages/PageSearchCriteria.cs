using SardCoreAPI.Models.Easy;

namespace SardCoreAPI.Models.Pages.Pages
{
    public class PageSearchCriteria : QueryOptions
    {
        public List<string>? Ids { get; set; }
        public bool IncludePageData { get; set; } = false;
    }
}
