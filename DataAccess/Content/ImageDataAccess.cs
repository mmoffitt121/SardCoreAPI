using SardCoreAPI.Models.Content;
using SardCoreAPI.Utility.Files;

namespace SardCoreAPI.DataAccess.Content
{
    public class ImageDataAccess
    {
        public async Task<byte[]> GetImage(ImageRequest request)
        {
            return await new FileHandler().LoadImage(request);
        }

        public async Task<int> PostImage(ImagePostRequest request)
        {
            await new FileHandler().SaveImage(request);
            return 1;
        }

        public async Task<int> DeleteImage(ImageRequest request)
        {
            return await new FileHandler().DeleteImage(request);
        }
    }
}
