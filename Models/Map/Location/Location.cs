using System.Reflection.Metadata;

namespace SardCoreAPI.Models.Map.Location
{
    public class Location
    {
        public int Id { get; set; }
        public string LocationName { get; set; }
        public int? LocationTypeId { get; set; }
        public string? LocationType { get; set; }
        public string? LocationTypeSummary { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public int? ParentId { get; set; }
        public int? ZoomProminence { get; set; }
    }
}
