using SardCoreAPI.Models.Content;
using SardCoreAPI.Models.Map.Location;
using SardCoreAPI.Models.Map.LocationType;
using SardCoreAPI.Services.Context;
using SardCoreAPI.Services.WorldContext;
using SardCoreAPI.Utility.Files;
using System.Collections.Specialized;

namespace SardCoreAPI.Services.Content
{
    public interface IContentService
    {
        public Task<byte[]> GetImage(ImageRequest request);
        public Task<byte[]> Image(string id);
        public Task<string> PostImage(ImagePostRequest request);
        public Task<int> DeleteImage(int id);
        public Task<int> PutImageUrl(ImagePostRequest request);
    }
    public class ContentService : IContentService
    {
        private IDataService _data;
        private IWorldInfoService _worldInfo;

        public int retryCount = 1;
        // Time between retries in ms
        public int retryDelay = 1;
        public ContentService(IDataService data)
        {
            this._data = data;
        }

        public async Task<byte[]> GetImage(ImageRequest request)
        {
            return await new FileHandler().LoadImage(request);
        }

        public async Task<byte[]> Image(string id)
        {
            _data.Context.Image.Fir
            string directory = _worldInfo.WorldLocation.ToString() + "/";
            string filePath = directory + id.Replace('_', '/');
        }

        public async Task<string> PostImage(ImagePostRequest request)
        {
            if (request.Data == null)
            {
                throw new ArgumentException("Image data cannot be null");
            }

            string id = CreateId();
            while (_data.Context.Image.SingleOrDefault(i => i.Id.Equals(id)) != null)
            {
                id = CreateId();
            }

            Image image = new Image()
            {
                Id = id,
                Name = request.Data.FileName,
                Size = request.Data.OpenReadStream().Length,
                Description = request.Description ?? "",
                CreationDate = DateTime.Now,
            };
            _data.Context.Image.Add(image);
            _data.Context.SaveChanges();
            await SaveImage(image.Id, request);

            return image.Id;
        }

        private static string CreateId()
        {
            return "Image_" + DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + Guid.NewGuid();
        }

        private async Task SaveImage(string id, ImagePostRequest request)
        {
            string directory = _worldInfo.WorldLocation.ToString() + "/";
            string filePath = directory + id.Replace('_', '/');
            Directory.CreateDirectory(directory);
            for (int i = 0; i < retryCount; i++)
            {
                try
                {
                    File.WriteAllBytes(filePath, await request.GetByteArray());
                    return;
                }
                catch (IOException)
                {
                    await Task.Delay(retryDelay);
                }
                throw new Exception();
            }
        }

        public async Task<int> DeleteImage(int id)
        {
            return 0;
        }

        public async Task<int> PutImageUrl(ImagePostRequest request)
        {/*if (request.URL == null)
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

            return await _data.Context.SaveChangesAsync();*/
            return 0;
        }
    }
}
