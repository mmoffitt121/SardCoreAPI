﻿using Dapper;
using Microsoft.Data.SqlClient;
using MySqlConnector;
using SardCoreAPI.Models.Common;
using SardCoreAPI.Models.Map.LocationType;
using SardCoreAPI.Models.Map.MapTile;

namespace SardCoreAPI.DataAccess.Map
{
    public class LocationTypeDataAccess : GenericDataAccess
    {
        public async Task<List<LocationType>> GetLocationTypes(PagedSearchCriteria criteria)
        {
            string pageSettings = "";
            if (criteria.PageNumber != null && criteria.PageSize != null)
            {
                pageSettings = $"LIMIT {criteria.PageSize} OFFSET {criteria.PageNumber * criteria.PageSize}";
            }

            string sql = $@"SELECT Id, Name, Summary, ParentTypeId, AnyTypeParent, IconPath, ZoomProminenceMin, ZoomProminenceMax, 
                    UsesIcon, UsesLabel, IconURL FROM LocationTypes 
                WHERE
                    Name LIKE CONCAT('%', IFNULL(@Query, ''), '%')
                ORDER BY
                    CASE WHEN Name LIKE CONCAT(@Query, '%') THEN 0 ELSE 1 END,
                    Name
                {pageSettings}
            ";

            SqlBuilder builder = new SqlBuilder();
            var template = builder.AddTemplate(sql);

            if (!string.IsNullOrEmpty(criteria.Query)) { builder.Where("Name LIKE CONCAT('%', IFNULL(@Query, ''), '%')"); }
            if (criteria.Id != null) { builder.Where("Id = @Id"); }

            return await Query<LocationType>(template.RawSql, criteria);
        }

        public async Task<int> PostLocationType(LocationType data)
        {
            string sql = $@"
                INSERT INTO LocationTypes (
                    Name,
                    Summary,
                    ParentTypeId,
                    AnyTypeParent,
                    ZoomProminenceMin,
                    ZoomProminenceMax,
                    UsesIcon,
                    UsesLabel,
                    IconURL
                ) 
                VALUES (
                    @Name,
                    @Summary,
                    @ParentTypeId,
                    @AnyTypeParent,
                    @ZoomProminenceMin,
                    @ZoomProminenceMax,
                    @UsesIcon,
                    @UsesLabel,
                    @IconURL
                );
            
                SELECT LAST_INSERT_ID();";

            return (await Query<int>(sql, data)).FirstOrDefault();
        }

        public async Task<int> PutLocationType(LocationType data)
        {
            string sql = @"
                UPDATE LocationTypes
                SET
                    Name = @Name,
                    Summary = @Summary,
                    ParentTypeId = @ParentTypeId,
                    AnyTypeParent = @AnyTypeParent,
                    ZoomProminenceMin = @ZoomProminenceMin,
                    ZoomProminenceMax = @ZoomProminenceMax,
                    UsesIcon = @UsesIcon,
                    UsesLabel = @UsesLabel,
                    IconURL = IFNULL(@IconURL, IconURL)
                WHERE Id = @Id";

            return await Execute(sql, data);
        }

        public async Task<int> DeleteLocationType(int Id)
        {
            string sql = @"DELETE FROM LocationTypes WHERE Id = @Id";

            return await Execute(sql, new { Id });
        }
    }
}
