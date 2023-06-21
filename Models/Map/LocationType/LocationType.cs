namespace SardCoreAPI.Models.Map.LocationType
{
    public class LocationType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Summary { get; set; }
        public int? ParentTypeId { get; set; }
        public bool? AnyTypeParent { get; set; }
        public string? IconPath { get; set; }
        public int? ZoomProminence { get; set; }
    }
}
