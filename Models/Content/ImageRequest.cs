using SardCoreAPI.Utility.Files;

namespace SardCoreAPI.Models.Content
{
    public class ImageRequest
    {
        public int Id { get; set; }
        public int? Id2 { get; set; }
        public IFormFile Data { get; set; }
        public ImageType Type { get; set; }

        public string Directory
        {
            get
            {
                switch (Type)
                {
                    case ImageType.MapIcon:
                        return "./storage/mapicons/";
                    case ImageType.LocationIcon:
                        return "./storage/location/";
                    case ImageType.LocationTypeIcon:
                        return "./storage/location/types/";
                    case ImageType.DataPointImage:
                        return "./storage/data/";
                    default:
                        return "./storage/misc/";
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
                        return "Parameter " + Id + " - " + Id2 + ".png";
                    case ImageType.MapIcon:
                        return "Map Icon " + Id + ".png";
                    case ImageType.LocationIcon:
                        return "Location Icon " + Id + ".png";
                    case ImageType.LocationTypeIcon:
                        return "Location Type " + Id + ".png";
                    default:
                        return Id + ".png";
                }
            }
        }

        public string URL
        {
            get
            {
                return Directory + FileName;
            }
        }

        public async Task<byte[]> GetByteArray()
        {
            byte[] bytes = await new FileHandler().FormToByteArray(Data);
            return bytes;
        }

        public enum ImageType
        {
            MapIcon,
            LocationIcon,
            LocationTypeIcon,
            DataPointImage
        }
    }
}
