using Microsoft.Data.SqlClient;
using SardCoreAPI.Models.Document.SearchResults;
using Dapper;
using SardCoreAPI.Models.Map;

namespace SardCoreAPI.DataAccess.Map
{
    public class MapTileDataAccess
    {
        public static bool PostTile(MapTile map)
        {
            string sql = @"INSERT INTO dbo.MAPS values (@MapName, @MapDate, @MapAuthorCode, @MapPublisherCode, @MapLink, @MapThumbnailLink, @MapDescription)";

            try
            {
                using (SqlConnection connection = new SqlConnection(Connection.GetConnectionString()))
                {
                    connection.Open();
                    connection.Query<MapSearchResult>(sql, map);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
