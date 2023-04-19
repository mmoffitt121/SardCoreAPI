namespace SardCoreAPI.Models.Map.Area
{
    public class Region
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? SubcontinentId { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
