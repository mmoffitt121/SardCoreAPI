using Dapper;
using MySqlConnector;
using SardCoreAPI.Models.Common;
using SardCoreAPI.Models.DataPoints;
using SardCoreAPI.Models.DataPoints.DataPointParameters;
using SardCoreAPI.Models.Map.Location;
using System.Collections.Generic;

namespace SardCoreAPI.DataAccess.DataPoints
{
    public class DataPointTypeDataAccess
    {
        public async Task<List<DataPointType>> GetDataPointTypes(PagedSearchCriteria criteria)
        {
            string pageSettings = "";
            if (criteria.PageNumber != null && criteria.PageSize != null)
            {
                pageSettings = $"LIMIT {criteria.PageSize} OFFSET { (criteria.PageNumber - 1) * criteria.PageSize }";
            }

            string sql = $@"SELECT Id, Name, Summary
                FROM DataPointTypes
                /**where**/
                ORDER BY Name
                { pageSettings }
            ";

            SqlBuilder builder = new SqlBuilder();
            var template = builder.AddTemplate(sql);

            if (!string.IsNullOrEmpty(criteria.Query)) { builder.Where("Name LIKE CONCAT('%', IFNULL(@Query, ''), '%')"); }
            if (criteria.Id != null) { builder.Where("Id = @Id"); }

            builder.OrderBy("Name");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(Connection.GetConnectionString()))
                {
                    connection.Open();
                    List<DataPointType> dataPointTypes = (await connection.QueryAsync<DataPointType>(template.RawSql, criteria)).ToList();
                    return dataPointTypes;
                }
            }
            catch (MySqlException s)
            {
                Console.WriteLine(s);
                return null;
            }
        }

        
        public bool PostDataPointType(DataPointType data)
        {
            string sql = @"INSERT INTO DataPointTypes (Name, Summary) 
                VALUES (@Name, @Summary)";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(Connection.GetConnectionString()))
                {
                    connection.Open();
                    if (connection.Execute(sql, data) > 0)
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
        /*
        public async Task<int> PutLocation(Location location)
        {
            string sql = @"UPDATE Locations SET 
	                LocationName = @LocationName,
                    AreaId = @AreaId,
                    LocationTypeId = @LocationTypeId,
                    Longitude = @Longitude,
                    Latitude = @Latitude
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
        }*/
    }
}
