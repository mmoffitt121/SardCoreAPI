using MySqlConnector;
using Dapper;
using SardCoreAPI.Models.Map.Location;
using SardCoreAPI.Models.Map.LocationType;
using SardCoreAPI.Models.Hub.Worlds;

namespace SardCoreAPI.DataAccess.Map
{
    public class LocationDataAccess : GenericDataAccess
    { 
        public List<Location> GetLocations(LocationSearchCriteria criteria, WorldInfo info)
        {
            string sql = @"
                SELECT 
                    l.Id, l.Name, LocationTypeId, LayerId, Longitude, Latitude, ParentId, 
                    IFNULL(l.ZoomProminenceMin, lt.ZoomProminenceMin) AS ZoomProminenceMin,
                    IFNULL(l.ZoomProminenceMax, lt.ZoomProminenceMax) AS ZoomProminenceMax,
                    IFNULL(l.IconURL, lt.IconURL) as IconURL, 
                    lt.UsesIcon, lt.UsesLabel,
                    IFNULL(l.LabelFontSize, lt.LabelFontSize) as LabelFontSize,
                    IFNULL(l.LabelFontColor, lt.LabelFontColor) as LabelFontColor
                FROM Locations l
                    LEFT JOIN LocationTypes lt on lt.Id = l.LocationTypeId
                /**where**/
                /**orderby**/
            ";

            SqlBuilder builder = new SqlBuilder();
            var template = builder.AddTemplate(sql);

            if (!string.IsNullOrEmpty(criteria.Query)) { builder.Where("Name LIKE CONCAT('%', IFNULL(@Query, ''), '%')"); }
            if (criteria.Id != null && criteria.Id > 0) { builder.Where("l.Id = @Id"); }
            if (criteria.MapLayerIds != null) { builder.Where("LayerId in @MapLayerIds"); }
            if (criteria.LocationTypeIds != null && criteria.LocationTypeIds.Count() > 0) { builder.Where("LocationTypeId in @LocationTypeIds"); }
            if (criteria.MinLatitude != null) { builder.Where("Latitude >= @MinLatitude"); }
            if (criteria.MaxLatitude != null) { builder.Where("Latitude <= @MaxLatitude"); }
            if (criteria.MinLongitude != null) { builder.Where("Longitude >= @MinLongitude"); }
            if (criteria.MaxLongitude != null) { builder.Where("Longitude <= @MaxLongitude"); }
            if (criteria.MinZoom != null) { builder.Where("@MinZoom >= IFNULL(l.ZoomProminenceMin, lt.ZoomProminenceMin)"); }
            if (criteria.MaxZoom != null) { builder.Where("@MaxZoom <= IFNULL(l.ZoomProminenceMax, lt.ZoomProminenceMax)"); }


            builder.OrderBy("CASE WHEN l.Name LIKE CONCAT(@Query, '%') THEN 0 ELSE 1 END, l.Name");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(Connection.GetConnectionString()))
                {
                    connection.Open();
                    List<Location> locationTypes = connection.Query<Location>(template.RawSql, criteria).ToList();
                    return locationTypes;
                }
            }
            catch (MySqlException s)
            {
                Console.WriteLine(s);
                return null;
            }
            
        }

        public async Task<Location> GetLocation(int? Id, WorldInfo info)
        {
            if (Id == null) return null;

            string sql = @"SELECT l.Id, l.Name, l.LocationTypeId, l.LayerId, l.Longitude, l.Latitude, l.ParentId, l.ZoomProminenceMin, l.ZoomProminenceMax, l.IconURL, l.LabelFontSize, l.LabelFontColor, lt.Name as LocationTypeName
                FROM Locations l
                    LEFT JOIN LocationTypes lt on l.LocationTypeId = lt.Id
                /**where**/";

            SqlBuilder builder = new SqlBuilder();
            var template = builder.AddTemplate(sql);

            builder.Where("l.Id = @Id");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(Connection.GetConnectionString()))
                {
                    connection.Open();
                    Location location = connection.QueryFirst<Location>(template.RawSql, new { Id });
                    return location;
                }
            }
            catch (MySqlException s)
            {
                Console.WriteLine(s);
                return null;
            }
        }

        public bool PostLocation(Location location, WorldInfo info)
        {
            string sql = @"INSERT INTO Locations (Name, LocationTypeId, LayerId, Longitude, Latitude, ParentId, ZoomProminenceMin, ZoomProminenceMax, IconURL, LabelFontSize, LabelFontColor) 
                VALUES (@Name, @LocationTypeId, @LayerId, @Longitude, @Latitude, @ParentId, @ZoomProminenceMin, @ZoomProminenceMax, @IconURL, @LabelFontSize, @LabelFontColor)";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(Connection.GetConnectionString()))
                {
                    connection.Open();
                    if (connection.Execute(sql, location) > 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (MySqlException s)
            {
                Console.WriteLine(s);
                return false;
            }
        }

        public async Task<int> PutLocation(Location location, WorldInfo info)
        {
            string sql = @"UPDATE Locations SET 
	                Name = @Name,
                    LocationTypeId = @LocationTypeId,
                    LayerId = @LayerId,
                    Longitude = @Longitude,
                    Latitude = @Latitude,
                    ParentId = @ParentId,
                    ZoomProminenceMin = @ZoomProminenceMin,
                    ZoomProminenceMax = @ZoomProminenceMax,
                    IconURL = IFNULL(@IconURL, IconURL),
                    LabelFontSize = @LabelFontSize,
                    LabelFontColor = @LabelFontColor
                WHERE Id = @Id";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(Connection.GetConnectionString()))
                {
                    connection.Open();
                    return await connection.ExecuteAsync(sql, location);
                }
            }
            catch (MySqlException s)
            {
                Console.WriteLine(s);
                return -1;
            }
        }

        public async Task<int> DeleteLocation(int Id, WorldInfo info)
        {
            string sql = @"DELETE FROM Locations WHERE Id = @Id;";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Connection.GetConnectionString()))
                {
                    connection.Open();
                    return await connection.ExecuteAsync(sql, new { Id });
                }
            }
            catch (MySqlException s)
            {
                Console.WriteLine(s);
                return -1;
            }
        }
    }
}
