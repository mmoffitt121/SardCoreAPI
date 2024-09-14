namespace SardCoreAPI.Models.Map.MapLayer
{
    public class MapLayer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Summary { get; set; }
        public int? MapId { get; set; }
        public bool? IsBaseLayer { get; set; }
        public bool? IsIconLayer { get; set; }
        public string? IconURL { get; set; }
        public List<PersistentZoomLevel>? PersistentZoomLevels { get; set; }
    }
}
