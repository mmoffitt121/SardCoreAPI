using Dapper;
using SardCoreAPI.Models.Common;
using SardCoreAPI.Models.DataPoints;
using SardCoreAPI.Models.Map;
using SardCoreAPI.Utility.Files;
using mp = SardCoreAPI.Models.Map;

namespace SardCoreAPI.DataAccess.Map
{
    public class MapDataAccess : GenericDataAccess
    {
        public string ImagePath = "./storage/mapicons/";

        #region Map
        public async Task<List<mp.Map>> GetMaps(MapSearchCriteria criteria)
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
                    AreaZoomProminence,
                    SubregionZoomProminence,
                    RegionZoomProminence,
                    SubcontinentZoomProminence,
                    ContinentZoomProminence,
                    DefaultZ,
                    DefaultX,
                    DefaultY,
                    MinZoom,
                    MaxZoom,
                    IsDefault
                FROM Map
                /**where**/
                ORDER BY Name
                {pageSettings}
            ";

            SqlBuilder builder = new SqlBuilder();
            var template = builder.AddTemplate(sql);

            if (!string.IsNullOrEmpty(criteria.Query)) { builder.Where("Name LIKE CONCAT('%', IFNULL(@Query, ''), '%')"); }
            if (criteria.Id != null) { builder.Where("Id = @Id"); }
            if (criteria.IsDefault != null) { builder.Where("IsDefault = @IsDefault"); }

            return await Query<mp.Map>(template.RawSql, criteria);
        }

        public async Task<int> PostMap(mp.Map data)
        {
            string sql = @"INSERT INTO Map (
                    Name,
                    Summary,
                    Loops,
                    AreaZoomProminence,
                    SubregionZoomProminence,
                    RegionZoomProminence,
                    SubcontinentZoomProminence,
                    ContinentZoomProminence,
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
                    @AreaZoomProminence,
                    @SubregionZoomProminence,
                    @RegionZoomProminence,
                    @SubcontinentZoomProminence,
                    @ContinentZoomProminence,
                    @DefaultZ,
                    @DefaultX,
                    @DefaultY,
                    @MinZoom,
                    @MaxZoom,
                    @IsDefault
                );
            
                SELECT LAST_INSERT_ID();";

            return (await Query<int>(sql, data)).FirstOrDefault();
        }

        public async Task<int> PutMap(mp.Map data)
        {
            string sql = @"UPDATE Map SET 
                    Name = @Name, 
                    Summary = @Summary,
                    Loops = @Loops,
                    AreaZoomProminence = @AreaZoomProminence,
                    SubregionZoomProminence = @SubregionZoomProminence,
                    RegionZoomProminence = @RegionZoomProminence,
                    SubcontinentZoomProminence = @SubcontinentZoomProminence,
                    ContinentZoomProminence = @ContinentZoomProminence,
                    DefaultZ = @DefaultZ,
                    DefaultX = @DefaultX,
                    DefaultY = @DefaultY,
                    MinZoom = @MinZoom,
                    MaxZoom = @MaxZoom,
                    IsDefault = @IsDefault
                WHERE Id = @Id";

            return await Execute(sql, data);
        }

        public async Task<int> DeleteMap(int Id)
        {
            string sql = @"DELETE FROM Map WHERE Id = @Id;";

            return await Execute(sql, new { Id });
        }
        #endregion

        #region Map Icon
        public async Task UploadMapIcon(byte[] data, int id)
        {
            await new FileHandler().SaveImage(ImagePath, "Map-" + id + ".png", data);
        }

        public async Task<byte[]> GetMapIcon(int id)
        {
            return await new FileHandler().LoadImage(ImagePath + "Map-" + id + ".png");
        }
        #endregion
    }
}
