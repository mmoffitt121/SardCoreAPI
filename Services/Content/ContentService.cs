using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using SardCoreAPI.Models.Common;
using SardCoreAPI.Models.Content;
using SardCoreAPI.Models.Map.Location;
using SardCoreAPI.Models.Map.LocationType;
using SardCoreAPI.Services.Context;
using SardCoreAPI.Services.WorldContext;
using SardCoreAPI.Utility.DataAccess;
using SardCoreAPI.Utility.Files;
using System.Collections.Specialized;

namespace SardCoreAPI.Services.Content
{
    public interface IContentService
    {
        public Task<List<Image>> GetImages(PagedSearchCriteria query);
        public Task<int> GetImageCount(PagedSearchCriteria query);
        public Task<(byte[], string)> Image(string id);
        public Task<(byte[], string)> Thumbnail(string id);
        public Task<ImagePostResponse> PostImage(ImagePostRequest request);
        public Task DeleteImage(string id);
        public Task<int> PutImageUrl(ImagePostRequest request);
    }

    public class ContentService : IContentService
    {
        private IDataService _data;
        private IWorldInfoService _worldInfo;

        public int retryCount = 1;
        // Time between retries in ms
        public int retryDelay = 1;
        private int THUMBNAIL_SIZE = 150;
        private string STORAGE_ROOT = "storage/";
        public ContentService(IDataService data, IWorldInfoService worldInfo)
        {
            _data = data;
            _worldInfo = worldInfo;
        }

        public async Task<List<Image>> GetImages(PagedSearchCriteria criteria)
        {
            if (criteria.PageSize == null || criteria.PageNumber == null)
            {
                return new List<Image>();
            }
            return await _data.Context.Image
                .Where(x => x.Description.Contains(criteria.Query ?? ""))
                .OrderByDescending(x => x.CreationDate)
                .Skip((int)criteria.PageSize * (int)criteria.PageNumber)
                .Take((int)criteria.PageSize)
                .ToListAsync();
        }

        public async Task<int> GetImageCount(PagedSearchCriteria criteria)
        {
            return await _data.Context.Image
                .Where(x => x.Id.Contains(criteria.Query ?? "") || x.Description.Contains(criteria.Query ?? ""))
                .CountAsync();
        }

        public async Task<(byte[], string)> Image(string id)
        {
            Image image = _data.Context.Image.Single(x => x.Id == id);

            string directory = GetImagePath(id);
            string filePath = directory + "image" + image.Extension;

            string fileType;
            switch (image.Extension)
            {
                case ".png":
                    fileType = "image/png";
                    break;
                case ".jpg":
                case ".jpeg":
                    fileType = "image/jpeg";
                    break;
                default:
                    throw new NotImplementedException("Unknown file type for file");
            }

            return (await new FileHandler().LoadImage(filePath), fileType);
        }

        public async Task<(byte[], string)> Thumbnail(string id)
        {
            Image image = _data.Context.Image.Single(x => x.Id == id);

            string directory = GetImagePath(id);
            string filePath = directory + "thumb" + image.Extension;

            string fileType;
            switch (image.Extension)
            {
                case ".png":
                    fileType = "image/png";
                    break;
                case ".jpg":
                case ".jpeg":
                    fileType = "image/jpeg";
                    break;
                default:
                    throw new NotImplementedException("Unknown file type for file");
            }

            return (await new FileHandler().LoadImage(filePath), fileType);
        }

        public async Task<ImagePostResponse> PostImage(ImagePostRequest request)
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

            // Do this early for validation
            string extension = GetFileExtension(request.Data.FileName);

            // Load thumbnail into memory
            FileHandler fileHandler = new FileHandler();
            byte[] thumb = await fileHandler.ResizeImage(await fileHandler.FormToByteArray(request.Data), THUMBNAIL_SIZE, THUMBNAIL_SIZE);

            // Get size of image and thumbnail
            long size;
            using (var thumbStream = new MemoryStream(thumb))
            {
                size = thumbStream.Length + request.Data.OpenReadStream().Length;
            }

            // Save image metadata to database
            Image image = new Image()
            {
                Id = id,
                Name = request.Data.FileName,
                Size = size,
                Description = request.Description ?? "",
                CreationDate = DateTime.Now,
                Extension = extension,
            };
            _data.Context.Image.Add(image);
            _data.Context.SaveChanges();

            // Save Images to files
            string directory = GetImagePath(id);
            string file = "image" + extension;
            string thumbnailFile = "thumb" + extension;
            
            await fileHandler.SaveImage(directory, file, await fileHandler.FormToByteArray(request.Data));
            await fileHandler.SaveImage(directory, thumbnailFile, thumb);

            return new ImagePostResponse(image.Id);
        }

        private string GetImagePath(string id)
        {
            return STORAGE_ROOT + _worldInfo.WorldLocation.ToString() + "/" + id.Replace('_', '/') + '/';
        }

        private string GetFileExtension(string fileName)
        {
            string extension = Path.GetExtension(fileName).ToLower();
            switch (extension)
            {
                case ".png":
                case ".jpg":
                case ".jpeg":
                    return extension;
                default:
                    throw new ArgumentException("Only files of type png, jpg, and jpeg are supported."); 
            }
        } 

        private static string CreateId()
        {
            return "Image_" + DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + Guid.NewGuid();
        }

        public async Task DeleteImage(string id)
        {
            string directory = _worldInfo.WorldLocation.ToString() + "/";
            string file = id.Replace('_', '/');
            string thumbnailFile = id.Replace('_', '/') + "-thumb";

            FileHandler fileHandler = new FileHandler();
            await fileHandler.DeleteImage(directory, file);
            await fileHandler.DeleteImage(directory, thumbnailFile);

            _data.Context.Image.Where(x => x.Id.Equals(id)).ExecuteDelete();
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
