using System.Drawing;
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

        public async Task<byte[]> LoadImage(string filePath)
        {
            byte[] data;

            for (int i = 0; i < retryCount; i++)
            {
                try
                {
                    data = File.ReadAllBytes(filePath);
                    return data;
                }
                catch (IOException)
                {
                    await Task.Delay(retryDelay);
                }
            }

            throw new Exception();
        }

        public async Task<int> DeleteImage(string fileDirectory, string fileName)
        {
            for (int i = 0; i < retryCount; i++)
            {
                try
                {
                    File.Delete(fileDirectory + fileName);
                    return 1;
                }
                catch (FileNotFoundException e)
                {
                    return 0;
                }
                catch (DirectoryNotFoundException e)
                {
                    return 0;
                }
                catch (IOException)
                {
                    await Task.Delay(retryDelay);
                }
            }
            throw new FileLoadException("File could not be deleted");
        }

        public async Task<byte[]> ResizeImage(byte[] data, int x, int y)
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
