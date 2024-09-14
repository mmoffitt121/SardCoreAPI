using System.Reflection.Metadata;

namespace SardCoreAPI.Models.Map.Location
{
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int LocationTypeId { get; set; }
        public LocationType.LocationType? LocationType { get; set; }
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
        public int? IconSize { get; set; }

        public void CopyValuesFrom(Location from)
        {
            Name = from.Name;
            LocationTypeId = from.LocationTypeId;
            LocationTypeName = from.LocationTypeName;
            LayerId = from.LayerId;
            Longitude = from.Longitude;
            Latitude = from.Latitude;
            ParentId = from.ParentId;
            ZoomProminenceMin = from.ZoomProminenceMin;
            ZoomProminenceMax = from.ZoomProminenceMax;
            UsesIcon = from.UsesIcon;
            UsesLabel = from.UsesLabel;
            IconURL = from.IconURL;
            LabelFontSize = from.LabelFontSize;
            LabelFontColor = from.LabelFontColor;
            IconSize = from.IconSize;
        }

        public Location ConsumeTypeData(LocationType.LocationType? type)
        {
            LocationTypeName = LocationTypeName ?? type?.Name;
            UsesIcon = UsesIcon ?? type?.UsesIcon;
            UsesLabel = UsesLabel ?? type?.UsesLabel;
            IconURL = IconURL ?? type?.IconURL;
            LabelFontSize = LabelFontSize ?? type?.LabelFontSize;
            LabelFontColor = LabelFontColor ?? type?.LabelFontColor;
            IconSize = IconSize ?? type?.IconSize;
            LocationType = null;
            ZoomProminenceMin = ZoomProminenceMin ?? type?.ZoomProminenceMin;
            ZoomProminenceMax = ZoomProminenceMax ?? type?.ZoomProminenceMax;
            return this;
        }
    }
}
