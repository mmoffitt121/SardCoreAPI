using Microsoft.Data.SqlClient;
using Dapper;
using SardCoreAPI.Models.Map.MapLayer;
using SardCoreAPI.Models.Common;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using SardCoreAPI.Utility.Files;
using Microsoft.VisualBasic;
using SardCoreAPI.Models.Map;

namespace SardCoreAPI.DataAccess.Map
{
    public class MapLayerDataAccess : GenericDataAccess
    {
        public string ImagePath = "./storage/mapicons/";

        #region Map Layer
        public async Task<List<MapLayer>> GetMapLayers(MapLayerSearchCriteria criteria)
        {
            string pageSettings = "";
            if (criteria.PageNumber != null && criteria.PageSize != null)
            {
                pageSettings = $"LIMIT {criteria.PageSize} OFFSET {(criteria.PageNumber) * criteria.PageSize}";
            }

            string sql = $@"SELECT Id, Name, Summary, MapId, IsBaseLayer, IsIconLayer, IconURL
                    FROM MapLayers 
                    /**where**/ 
                    ORDER BY Name
                    {pageSettings}";

            SqlBuilder builder = new SqlBuilder();
            var template = builder.AddTemplate(sql);

            if (!string.IsNullOrEmpty(criteria.Query)) { builder.Where("Name LIKE CONCAT('%', IFNULL(@Query, ''), '%')"); }
            if (criteria.Id != null) { builder.Where("Id = @Id"); }
            if (criteria.MapId != null) { builder.Where("MapId = @MapId"); }
            if (criteria.IsBaseLayer != null) { builder.Where("IsBaseLayer = @IsBaseLayer"); }
            if (criteria.IsIconLayer != null) { builder.Where("IsIconLayer = @IsIconLayer"); }

            return await Query<MapLayer>(template.RawSql, criteria);
        }

        public async Task<int> PostMapLayer(MapLayer layer)
        {
            string sql = @"INSERT INTO MapLayers (Name, Summary, MapId, IsBaseLayer, IsIconLayer, IconURL) VALUES (@Name, @Summary, @MapId, @IsBaseLayer, @IsIconLayer, @IconURL);

                SELECT LAST_INSERT_ID();
            ";

            return await QueryFirst<int>(sql, layer);
        }

        public async Task<int> PutMapLayer(MapLayer layer)
        {
            string sql = @"";

            if (layer.IsBaseLayer ?? false)
            {
                sql += @"UPDATE MapLayers SET
                        IsBaseLayer = false
                    WHERE
                        @MapId = MapId AND @IsIconLayer = IsIconLayer;
                ";
            }

            sql += @"UPDATE MapLayers SET
                    Name = @Name,
                    Summary = @Summary,
                    MapId = @MapId,
                    IsBaseLayer = @IsBaseLayer,
                    IsIconLayer = @IsIconLayer,
                    IconURL = @IconURL
                WHERE
                    Id = @Id;
            ";


            return await Execute(sql, layer);
        }

        public async Task<int> DeleteMapLayer(int Id)
        {
            string sql = @"DELETE FROM MapLayers
                WHERE
                    Id = @Id;
            ";

            return await Execute(sql, new { Id });
        }

        public async Task<int> DeleteMapLayersOfMapId(int MapId)
        {
            string sql = @"DELETE FROM MapLayers
                WHERE
                    MapId = @MapId;
            ";

            return await Execute(sql, new { MapId });
        }
        #endregion

        #region Map Layer Icon
        public async Task PostMapLayerIcon(byte[] data, int id)
        {
            await new FileHandler().SaveImage(ImagePath, "MapLayer-" + id + ".png", data);
        }

        public async Task<byte[]> GetMapLayerIcon(int id)
        {
            return await new FileHandler().LoadImage(ImagePath + "MapLayer-" + id + ".png");
        }
        #endregion
    }
}
