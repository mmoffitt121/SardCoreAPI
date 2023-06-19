using SardCoreAPI.Models.Content;
using SardCoreAPI.Utility.Files;

namespace SardCoreAPI.DataAccess.Content
{
    public class ImageDataAccess
    {
        public async Task<int> PostImage(ImageRequest request)
        {
            await new FileHandler().SaveImage(request);
            return 1;
        }

        public async Task<byte[]> GetImage(int id)
        {
            return new byte[6];
        }
    }
}
