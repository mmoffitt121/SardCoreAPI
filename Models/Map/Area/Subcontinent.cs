namespace SardCoreAPI.Models.Map.Area
{
    public class Subcontinent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ContinentId { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
