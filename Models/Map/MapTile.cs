using ImageMagick;

namespace SardCoreAPI.Models.Map
{
    public class MapTile
    {
        public int Z { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public IMagickImage Tile { get; set; }

        public MapTile(int Z, int X, int Y, IMagickImage Tile)
        {
            this.Z = Z;
            this.X = X;
            this.Y = Y;
            this.Tile = Tile;
        }
    }
}
