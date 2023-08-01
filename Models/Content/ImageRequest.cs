using SardCoreAPI.Models.Hub.Worlds;
using SardCoreAPI.Utility.Files;
using System.Text.Json.Serialization;

namespace SardCoreAPI.Models.Content
{
    public class ImageRequest
    {
        public int Id { get; set; }
        public int? Id2 { get; set; }
        public ImageType? Type { get; set; }
        public string? URL { get; set; }
        public string? WorldPath { get; set; }
        [JsonIgnore]
        public WorldInfo? WorldInfo { get; set; }

        public string Directory
        {
            get
            {
                switch (Type)
                {
                    case ImageType.MapIcon:
                        return $"./storage/{WorldInfo?.WorldLocation}/mapicons/";
                    case ImageType.LocationIcon:
                        return $"./storage/{WorldInfo?.WorldLocation}/location/";
                    case ImageType.LocationTypeIcon:
                        return $"./storage/{WorldInfo?.WorldLocation}/location/types/";
                    case ImageType.DataPointImage:
                        return $"./storage/{WorldInfo?.WorldLocation}/data/";
                    default:
                        return $"./storage/{WorldInfo?.WorldLocation}/misc/";
                }
            }
        }

        public string FileName
        {
            get
            {
                switch (Type)
                {
                    case ImageType.DataPointImage:
                        return "Parameter-" + Id + " - " + Id2 + ".png";
                    case ImageType.MapIcon:
                        return "Map-" + Id + ".png";
                    case ImageType.LocationIcon:
                        return "Location-Icon-" + Id + ".png";
                    case ImageType.LocationTypeIcon:
                        return "Location-Type-" + Id + ".png";
                    default:
                        return Id + ".png";
                }
            }
        }

        public string FilePath
        {
            get
            {
                return Directory + FileName;
            }
        }

        public enum ImageType
        {
            MapIcon = 0,
            LocationTypeIcon = 1,
            LocationIcon = 2,
            DataPointImage = 3
        }
    }
}
