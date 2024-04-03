using SardCoreAPI.Models.DataPoints;

namespace SardCoreAPI.Models.Pages
{
    public class View
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DataPointSearchCriteriaOptions SearchCriteriaOptions { get; set; }
    }
}
