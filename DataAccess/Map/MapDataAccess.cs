using Dapper;
using SardCoreAPI.Models.Common;
using SardCoreAPI.Models.DataPoints;
using SardCoreAPI.Models.Hub.Worlds;
using SardCoreAPI.Models.Map;
using SardCoreAPI.Models.Map.MapLayer;
using SardCoreAPI.Utility.Files;
using mp = SardCoreAPI.Models.Map;

namespace SardCoreAPI.DataAccess.Map
{
    public class MapDataAccess : GenericDataAccess
    {
        public string ImagePath(WorldInfo info)
        {
            return "./storage/" + info.WorldLocation + "/mapicons/";
        }

        #region Map
        public async Task<List<mp.Map>> GetMaps(MapSearchCriteria criteria, WorldInfo info)
        {
            string pageSettings = "";
            if (criteria.PageNumber != null && criteria.PageSize != null)
            {
                pageSettings = $"LIMIT {criteria.PageSize} OFFSET {(criteria.PageNumber) * criteria.PageSize}";
            }

            string sql = $@"SELECT 
                    Id, 
                    Name,
                    Summary,
                    Loops,
                    DefaultZ,
                    DefaultX,
                    DefaultY,
                    MinZoom,
                    MaxZoom,
                    IsDefault
                FROM Map
                /**where**/
                ORDER BY Name
                {pageSettings};
            ";

            SqlBuilder builder = new SqlBuilder();
            var template = builder.AddTemplate(sql);

            if (!string.IsNullOrEmpty(criteria.Query)) { builder.Where("Name LIKE CONCAT('%', IFNULL(@Query, ''), '%')"); }
            if (criteria.Id != null) { builder.Where("Id = @Id"); }
            if (criteria.IsDefault != null) { builder.Where("IsDefault = @IsDefault"); }

            return await Query<mp.Map>(template.RawSql, criteria, info);
        }

        public async Task<int> PostMap(mp.Map data, WorldInfo info)
        {
            string updateDefaults = "";

            if (data.IsDefault ?? false)
            {
                updateDefaults = "UPDATE Map SET IsDefault = false;";
            }

            string sql = $@"
                {updateDefaults}
                INSERT INTO Map (
                    Name,
                    Summary,
                    Loops,
                    DefaultZ,
                    DefaultX,
                    DefaultY,
                    MinZoom,
                    MaxZoom,
                    IsDefault
                ) 
                VALUES (
                    @Name, 
                    @Summary,
                    @Loops,
                    @DefaultZ,
                    @DefaultX,
                    @DefaultY,
                    @MinZoom,
                    @MaxZoom,
                    @IsDefault
                );
            
                SELECT LAST_INSERT_ID();";

            return (await Query<int>(sql, data, info)).FirstOrDefault();
        }

        public async Task<int> PutMap(mp.Map data, WorldInfo info)
        {
            string updateDefaults = "";

            if (data.IsDefault ?? false)
            {
                updateDefaults = "UPDATE Map SET IsDefault = false;";
            }
            string sql = $@"
                {updateDefaults}
                UPDATE Map SET 
                    Name = @Name, 
                    Summary = @Summary,
                    Loops = @Loops,
                    DefaultZ = @DefaultZ,
                    DefaultX = @DefaultX,
                    DefaultY = @DefaultY,
                    MinZoom = @MinZoom,
                    MaxZoom = @MaxZoom,
                    IsDefault = @IsDefault
                WHERE Id = @Id";

            return await Execute(sql, data, info);
        }

        public async Task<int> DeleteMap(int Id, WorldInfo info)
        {
            string sql = @"DELETE FROM Map WHERE Id = @Id;";

            return await Execute(sql, new { Id }, info);
        }
        #endregion

        #region Map Icon
        public async Task UploadMapIcon(byte[] data, int id, WorldInfo info)
        {
            await new FileHandler().SaveImage(ImagePath(info), "Map-" + id + ".png", data);
        }

        public async Task<byte[]> GetMapIcon(int id, WorldInfo info)
        {
            return await new FileHandler().LoadImage(ImagePath(info) + "Map-" + id + ".png");
        }
        #endregion
    }
}
