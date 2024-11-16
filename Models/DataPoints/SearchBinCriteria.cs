using static SardCoreAPI.Models.DataPoints.ParameterSearchOptions;

namespace SardCoreAPI.Models.DataPoints
{
    public class SearchBinCriteria
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string TypeValue { get; set; }
        public List<int> Parameters { get; set; }
        public FilterModeEnum FilterMode { get; set; }
    }
}
