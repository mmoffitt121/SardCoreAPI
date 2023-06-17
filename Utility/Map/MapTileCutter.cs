using ImageMagick;
using System;
using SardCoreAPI.Models.Map.MapTile;
using Microsoft.AspNetCore.SignalR;
using SardCoreAPI.Utility.Progress;

namespace SardCoreAPI.Utility.Map
{
    public static class MapTileCutter
    {
        public static readonly int SLICING_PROGRESS_INTERVAL = 30;
        public static readonly double SLICING_PROGRESS_MIN = 40;
        public static readonly double SLICING_PROGRESS_MAX = 95;

        /// <summary>
        /// Make sure to dispose of the Images this creates.
        /// </summary>
        /// <param name="file">A file containing an image, sized a multiple of 256, and in a square shape.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async static Task<MapTile[]> Slice(IFormFile file, int rootZ, int rootX, int rootY, int layerId, IHubContext<ProgressManager> hubContext)
        {
            // Report Progress ---
            await hubContext.Clients.All.SendAsync("ProgressUpdate", 10, "Readying tiles for slicing...");

            using (var stream = file.OpenReadStream())
            {
                // Report Progress ---
                await hubContext.Clients.All.SendAsync("ProgressUpdate", 20, "Converting tiles...");
                using (var image = new MagickImage(stream))
                {
                    // If the input image is not a power of 2 above 256 in size, or is not a square, throw.
                    if (image.Width != image.Height || Math.Ceiling(Math.Log2(image.Width)) != Math.Floor(Math.Log2(image.Width)) || image.Width < 256)
                    {
                        throw new Exception("Input image was an invalid size. Images must be squares, sized a multiple of two.");
                    }

                    List<MapTile> mapTiles = new List<MapTile>();

                    // Report Progress ---
                    await hubContext.Clients.All.SendAsync("ProgressUpdate", 30, "Adjusting root image...");

                    // Resize and dd original image to the array
                    var resized = image.Clone();
                    resized.Resize(256, 256);
                    mapTiles.Add(new MapTile(rootZ, rootX, rootY, layerId, resized.ToByteArray()));

                    // Report Progress ---
                    await hubContext.Clients.All.SendAsync("ProgressUpdate", 40, "Slicing tiles...");

                    int subdivisions = (int)Math.Log2(image.Width / 256);

                    int iTransform = rootX;
                    int jTransform = rootY;
                    for (int k = 1; k <= subdivisions; k++)
                    {
                        double progress = (double)k / (double)subdivisions * (SLICING_PROGRESS_MAX - SLICING_PROGRESS_MIN) + SLICING_PROGRESS_MIN;
                        await hubContext.Clients.All.SendAsync("ProgressUpdate", progress, "Creating levels... (" + k + "/" + subdivisions + " levels)");
                        resized = image.Clone();
                        resized.Resize(256 * (int)Math.Pow(2, k), 256 * (int)Math.Pow(2, k)); // Bounds error
                        iTransform *= 2;
                        jTransform *= 2;
                        for (int i = 0; i < Math.Pow(2, k); i++)
                        {
                            for (int j = 0; j < Math.Pow(2, k); j++)
                            {
                                var geometry = new MagickGeometry(i * 256, j * 256, 256, 256);
                                var cloned = resized.Clone(geometry);
                                mapTiles.Add(new MapTile(k + rootZ, i + iTransform, j + jTransform, layerId, cloned.ToByteArray()));
                            }
                        }
                    }
                    return mapTiles.ToArray();
                }
            }
        }
    }
}
