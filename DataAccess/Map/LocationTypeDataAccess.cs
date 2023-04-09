using Dapper;
using Microsoft.Data.SqlClient;
using SardCoreAPI.Models.Map.LocationType;
using SardCoreAPI.Models.Map.MapTile;

namespace SardCoreAPI.DataAccess.Map
{
    public class LocationTypeDataAccess
    {
        public LocationType? GetLocationType(int id)
        {
            string sql = @"SELECT TOP (1) * FROM dbo.LocationTypes 
                WHERE
                    Id = @Id
            ";

            try
            {
                using (SqlConnection connection = new SqlConnection(Connection.GetConnectionString()))
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
            catch (SqlException s)
            {
                Console.WriteLine(s);
                return null;
            }
        }

        public List<LocationType> GetLocationTypes(string? query)
        {
            string sql = @"SELECT * FROM dbo.LocationTypes 
                WHERE
                    Name LIKE CONCAT('%', @Name, '%')
                ORDER BY
                    CASE WHEN Name LIKE CONCAT(@Name, '%') THEN 0 ELSE 1 END,
                    Name
            ";

            try
            {
                using (SqlConnection connection = new SqlConnection(Connection.GetConnectionString()))
                {
                    connection.Open();
                    List<LocationType> locationTypes = connection.Query<LocationType>(sql, new { Name = query }).ToList();
                    if (locationTypes != null && locationTypes.Count > 0)
                    {
                        return locationTypes;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (SqlException s)
            {
                Console.WriteLine(s);
                return null;
            }
        }
    }
}
