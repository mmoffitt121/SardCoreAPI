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
        public Task<int> PostImage(ImagePostRequest request);
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
