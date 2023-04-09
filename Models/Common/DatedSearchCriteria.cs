namespace SardCoreAPI.Models.Common
{
    public class DatedSearchCriteria
    {
        public string? Query { get; set; }
        public double? BeginDate { get; set; }
        public double? EndDate { get; set; }
        public int? Era { get; set; }
    }
}
