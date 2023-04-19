namespace SardCoreAPI.Models.Map.Area
{
    public class Area
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? SubregionId { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
