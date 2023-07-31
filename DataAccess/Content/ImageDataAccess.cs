using SardCoreAPI.Models.Content;
using SardCoreAPI.Models.Hub.Worlds;
using SardCoreAPI.Utility.Files;

namespace SardCoreAPI.DataAccess.Content
{
    public class ImageDataAccess : GenericDataAccess
    {
        public async Task<byte[]> GetImage(ImageRequest request)
        {
            return await new FileHandler().LoadImage(request);
        }

        public async Task<int> PostImage(ImagePostRequest request)
        {
            switch (request.Type) 
            {
                case ImageRequest.ImageType.LocationTypeIcon:
                case ImageRequest.ImageType.LocationIcon:
                    await new FileHandler().SaveAndResizeImage(request, 64, 64);
                    break;
                default:
                    await new FileHandler().SaveImage(request);
                    break;
            }

            return 1;
        }

        public async Task<int> DeleteImage(ImageRequest request)
        {
            return await new FileHandler().DeleteImage(request);
        }

        public async Task<int> PutImageUrl(ImagePostRequest request)
        {
            if (request.URL == null)
            {
                return -1;
            }

            string sql;
            switch (request.Type) {
                case ImageRequest.ImageType.LocationTypeIcon:
                    sql = "UPDATE LocationTypes SET IconURL = @URL WHERE Id = @Id";
                    break;
                case ImageRequest.ImageType.LocationIcon:
                    sql = "UPDATE Locations SET IconURL = @URL WHERE Id = @Id";
                    break;
                default:
                    sql = "SELECT 0";
                    break;
            }

            return await Execute(sql, request, request.WorldInfo);
        }
    }
}
