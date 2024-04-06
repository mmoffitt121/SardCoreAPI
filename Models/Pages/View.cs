using SardCoreAPI.Models.DataPoints;

namespace SardCoreAPI.Models.Pages
{
    public class View
    {
        public string? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ViewType { get; set; }
        public DataPointSearchCriteriaOptions? SearchCriteriaOptions { get; set; }

        public static HashSet<string> viewTypes = new HashSet<string>()
        {
            "Card",
            "List"
        };
    }
}
