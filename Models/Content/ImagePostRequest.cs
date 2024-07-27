using SardCoreAPI.Utility.Files;

namespace SardCoreAPI.Models.Content
{
    public class ImagePostRequest
    {
        public IFormFile? Data { get; set; }
        public string? Description { get; set; }    

        public async Task<byte[]> GetByteArray()
        {
            byte[] bytes = await new FileHandler().FormToByteArray(Data);
            return bytes;
        }
    }
}
