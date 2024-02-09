using System.Text.Json.Serialization;

namespace SardCoreAPI.Models.DataPoints
{
    public class ParameterSearchOptions
    {
        public int DataPointTypeParameterId { get; set; }
        public FilterModeEnum FilterMode { get; set; }
        [JsonIgnore]
        public int SequenceId { get; set; }
        public enum FilterModeEnum
        {
            Equals = 0,
            Contains = 1,
            StartsWith = 2,
            EndsWith = 3,
            GreaterThan = 5,
            LessThan = 6
        }
    }
}
