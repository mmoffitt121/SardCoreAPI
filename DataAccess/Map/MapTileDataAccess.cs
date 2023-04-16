using Microsoft.Data.SqlClient;
using SardCoreAPI.Models.Document.SearchResults;
using Dapper;
using ImageMagick;
using SardCoreAPI.Models.Map.MapTile;
using MySqlConnector;

namespace SardCoreAPI.DataAccess.Map
{
    public class MapTileDataAccess
    {
        public MapTile GetTile(int Z, int X, int Y, int LayerId)
        {
            string sql = @"SELECT Tile FROM MapTiles 
                WHERE
                    Z = @Z AND
                    X = @X AND
                    Y = @Y AND
                    LayerId = @LayerId
            ";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(Connection.GetConnectionString()))
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
            catch (MySqlException s)
            {
                Console.WriteLine(s);
                return new MapTile(Z, X, Y, LayerId, new byte[0]);
            }
        }

        public bool PostTiles(MapTile[] tiles)
        {
            string sql = @"
                INSERT INTO MapTiles
	                (Z, Y, X, LayerId, Tile)
                VALUES
	                (@Z, @Y, @X, @LayerId, @Tile)
                ON DUPLICATE KEY UPDATE
	                Tile = @Tile";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(Connection.GetConnectionString()))
                {
                    connection.Open();
                    connection.Execute(sql, tiles);
                    return true;
                }
            }
            catch (MySqlException s)
            {
                Console.WriteLine(s);
                return false;
            }
        }
    }
}
