using Dapper;
using Microsoft.Data.SqlClient;
using MySqlConnector;
using SardCoreAPI.Models.Map.LocationType;
using SardCoreAPI.Models.Map.MapTile;

namespace SardCoreAPI.DataAccess.Map
{
    public class LocationTypeDataAccess
    {
        public LocationType? GetLocationType(int id)
        {
            string sql = @"SELECT * FROM LocationTypes 
                WHERE
                    Id = @Id
                LIMIT 1;
            ";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(Connection.GetConnectionString()))
                {
                    connection.Open();
                    LocationType? locationType = connection.Query<LocationType>(sql, new { Id = id }).FirstOrDefault();
                    if (locationType != null)
                    {
                        return locationType;
                    }
                    else
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

        public List<LocationType> GetLocationTypes(string? query)
        {
            string sql = @"SELECT * FROM LocationTypes 
                WHERE
                    Name LIKE CONCAT('%', IFNULL(@Name, ''), '%')
                ORDER BY
                    CASE WHEN Name LIKE CONCAT(@Name, '%') THEN 0 ELSE 1 END,
                    Name
            ";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(Connection.GetConnectionString()))
                {
                    connection.Open();
                    List<LocationType> locationTypes = connection.Query<LocationType>(sql, new { Name = query }).ToList();
                    return locationTypes;
                }
            }
            catch (MySqlException s)
            {
                Console.WriteLine(s);
                return null;
            }
        }
    }
}
