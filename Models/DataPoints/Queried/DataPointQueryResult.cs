using SardCoreAPI.Models.DataPoints.DataPointParameters;

namespace SardCoreAPI.Models.DataPoints.Queried
{
    public class DataPointQueryResult
    {
        public List<QueriedDataPoint> Results { get; set; }
        public int Count { get; set; }
        public List<DataPointType>? Types { get; set; }

        public DataPointQueryResult() { }

        public DataPointQueryResult(int count, List<QueriedDataPoint> results, List<DataPointType> types)
        {
            Count = count;
            Types = types;
            Results = results;
        }
    }
}
