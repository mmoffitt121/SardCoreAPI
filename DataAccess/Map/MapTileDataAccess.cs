using Microsoft.Data.SqlClient;
using SardCoreAPI.Models.Document.SearchResults;
using Dapper;
using SardCoreAPI.Models.Map;

namespace SardCoreAPI.DataAccess.Map
{
    public class MapTileDataAccess
    {
        public static bool PostTiles(MapTile[] tiles)
        {
            string sql = @"INSERT INTO dbo.MAPS VALUES (@Z, @Y, @X, @Tile)";

            try
            {
                using (SqlConnection connection = new SqlConnection(Connection.GetConnectionString()))
                {
                    connection.Open();
                    connection.Query<MapTile>(sql, tiles);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
