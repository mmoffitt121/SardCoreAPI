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
            string sql = @"SELECT Id, LocationName, AreaId, LocationTypeId, Longitude, Latitude FROM Locations 
                /**where**/
                /**orderby**/
            ";

            SqlBuilder builder = new SqlBuilder();
            var template = builder.AddTemplate(sql);

            if (!string.IsNullOrEmpty(criteria.Query)) { builder.Where("LocationName LIKE CONCAT('%', IFNULL(@Query, ''), '%')"); }
            if (criteria.Cities) { builder.OrWhere("LocationTypeId = 1"); }
            if (criteria.Towns) { builder.OrWhere("LocationTypeId = 2"); }
            if (criteria.Villages) { builder.OrWhere("LocationTypeId = 3"); }
            if (criteria.Hamlets) { builder.OrWhere("LocationTypeId = 4"); }
            if (criteria.Fortresses) { builder.OrWhere("LocationTypeId = 5"); }
            

            builder.OrderBy("CASE WHEN LocationName LIKE CONCAT(@Query, '%') THEN 0 ELSE 1 END, LocationName");

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

        public bool PostLocation(Location location)
        {
            string sql = @"INSERT INTO Locations (LocationName, AreaId, LocationTypeId, Longitude, Latitude) VALUES (@LocationName, @AreaId, @LocationTypeId, @Longitude, @Latitude)";
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
    }
}
