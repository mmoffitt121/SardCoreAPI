namespace SardCoreAPI.Models.Map.Area
{
    public class Subregion
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? RegionId { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
