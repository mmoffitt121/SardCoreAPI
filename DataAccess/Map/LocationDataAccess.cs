using MySqlConnector;
using Dapper;
using SardCoreAPI.Models.Map.Location;
using SardCoreAPI.Models.Map.LocationType;

namespace SardCoreAPI.DataAccess.Map
{
    public class LocationDataAccess
    { 
        public List<Location> GetLocations(LocationSearchCriteria criteria)
        {
            string sql = @"
                SELECT 
                    l.Id, l.Name, LocationTypeId, LayerId, Longitude, Latitude, ParentId, 
                    IFNULL(l.ZoomProminenceMin, lt.ZoomProminenceMin) AS ZoomProminenceMin,
                    IFNULL(l.ZoomProminenceMax, lt.ZoomProminenceMax) AS ZoomProminenceMax
                FROM Locations l
                    LEFT JOIN LocationTypes lt on lt.Id = l.LocationTypeId
                /**where**/
                /**orderby**/
            ";

            SqlBuilder builder = new SqlBuilder();
            var template = builder.AddTemplate(sql);

            if (!string.IsNullOrEmpty(criteria.Query)) { builder.Where("Name LIKE CONCAT('%', IFNULL(@Query, ''), '%')"); }
            if (criteria.MapLayerIds != null) { builder.Where("LayerId in @MapLayerIds"); }
            if (criteria.LocationTypes != null && criteria.LocationTypes.Count() > 0) { builder.Where("LocationTypeId in @LocationTypes"); }
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

        public Location GetLocation(int? Id)
        {
            if (Id == null) return null;

            string sql = @"SELECT Id, Name, LocationTypeId, LayerId, Longitude, Latitude, ParentId, ZoomProminenceMin, ZoomProminenceMax
                FROM Locations l
                /**where**/";

            SqlBuilder builder = new SqlBuilder();
            var template = builder.AddTemplate(sql);

            builder.Where("l.Id = @Id");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(Connection.GetConnectionString()))
                {
                    connection.Open();
                    try
                    {
                        Location location = connection.QueryFirst<Location>(template.RawSql, new { Id });
                        return location;
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            catch (MySqlException s)
            {
                Console.WriteLine(s);
                return null;
            }
        }

        public bool PostLocation(Location location)
        {
            string sql = @"INSERT INTO Locations (Name, LocationTypeId, LayerId, Longitude, Latitude, ParentId, ZoomProminenceMin, ZoomProminenceMax) 
                VALUES (@Name, @LocationTypeId, @LayerId, @Longitude, @Latitude, @ParentId, @ZoomProminenceMin, @ZoomProminenceMax)";

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

        public async Task<int> PutLocation(Location location)
        {
            string sql = @"UPDATE Locations SET 
	                Name = @Name,
                    LocationTypeId = @LocationTypeId,
                    LayerId = @LayerId,
                    Longitude = @Longitude,
                    Latitude = @Latitude,
                    ParentId = @ParentId,
                    ZoomProminenceMin = @ZoomProminenceMin,
                    ZoomProminenceMax = @ZoomProminenceMax
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

        public async Task<int> DeleteLocation(int Id)
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
