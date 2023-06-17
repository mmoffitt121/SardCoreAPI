using Microsoft.Data.SqlClient;
using Dapper;
using ImageMagick;
using SardCoreAPI.Models.Map.MapTile;
using MySqlConnector;
using SardCoreAPI.Utility.Files;
using System.Data;
using Microsoft.AspNetCore.Routing;

namespace SardCoreAPI.DataAccess.Map
{
    public class MapTileDataAccess : GenericDataAccess
    {
        public string ImagePath = "./storage/maptiles/";

        public async Task<MapTile> GetTile(int Z, int X, int Y, int LayerId)
        {
            string sql = @"SELECT Z, X, Y, LayerId FROM MapTiles 
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
                        MapTile tile = mapTiles[0];
                        try
                        {
                            tile.Tile = await new FileHandler().LoadImage(ImagePath + mapTiles[0].FileName);
                        }
                        catch
                        {
                            return new MapTile(Z, X, Y, LayerId, new byte[0]);
                        }
                        return tile;
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

        public async Task<MapTile[]> GetTiles(int RootZ, int RootX, int RootY, int MaxZ, int LayerId)
        {
            string sql = @"
            SELECT Z, X, Y, LayerId, POWER(2, Z - @RootZ) FROM MapTiles
                WHERE
                    Z >= @RootZ AND
                    Z <= @MaxZ AND
                    X >= @RootX * (POWER(2, Z - @RootZ)) AND
                    X < (@RootX + 1) * (POWER(2, Z - @RootZ)) AND
                    Y >= @RootY * (POWER(2, Z - @RootZ)) AND
                    Y < (@RootY + 1) * (POWER(2, Z - @RootZ)) AND
                    LayerId = @LayerId;
            ";

            return (await Query<MapTile>(sql, new { RootZ, RootX, RootY, MaxZ, LayerId })).ToArray();
        }

        public async Task<int> PostTiles(MapTile[] tiles)
        {
            string sql = @"
                INSERT INTO MapTiles
	                (Z, Y, X, LayerId)
                VALUES
	                (@Z, @Y, @X, @LayerId)
                ON DUPLICATE KEY UPDATE
	                Z = @Z";

            FileHandler fh = new FileHandler();
            List<Task> tasks = new List<Task>();

            foreach (MapTile tile in tiles)
            {
                tasks.Add(fh.SaveImage(ImagePath, tile.FileName, tile.Tile));
            }

            await Task.WhenAll(tasks);

            try
            {
                using (MySqlConnection connection = new MySqlConnection(Connection.GetConnectionString()))
                {
                    connection.Open();
                    return connection.Execute(sql, tiles);
                }
            }
            catch (MySqlException s)
            {
                Console.WriteLine(s);
                throw s;
            }
        }

        public async Task<int> DeleteTile(int Z, int X, int Y, int LayerId)
        {
            string sql = @"DELETE FROM MapTiles
                WHERE
                    Z = @Z AND
                    X = @X AND
                    Y = @Y AND
                    LayerId = @LayerId
            ";

            return await Execute(sql, new { Z, X, Y, LayerId });
        }
    }
}
