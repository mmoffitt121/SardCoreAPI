using SardCoreAPI.Models.Map.MapLayer;

namespace SardCoreAPI.Models.Map
{
    public class Map
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Summary { get; set; }
        public bool? Loops { get; set; }
        public float? DefaultZ { get; set; }
        public float? DefaultX { get; set; }
        public float? DefaultY { get; set; }
        public int? MinZoom { get; set; }
        public int? MaxZoom { get; set; }
        public bool? IsDefault { get; set; }
        public string? IconId { get; set; }
    }
}
