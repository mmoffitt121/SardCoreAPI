using Microsoft.Data.SqlClient;
using SardCoreAPI.Models.Document.SearchResults;
using Dapper;
using ImageMagick;
using SardCoreAPI.Models.Map.MapTile;

namespace SardCoreAPI.DataAccess.Map
{
    public class MapTileDataAccess
    {
        public MapTile GetTile(int Z, int X, int Y, int LayerId)
        {
            string sql = @"SELECT TOP (1) Tile FROM dbo.MapTiles 
                WHERE
                    Z = @Z AND
                    X = @X AND
                    Y = @Y AND
                    LayerId = @LayerId
            ";

            try
            {
                using (SqlConnection connection = new SqlConnection(Connection.GetConnectionString()))
                {
                    connection.Open();
                    List<MapTile> mapTiles = connection.Query<MapTile>(sql, new MapTile(Z, X, Y, LayerId, new byte[0])).ToList();
                    if (mapTiles.Count > 0)
                    {
                        return mapTiles.First();
                    }
                    else
                    {
                        return new MapTile(Z, X, Y, LayerId, new byte[0]);
                    }
                }
            }
            catch (SqlException s)
            {
                Console.WriteLine(s);
                return new MapTile(Z, X, Y, LayerId, new byte[0]);
            }
        }

        public bool PostTiles(MapTile[] tiles)
        {
            string sql = @"
                IF EXISTS 
                    (SELECT 1 FROM dbo.MapTiles WHERE @Z = Z AND @X = X AND @Y = Y AND @LayerId = LayerId)
                BEGIN
                    UPDATE dbo.MapTiles
                    SET TILE = @Tile
                    WHERE @Z = Z AND @X = X AND @Y = Y AND @LayerId = LayerId
                END
                ELSE
                BEGIN
                    INSERT INTO dbo.MapTiles VALUES (@Z, @Y, @X, @LayerId, @Tile)
                END";

            try
            {
                using (SqlConnection connection = new SqlConnection(Connection.GetConnectionString()))
                {
                    connection.Open();
                    connection.Execute(sql, tiles);
                    return true;
                }
            }
            catch (SqlException s)
            {
                Console.WriteLine(s);
                return false;
            }
        }
    }
}
