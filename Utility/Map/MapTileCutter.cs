using ImageMagick;
using System;
using SardCoreAPI.Models.Map;

namespace SardCoreAPI.Utility.Map
{
    public static class MapTileCutter
    {
        /// <summary>
        /// Make sure to dispose of the Images this creates.
        /// </summary>
        /// <param name="file">A file containing an image, sized a multiple of 256, and in a square shape.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static MapTile[] Slice(IFormFile file, int rootZ, int rootX, int rootY)
        {
            using (var stream = file.OpenReadStream())
            {
                using (var image = new MagickImage(stream))
                {
                    // If the input image is not a power of 2 above 256 in size, or is not a square, throw.
                    if (image.Width != image.Height || Math.Ceiling(Math.Log2(image.Width)) != Math.Floor(Math.Log2(image.Width)) || image.Width < 256)
                    {
                        throw new Exception("Input image was an invalid size. Images must be squares, sized a multiple of two.");
                    }

                    List<MapTile> mapTiles = new List<MapTile>();

                    // Resize and dd original image to the array
                    var resized = image.Clone();
                    resized.Resize(256, 256);
                    mapTiles.Add(new MapTile(rootZ, rootX, rootY, resized.ToByteArray()));
                    

                    int subdivisions = (int)Math.Log2(image.Width / 256);

                    for (int k = 1; k < subdivisions; k++)
                    {
                        resized = image.Clone();
                        resized.Resize(256 * (int)Math.Pow(2, k), 256 * (int)Math.Pow(2, k)); // Bounds error
                        for (int i = 0; i < Math.Pow(2, k); i++)
                        {
                            for (int j = 0; j < Math.Pow(2, k); j++)
                            {
                                var geometry = new MagickGeometry(i * 256, j * 256, 256, 256);
                                var cloned = resized.Clone(geometry);
                                mapTiles.Add(new MapTile(k + rootZ, i, j, cloned.ToByteArray()));
                            }
                        }
                    }
                    return mapTiles.ToArray();
                }
            }
        }
    }
}
