using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace SardCoreAPI.Models.Map.MapTile
{
    [PrimaryKey("X", "Y", "Z", "LayerId")]
    public class MapTile
    {
        public int Z { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int LayerId { get; set; }
        public long Size { get; set; }
        [NotMapped]
        public byte[] Tile { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null) 
            {
                return false;
            }

            if (obj is not MapTile tile)
            {
                return false;
            }

            return tile.Z == Z && tile.X == X && tile.Y == Y && tile.LayerId == LayerId;
        }

        public string FileName
        {
            get
            {
                 return FileLocation + "/" + X + "." + Y + ".png";
            }
        }

        public string TruncatedFileName
        {
            get
            {
                return X + "." + Y + ".png";
            }
        }

        public string FileLocation
        {
            get
            {
                return LayerId + "/" + Z;
            }
            
        }

        public MapTile(int Z, int X, int Y, int layerId, byte[] Tile)
        {
            this.Z = Z;
            this.X = X;
            this.Y = Y;
            this.LayerId = layerId;
            this.Tile = Tile;
        }

        public MapTile()
        {
            Tile = new byte[0];
        }
    }
}
