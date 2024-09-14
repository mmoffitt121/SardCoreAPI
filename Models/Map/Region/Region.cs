namespace SardCoreAPI.Models.Map.Region
{
    public class Region
    {
        public int? Id { get; set; }
        public int LocationId { get; set; }
        public string Name { get; set; }
        public string Shape { get; set; }
        public bool ShowByDefault { get; set; }
        public string Color { get; set; }

        public void CopyValuesFrom(Region region)
        {
            LocationId = region.LocationId;
            Name = region.Name;
            Shape = region.Shape;
            ShowByDefault = region.ShowByDefault;
            Color = region.Color;
        }
    }
}
