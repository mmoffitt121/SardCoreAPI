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

        public Location GetLocation(int? Id)
        {
            if (Id == null) return null;

            string sql = @"SELECT 
                l.Id, l.LocationName, l.LocationTypeId, l.Longitude, l.Latitude, l.LocationName, 
                    lt.Name as LocationType,
                    a.Id as AreaId, a.Name as AreaName, 
                    sr.Id as SubregionId, sr.Name as SubregionName,
                    r.Id as RegionId, r.Name as RegionName,
                    sc.Id as SubcontinentId, sc.Name as SubcontinentName,
                    c.Id as ContinentId, c.Name as ContinentName,
                    cb.Id as CelestialBodyId, cb.Name as CelestialBodyName,
                    cs.Id as CelestialSystemId, cs.Name as CelestialSystemName,
                    m.Id as ManifoldId, m.Name as ManifoldName
                FROM Locations l
                    LEFT JOIN LocationTypes lt on lt.Id = l.LocationTypeId
                    LEFT JOIN Areas a on a.Id = l.areaId
                    LEFT JOIN Subregions sr on sr.Id = a.SubregionId
                    LEFT JOIN Regions r on r.id = sr.RegionId
                    LEFT JOIN Subcontinents sc on sc.id = r.SubcontinentId
                    LEFT JOIN Continents c on c.id = sc.ContinentId
                    LEFT JOIN CelestialObjects cb on cb.id = c.CelestialObjectId
                    LEFT JOIN CelestialSystems cs on cs.id = cb.CelestialSystemId
                    LEFT JOIN Manifolds m on m.id = cs.ManifoldId
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
            string sql = @"INSERT INTO Locations (LocationName, AreaId, LocationTypeId, Longitude, Latitude) 
                VALUES (@LocationName, @AreaId, @LocationTypeId, @Longitude, @Latitude)";
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

        public bool DeleteLocation(int Id)
        {
            string sql = @"DELETE FROM Locations WHERE Id = @Id;";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Connection.GetConnectionString()))
                {
                    connection.Open();
                    if (connection.Execute(sql, new { Id }) > 0)
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
