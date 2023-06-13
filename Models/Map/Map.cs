namespace SardCoreAPI.Models.Map
{
    public class Map
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool? Loops { get; set; }
        public int? AreaZoomProminence { get; set; }
        public int? SubregionZoomProminence { get; set; }
        public int? RegionZoomProminence { get; set; }
        public int? SubcontinentZoomProminence { get; set; }
        public int? ContinentZoomProminence { get; set; }
        public float? DefaultZ { get; set; }
        public float? DefaultX { get; set; }
        public float? DefaultY { get; set; }
        public int? MinZoom { get; set; }
        public int? MaxZoom { get; set; }
        public bool? IsDefault { get; set; }
        public byte[]? Icon { get; set; }
    }
}
