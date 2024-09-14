using ImageMagick;
using Microsoft.AspNetCore.Routing;
using SardCoreAPI.Database.DBContext;
using SardCoreAPI.Models.Hub.Worlds;
using SardCoreAPI.Models.Map.MapTile;
using SardCoreAPI.Services.Content;
using SardCoreAPI.Services.Context;
using SardCoreAPI.Services.Tasks;
using SardCoreAPI.Utility.Files;

namespace SardCoreAPI.Services.Maps
{
    public class PutMapTilesTask : SardTask
    {
        public static readonly double SLICING_PROGRESS_MIN = 30;
        public static readonly double SLICING_PROGRESS_MAX = 97;

        public string WorldLocation { get; set; }
        public string ImageId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int LayerId { get; set; }
        public string? ReplaceMode { get; set; }
        public bool? ReplaceRoot { get; set; }

        private static readonly int TILE_BATCH_SIZE = 10;

        public PutMapTilesTask(string worldLocation, string imageId, TileUploadRequest request) : base("Create Map Tiles", worldLocation)
        {
            WorldLocation = worldLocation;
            ImageId = imageId;
            X = request.X;
            Y = request.Y;
            Z = request.Z;
            LayerId = request.LayerId;
            ReplaceMode = request.ReplaceMode;
            ReplaceRoot = request.ReplaceRoot;
        } 

        public override async Task Run(CancellationToken cancellationToken, IServiceScopeFactory serviceScopeFactory)
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                IDataService dataService = scope.ServiceProvider.GetRequiredService<IDataService>();
                IContentService contentService = scope.ServiceProvider.GetRequiredService<IContentService>();

                await dataService.StartUsingWorldContext(new WorldInfo(WorldLocation));
                await GenerateMapTiles(dataService, contentService);
                await dataService.EndUsingWorldContext();
            }
        }

        private async Task GenerateMapTiles(IDataService data, IContentService contentService)
        {
            (byte[], string) imageData = await contentService.Image(ImageId);

            using (var stream = new MemoryStream(imageData.Item1))
            {
                using (var image = new MagickImage(stream))
                {
                    SetProgress(10, "Readying image...");
                    ResizeImage(image);

                    List<MapTile> mapTiles = new List<MapTile>();

                    var resized = image.Clone();

                    int subdivisions = (int)Math.Log2(image.Width / 256);

                    SetProgress(20, "Preparing to slice tiles...");
                    int iTransform = X;
                    int jTransform = Y;
                    for (int k = 0; k <= subdivisions; k++)
                    {
                        resized = image.Clone();
                        resized.Resize(256 * (int)Math.Pow(2, k), 256 * (int)Math.Pow(2, k)); // Bounds error
                        for (int i = 0; i < Math.Pow(2, k); i++)
                        {
                            for (int j = 0; j < Math.Pow(2, k); j++)
                            {
                                SetProgress((int)((double)k / (double)subdivisions * (SLICING_PROGRESS_MAX - SLICING_PROGRESS_MIN) + SLICING_PROGRESS_MIN),
                                    "Slicing map tiles...");

                                var geometry = new MagickGeometry(i * 256, j * 256, 256, 256);
                                var cloned = resized.Clone(geometry);
                                var stats = cloned.Statistics();
                                double? average = stats.GetChannel(PixelChannel.Alpha)?.Mean;
                                if (average.HasValue && average != 0)
                                {
                                    mapTiles.Add(new MapTile(k + Z, i + iTransform, j + jTransform, LayerId, cloned.ToByteArray()));
                                }

                                if (mapTiles.Count() >= TILE_BATCH_SIZE)
                                {
                                    await SaveTiles(mapTiles, data);
                                    mapTiles = new List<MapTile>();
                                }
                            }
                        }
                        iTransform *= 2;
                        jTransform *= 2;
                    }

                    if (mapTiles.Count() > 0)
                    {
                        SetProgress(98, "Saving last set of tiles...");
                        await SaveTiles(mapTiles, data);
                    }
                }
            }
        }

        private async Task SaveTiles(List<MapTile> mapTiles, IDataService data)
        {
            FileHandler fh = new FileHandler();
            List<Task> tasks = new List<Task>();

            foreach (MapTile tile in mapTiles)
            {
                // Load the old tile, if it exists, and delete it
                MapTile? oldTile = data.Context.MapTile.Where(mt => mt.X.Equals(tile.X) && mt.Y.Equals(tile.Y) && mt.Z.Equals(tile.Z) && mt.LayerId.Equals(tile.LayerId)).SingleOrDefault();
                if (oldTile != null)
                {
                    data.Context.Remove(oldTile);
                }
                // Create new tile
                tile.Size = tile.Tile.LongLength;
                data.Context.MapTile.Add(tile);
                
                // Save tile data to disk
                tasks.Add(fh.SaveImage(ImagePath(data.WorldLocation) + "/" + tile.FileLocation, "/" + tile.TruncatedFileName, tile.Tile));
            }

            tasks.Add(data.Context.SaveChangesAsync());
            await Task.WhenAll(tasks);
        }

        public string ImagePath(string worldLocation)
        {
            return "./storage/" + worldLocation + "/maptiles/";
        }

        /// <summary>
        /// Make sure to dispose of the Images this creates.
        /// </summary>
        /// <param name="file">A file containing an image, sized a multiple of 256, and in a square shape.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async static Task<MapTile[]> Slice(byte[] byteImage, int rootZ, int rootX, int rootY, int layerId)
        {
            using (var stream = new MemoryStream(byteImage))
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
