using ImageMagick;
using System;
using SardCoreAPI.Models.Map.MapTile;
using Microsoft.AspNetCore.SignalR;
using SardCoreAPI.Utility.Progress;

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
        public async static Task<MapTile[]> Slice(IFormFile file, int rootZ, int rootX, int rootY, int layerId)
        {
            using (var stream = file.OpenReadStream())
            {
                using (var image = new MagickImage(stream))
                {
                    ResizeImage(image);

                    List<MapTile> mapTiles = new List<MapTile>();

                    var resized = image.Clone();

                    int subdivisions = (int)Math.Log2(image.Width / 256);

                    int iTransform = rootX;
                    int jTransform = rootY;
                    for (int k = 0; k <= subdivisions; k++)
                    {
                        resized = image.Clone();
                        resized.Resize(256 * (int)Math.Pow(2, k), 256 * (int)Math.Pow(2, k)); // Bounds error
                        for (int i = 0; i < Math.Pow(2, k); i++)
                        {
                            for (int j = 0; j < Math.Pow(2, k); j++)
                            {
                                var geometry = new MagickGeometry(i * 256, j * 256, 256, 256);
                                var cloned = resized.Clone(geometry);
                                var stats = cloned.Statistics();
                                double? average = stats.GetChannel(PixelChannel.Alpha)?.Mean;
                                if (average.HasValue && average != 0)
                                {
                                    mapTiles.Add(new MapTile(k + rootZ, i + iTransform, j + jTransform, layerId, cloned.ToByteArray()));
                                }
                            }
                        }
                        iTransform *= 2;
                        jTransform *= 2;
                    }
                    return mapTiles.ToArray();
                }
            }
        }

        public static void ResizeImage(MagickImage image)
        {
            int newSize;
            MagickGeometry geometry = new MagickGeometry();

            if (image.Width > image.Height)
            {
                newSize = (int)Math.Pow(2, Math.Ceiling(Math.Log2(image.Width)));
            }
            else
            {
                newSize = (int)Math.Pow(2, Math.Ceiling(Math.Log2(image.Height)));
            }

            image.HasAlpha = true;
            image.Format = MagickFormat.Png;

            geometry.Width = newSize;
            geometry.Height = newSize;
            image.Resize(geometry);

            using (var newImage = new MagickImage(MagickColor.FromRgba(0, 0, 0, 0), newSize, newSize))
            {
                newImage.Composite(image, CompositeOperator.Copy);
                geometry.IgnoreAspectRatio = true;
                image.Resize(geometry);
                image.Composite(newImage, CompositeOperator.Replace);
                Console.WriteLine(image.Width);
            }
        }
    }
}
