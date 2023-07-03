using System.Reflection.Metadata;

namespace SardCoreAPI.Models.Map.Location
{
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? LocationTypeId { get; set; }
        public string? LocationTypeName { get; set; }
        public int LayerId { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public int? ParentId { get; set; }
        public int? ZoomProminenceMin { get; set; }
        public int? ZoomProminenceMax { get; set; }
        public bool? UsesIcon { get; set; }
        public bool? UsesLabel { get; set; }
        public string? IconURL { get; set; }
        public string? LabelFontSize { get; set; }
        public string? LabelFontColor { get; set; }
    }
}
