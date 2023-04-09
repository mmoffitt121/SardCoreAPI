using Microsoft.Data.SqlClient;
using Dapper;
using SardCoreAPI.Models.Map.MapLayer;
using SardCoreAPI.Models.Common;
using Microsoft.AspNetCore.Mvc;

namespace SardCoreAPI.DataAccess.Map
{
    public class MapLayerDataAccess
    {
        public MapLayer? GetMapLayer(int Id)
        {
            string sql = @"SELECT TOP (1) * FROM dbo.MapLayers WHERE Id = @Id";

            try
            {
                using (SqlConnection connection = new SqlConnection(Connection.GetConnectionString()))
                {
                    connection.Open();
                    List<MapLayer> layers = connection.Query<MapLayer>(sql, new { Id = Id }).ToList();
                    if (layers.Count > 0)
                    {
                        return layers.First();
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

        public List<MapLayer>? GetMapLayers(DatedSearchCriteria criteria)
        {
            string sql = @"SELECT * FROM dbo.MapLayers /**where**/ /**orderby**/";

            SqlBuilder builder = new SqlBuilder();
            var template = builder.AddTemplate(sql);

            if (!string.IsNullOrEmpty(criteria.Query)) { builder.Where("Name LIKE CONCAT('%', @Query, '%')"); Console.WriteLine("thing"); }
            if (criteria.BeginDate != null) { builder.Where("LayerDate > @BeginDate"); Console.WriteLine("thing"); }
            if (criteria.EndDate != null) {builder.Where("LayerDate < @EndDate"); Console.WriteLine("thing"); }
            if (criteria.Era != null){ builder.Where("LayerEraId = @Era"); Console.WriteLine("thing"); }

            builder.OrderBy("CASE WHEN Name LIKE CONCAT(@Query, '%') THEN 0 ELSE 1 END, Name");

            try
            {
                using (SqlConnection connection = new SqlConnection(Connection.GetConnectionString()))
                {
                    connection.Open();
                    List<MapLayer> layers = connection.Query<MapLayer>(template.RawSql, criteria).ToList();
                    return layers;
                }
            }
            catch (SqlException s)
            {
                Console.WriteLine(s);
                return null;
            }
        }

        public bool PostMapLayer(MapLayer layer)
        {
            string sql = @"INSERT INTO dbo.MapLayers (Name, LayerDate, LayerEraId) VALUES (@Name, @LayerDate, @LayerEraId)";
            try
            {
                using (SqlConnection connection = new SqlConnection(Connection.GetConnectionString()))
                {
                    connection.Open();
                    if (connection.Execute(sql, layer) > 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (SqlException s)
            {
                Console.WriteLine(s);
                return false;
            }
        }
    }
}
