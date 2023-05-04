using System.Reflection.Metadata;

namespace SardCoreAPI.Models.Map.Location
{
    public class Location
    {
        public int Id { get; set; }
        public string LocationName { get; set; }
        public int? LocationTypeId { get; set; }
        public string? LocationType { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public int? AreaId { get; set; }
        public string? AreaName { get; set; }
        public int? SubregionId { get; set; }
        public string? SubregionName { get; set; }
        public int? RegionId { get; set; }
        public string? RegionName { get; set; }
        public int? SubcontinentId { get; set; }
        public string? SubcontinentName { get; set; }
        public int? ContinentId { get; set; }
        public string? ContinentName { get; set; }
        public int? CelestialBodyId { get; set; }
        public string? CelestialBodyName { get; set; }
        public int? CelestialSystemId { get; set; }
        public string? CelestialSystemName { get; set; }
        public int? ManifoldId { get; set; }
        public string? ManifoldName { get; set; }
    }
}
