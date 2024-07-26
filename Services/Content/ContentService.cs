using SardCoreAPI.Models.Content;
using SardCoreAPI.Models.Map.Location;
using SardCoreAPI.Models.Map.LocationType;
using SardCoreAPI.Services.Context;
using SardCoreAPI.Utility.Files;

namespace SardCoreAPI.Services.Content
{
    public interface IContentService
    {
        public Task<byte[]> GetImage(ImageRequest request);
        public Task<string> PostImage(ImagePostRequest request);
        public Task<int> DeleteImage(ImageRequest request);
        public Task<int> PutImageUrl(ImagePostRequest request);
    }
    public class ContentService : IContentService
    {
        private IDataService _data;
        public ContentService(IDataService data)
        {
            this._data = data;
        }

        public async Task<byte[]> GetImage(ImageRequest request)
        {
            return await new FileHandler().LoadImage(request);
        }

        public async Task<string> PostImage(ImagePostRequest request)
        {
            if (request.Data == null)
            {
                throw new ArgumentException("Image data cannot be null");
            }

            Image image = new Image()
            {
                Id = "Image_" + DateTime.Now.ToString(),
                Name = request.Data.FileName,
                Size = request.Data.OpenReadStream().Length,
                Description = request.Description ?? "",
                Url = request.URL ?? "",
                CreationDate = DateTime.Now,
            };
            _data.Context.Image.Add(image);
            _data.Context.SaveChanges();
            await new FileHandler().SaveImage(request);

            return image.Id;
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

            switch (request.Type)
            {
                case ImageRequest.ImageType.LocationTypeIcon:
                    LocationType lt = _data.Context.LocationType.Single(lt => lt.Id.Equals(request.Id));
                    lt.IconURL = request.URL;
                    _data.Context.Update(lt);
                    break;
                case ImageRequest.ImageType.LocationIcon:
                    Location l = _data.Context.Location.Single(l => l.Id.Equals(request.Id));
                    l.IconURL = request.URL;
                    _data.Context.Update(l);
                    break;
                default:
                    throw new NotImplementedException();
            }

            return await _data.Context.SaveChangesAsync();
        }
    }
}
