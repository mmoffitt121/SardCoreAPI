using System.IO;

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
                }
                catch (IOException)
                {
                    await Task.Delay(retryDelay);
                }
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
    }
}
