namespace SardCoreAPI.Models.Common
{
    public class ImageUploadRequest
    {
        public int Id { get; set; }
        public IFormFile Data { get; set; }
    }
}
