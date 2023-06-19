using System.IO;
using ImageMagick;
using Microsoft.AspNetCore.Routing;
using SardCoreAPI.Models.Content;
using SardCoreAPI.Models.Map.MapTile;

namespace SardCoreAPI.Utility.Files
{
    public class FileHandler
    {
        public int retryCount = 1;
        // Time between retries in ms
        public int retryDelay = 1;

        public async Task SaveImage(string fileDirectory, string fileName, byte[] data)
        {
            Directory.CreateDirectory(fileDirectory);
            for (int i = 0; i < retryCount; i++)
            {
                try
                {
                    File.WriteAllBytes(fileDirectory + fileName, data);
                    return;
                }
                catch (IOException)
                {
                    await Task.Delay(retryDelay);
                }
            }
            throw new Exception();
        }

        public async Task SaveImage(ImageRequest request)
        {
            Directory.CreateDirectory(request.Directory);
            for (int i = 0; i < retryCount; i++)
            {
                try
                {
                    File.WriteAllBytes(request.URL, await request.GetByteArray());
                    return;
                }
                catch (IOException)
                {
                    await Task.Delay(retryDelay);
                }
                throw new Exception();
            }
        }

        public async Task<byte[]> LoadImage(string fileName)
        {
            byte[] data;

            for (int i = 0; i < retryCount; i++)
            {
                try
                {
                    data = File.ReadAllBytes(fileName);
                    return data;
                }
                catch (FileNotFoundException e)
                {
                    throw e;
                }
                catch (DirectoryNotFoundException e)
                {
                    throw e;
                }
                catch (IOException)
                {
                    await Task.Delay(retryDelay);
                }
            }

            throw new Exception();
        }

        public async Task<byte[]> CompressImage(byte[] data, int x, int y)
        {
            using (var stream = new MemoryStream(data))
            {
                using (var image = new MagickImage(stream))
                {
                    image.Resize(x, y);
                    return image.ToByteArray();
                }
            }
        }

        public async Task<byte[]> FormToByteArray(IFormFile file)
        {
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                return stream.ToArray();
            }
        }
    }
}
