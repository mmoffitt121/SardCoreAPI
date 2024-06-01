using SardCoreAPI.Models.DataPoints.DataPointParameters;

namespace SardCoreAPI.Models.DataPoints
{
    public class DataPointSearchCriteriaOptions
    {
        public DataPointSearchCriteria Criteria { get; set; }
        public List<DataPointTypeParameter>? UserSortParameters { get; set; }
        public List<DataPointTypeParameter>? UserFilterParameters { get; set; }
    }
}
