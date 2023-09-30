using Dapper;
using SardCoreAPI.Models.Common;
using SardCoreAPI.Models.Hub.Worlds;
using SardCoreAPI.Models.Units;

namespace SardCoreAPI.DataAccess.Units
{
    public class UnitDataAccess : GenericDataAccess
    {
        public async Task<List<UnitTable>> GetUnitTables(PagedSearchCriteria criteria, WorldInfo info)
        {
            string pageSettings = "";
            if (criteria.PageNumber != null && criteria.PageSize != null)
            {
                pageSettings = $"LIMIT {criteria.PageSize} OFFSET {(criteria.PageNumber) * criteria.PageSize}";
            }

            string sql = $@"
                SELECT u.Id, u.Name, u.Summary, u.ParentId, u.AmountPerParent, u.MeasurableId
                FROM Units u
                    LEFT JOIN Measurables m ON u.MeasurableId = m.Id
                /**where**/
                /**orderby**/
                {pageSettings}
            ";

            SqlBuilder builder = new SqlBuilder();
            var template = builder.AddTemplate(sql);

            if (!string.IsNullOrEmpty(criteria.Query)) { builder.Where("Name LIKE CONCAT('%', IFNULL(@Query, ''), '%')"); }
            if (criteria.Id != null && criteria.Id > 0) { builder.Where("Id = @Id"); }

            builder.OrderBy("CASE WHEN u.Name LIKE CONCAT(@Query, '%') THEN 0 ELSE 1 END, u.Name");

            List<Unit> units = await Query<Unit>(template.RawSql, criteria, info);

            Dictionary<int, List<Unit>> tablesDict = new Dictionary<int, List<Unit>>();
            foreach (Unit u in units)
            {
                if (tablesDict.TryGetValue(u.MeasurableId, out List<Unit> value))
                {
                    value.Add(u);
                }
                else
                {
                    tablesDict.Add(u.MeasurableId, new List<Unit>());
                    if (tablesDict.TryGetValue(u.MeasurableId, out List<Unit> val))
                    {
                        val.Add(u);
                    }
                }
            }

            List<Measurable> measurables = await new MeasurableDataAccess().GetMeasurables(new PagedSearchCriteria(), info);

            List<UnitTable> tables = new List<UnitTable>();
            tablesDict.ToList().ForEach(t =>
            {
                UnitTable unitTable = new UnitTable();
                unitTable.Units = t.Value.ToArray();
                unitTable.Measurable = measurables.Find(m => m.Id == t.Key);
            });

            return tables;
        }

        public async Task<List<Unit>> GetUnits(PagedSearchCriteria criteria, WorldInfo info)
        {
            string pageSettings = "";
            if (criteria.PageNumber != null && criteria.PageSize != null)
            {
                pageSettings = $"LIMIT {criteria.PageSize} OFFSET {(criteria.PageNumber) * criteria.PageSize}";
            }

            string sql = $@"
                SELECT Id, Name, Summary, ParentId, AmountPerParent, MeasurableId
                FROM Units
                /**where**/
                /**orderby**/
                {pageSettings}
            ";

            SqlBuilder builder = new SqlBuilder();
            var template = builder.AddTemplate(sql);

            if (!string.IsNullOrEmpty(criteria.Query)) { builder.Where("Name LIKE CONCAT('%', IFNULL(@Query, ''), '%')"); }
            if (criteria.Id != null && criteria.Id > 0) { builder.Where("Id = @Id"); }

            builder.OrderBy("CASE WHEN Name LIKE CONCAT(@Query, '%') THEN 0 ELSE 1 END, Name");

            return await Query<Unit>(template.RawSql, criteria, info);
        }

        public async Task<bool> PostUnit(Unit data, WorldInfo info)
        {
            string sql = @"INSERT INTO Units (Name, Summary, ParentId, AmountPerParent, MeasurableId) 
                VALUES (@Name, @Summary, @ParentId, @AmountPerParent, @MeasurableId)
            ";

            return await Execute(sql, data, info) > 0;
        }

        public async Task<int> PutUnit(Unit data, WorldInfo info)
        {
            string sql = @"UPDATE Units SET 
	                Name = @Name,
                    Summary = @Summary,
                    ParentId = @ParentId,
                    AmountPerParent = @AmountPerParent,
                    MeasurableId = @MeasurableId
                WHERE Id = @Id";

            return await Execute(sql, data, info);
        }

        public async Task<int> DeleteUnit (int Id, WorldInfo info)
        {
            string sql = @"DELETE FROM Units WHERE Id = @Id;";
            return await Execute(sql, new { Id }, info);
        }
    }
}
