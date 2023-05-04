﻿namespace SardCoreAPI.Models.Map.Area
{
    public class Subregion
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int? RegionId { get; set; }
        public string? RegionName { get; set; }
        public int? SubcontinentId { get; set; }
        public string? SubcontinentName { get; set; }
        public int? ContinentId { get; set; }
        public string? ContinentName { get; set; }
        public int? CelestialObjectId { get; set; }
        public string? CelestialObjectName { get; set; }
        public int? CelestialSystemId { get; set; }
        public string? CelestialSystemName { get; set; }
        public int? ManifoldId { get; set; }
        public string? ManifoldName { get; set; }
    }
}
