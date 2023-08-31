namespace SardCoreAPI.Models.Map.Region
{
    public class Region
    {
        public int? Id { get; set; }
        public int LocationId { get; set; }
        public string Name { get; set; }
        public string Shape { get; set; }
        public bool ShowByDefault { get; set; }
    }
}
