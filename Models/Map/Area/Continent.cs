namespace SardCoreAPI.Models.Map.Area
{
    public class Continent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? CelestialObjectId { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public bool? Aquatic { get; set; }
    }
}
