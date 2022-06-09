using SardCoreAPI.Models;
using SardCoreAPI.Models.SearchResults;
using SardCoreAPI;
using Microsoft.Data.SqlClient;
using Dapper;
using SardCoreAPI.Models.SearchCriteria;

namespace SardCoreAPI.CodeStore
{
    public class MapCode
    {
        public static IEnumerable<Map> GetMaps(MapSearchCriteria criteria)
        {
            string sql = @"
                SELECT
                    MapId,
                    MapName, 
                    MapDate, 
                    MapAuthorCode,
					AuthorFirstName,
					AuthorMiddleName,
					AuthorLastName, 
                    MapPublisherCode, 
					PublisherName,
					PublisherLocationCode,
					LocationName,
					LocationJurisdictionID,
					j.JurisdictionName,
					j.ParentJurisdictionID,
					j2.JurisdictionName as ParentJurisdictionName,
                    MapLink, 
                    MapThumbnailLink, 
                    MapDescription
                FROM 
                    dbo.MAPS AS m
                    LEFT JOIN dbo.AUTHOR AS a ON m.MapAuthorCode = a.AuthorID
					LEFT JOIN dbo.PUBLISHER AS p ON m.MapPublisherCode = p.PublisherID
					LEFT JOIN dbo.LOCATIONS as l on p.PublisherLocationCode = l.LocationID
					LEFT JOIN dbo.JURISDICTIONS as j on l.LocationJurisdictionID = j.JurisdictionID
					LEFT JOIN dbo.JURISDICTIONS as j2 on j.ParentJurisdictionID = j2.JurisdictionID
                /**where**/";

            SqlBuilder builder = new SqlBuilder();

            if (criteria.MapName != null)
            {
                builder.Where("ISNULL(MapName,'') LIKE @MapNameSearch");
            }

            var template = builder.AddTemplate(sql);

            using (SqlConnection connection = new SqlConnection(Connection.GetConnectionString()))
            {
                connection.Open();
                return connection.Query<Map>(template.RawSql, criteria);
            }
        }

        public static Map? GetMap(int mapid)
        {
            string sql = @"
                SELECT
                    MapId,
                    MapName, 
                    MapDate, 
                    MapAuthorCode, 
                    MapPublisherCode, 
                    MapLink, 
                    MapThumbnailLink, 
                    MapDescription
                FROM 
                    dbo.MAPS
                WHERE
                    MapID = @mapid";

            try
            {
                using (SqlConnection connection = new SqlConnection(Connection.GetConnectionString()))
                {
                    connection.Open();
                    return connection.Query<Map>(sql, new Map { MapId = mapid }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static bool PostMap(Map map)
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

        public static bool PutMap(Map map)
        {
            string sql = @"
                UPDATE dbo.MAPS SET 
                    MapName = @MapName,
                    MapDate = @MapDate,
                    MapAuthorCode = @AuthorCode,
                    MapPublisherCode = @MapPublisherCode,
                    MapLink = @MapLink,
                    MapThumbnailLink = @MapThumbnailLink,
                    MapDescription = @MapDescription
                WHERE
                    MapID = @MapID";

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

        public static bool DeleteMap(int mapid)
        {
            string sql = @"DELETE FROM dbo.MAPS WHERE MapID = @mapid";

            try
            {
                using (SqlConnection connection = new SqlConnection(Connection.GetConnectionString()))
                {
                    connection.Open();
                    connection.Query<Map>(sql, new Map { MapId = mapid }).FirstOrDefault();
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
