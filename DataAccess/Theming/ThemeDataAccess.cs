using Dapper;
using SardCoreAPI.Models.Common;
using SardCoreAPI.Models.Hub.Worlds;
using SardCoreAPI.Models.Theming;

namespace SardCoreAPI.DataAccess.Theming
{
    public class ThemeDataAccess : GenericDataAccess
    {
        public async Task<List<Theme>> GetThemes(DefaultablePagedSearchCriteria criteria, WorldInfo info)
        {
            string pageSettings = "";
            if (criteria.PageNumber != null && criteria.PageSize != null)
            {
                pageSettings = $"LIMIT {criteria.PageSize} OFFSET {(criteria.PageNumber) * criteria.PageSize}";
            }

            string sql = $@"SELECT * FROM Themes
                /**where**/
                ORDER BY Name
                {pageSettings};
            ";

            SqlBuilder builder = new SqlBuilder();
            var template = builder.AddTemplate(sql);

            if (!string.IsNullOrEmpty(criteria.Query)) { builder.Where("Name LIKE CONCAT('%', IFNULL(@Query, ''), '%')"); }
            if (criteria.Id != null) { builder.Where("Id = @Id"); }
            if (criteria.IsDefault != null) { builder.Where("IsDefault = @IsDefault"); }

            return await Query<Theme>(template.RawSql, criteria, info);
        }

        public async Task<int> PostTheme(Theme data, WorldInfo info)
        {
            string updateDefaults = "";

            if (data.IsDefault ?? false)
            {
                updateDefaults = "UPDATE Themes SET IsDefault = false;";
            }

            string sql = $@"
                {updateDefaults}
                INSERT INTO Themes (
                    Id,
	                Name,
	                PrimaryColor,
	                PrimaryColorSelected,
	                InvertedTextColor,
	                InvertedTextColorDisabled,
	                TextColor,
	                TextColorDisabled,
	                SecondaryTextColor,
	                TertiaryTextColor,
	                PrimaryAccentColor,
	                PrimaryAccentColorDisabled,
	                BackgroundColor,
	                SecondaryBackgroundColor,
	                FieldOverlayColor,
	                FieldOverlayColorDark,
	                DestructiveActionColor,
	                PrimaryFont,
	                FontWeightbold,
	                DataPointValueFontSize
                ) 
                VALUES (
                    @Id,
	                @Name,
	                @PrimaryColor,
	                @PrimaryColorSelected,
	                @InvertedTextColor,
	                @InvertedTextColorDisabled,
	                @TextColor,
	                @TextColorDisabled,
	                @SecondaryTextColor,
	                @TertiaryTextColor,
	                @PrimaryAccentColor,
	                @PrimaryAccentColorDisabled,
	                @BackgroundColor,
	                @SecondaryBackgroundColor,
	                @FieldOverlayColor,
	                @FieldOverlayColorDark,
	                @DestructiveActionColor,
	                @PrimaryFont,
	                @FontWeightbold,
	                @DataPointValueFontSize
                );
            
                SELECT LAST_INSERT_ID();";

            return (await Query<int>(sql, data, info)).FirstOrDefault();
        }

        public async Task<int> PutTheme(Theme data, WorldInfo info)
        {
            string updateDefaults = "";

            if (data.IsDefault ?? false)
            {
                updateDefaults = "UPDATE Themes SET IsDefault = false;";
            }
            string sql = $@"
                {updateDefaults}
                UPDATE Themes SET 
                    Id = @Id,
	                Name = @Name,
	                PrimaryColor = @PrimaryColor,
	                PrimaryColorSelected = @PrimaryColorSelected,
	                InvertedTextColor = @InvertedTextColor,
	                InvertedTextColorDisabled = @InvertedTextColorDisabled,
	                TextColor = @TextColor,
	                TextColorDisabled = @TextColorDisabled,
	                SecondaryTextColor = @SecondaryTextColor,
	                TertiaryTextColor = @TertiaryTextColor,
	                PrimaryAccentColor = @PrimaryAccentColor,
	                PrimaryAccentColorDisabled = @PrimaryAccentColorDisabled,
	                BackgroundColor = @BackgroundColor,
	                SecondaryBackgroundColor = @SecondaryBackgroundColor,
	                FieldOverlayColor = @FieldOverlayColor,
	                FieldOverlayColorDark = @FieldOverlayColorDark,
	                DestructiveActionColor = @DestructiveActionColor,
	                PrimaryFont = @PrimaryFont,
	                FontWeightbold = @FontWeightbold,
	                DataPointValueFontSize = @DataPointValueFontSize
                WHERE Id = @Id";

            return await Execute(sql, data, info);
        }

        public async Task<int> DeleteTheme(int Id, WorldInfo info)
        {
            string sql = @"DELETE FROM Themes WHERE Id = @Id;";

            return await Execute(sql, new { Id }, info);
        }
    }
}
