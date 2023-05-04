namespace SardCoreAPI.Models.Map.Area
{
    public class CelestialObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? CelestialSystemId { get; set; }
        public string? CelestialSystemName { get; set; }
        public int? ManifoldId { get; set; }
        public string? ManifoldName { get; set; }
    }
}
