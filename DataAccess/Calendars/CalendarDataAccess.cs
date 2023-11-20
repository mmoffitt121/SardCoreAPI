using Dapper;
using SardCoreAPI.Models.DataPoints.DataPointParameters;
using SardCoreAPI.Models.DataPoints;
using SardCoreAPI.Models.Hub.Worlds;
using SardCoreAPI.Models.Calendars.CalendarData;
using SardCoreAPI.Models.Calendars;
using SardCoreAPI.Utility.Calendars;
using SardCoreAPI.Models.Common;

namespace SardCoreAPI.DataAccess.Calendars
{
    public class CalendarDataAccess : GenericDataAccess
    {
        public async Task<List<Calendar>> Get(PagedSearchCriteria? criteria, WorldInfo worldInfo)
        {
            var sql = "SELECT Id, CalendarObject AS Data FROM Calendars /**where**/";
            SqlBuilder builder = new SqlBuilder();
            var template = builder.AddTemplate(sql);

            if (criteria?.Id != null) { builder.Where("Id == @Id"); }

            return (await Query<CalendarDataAccessWrapper>(template.RawSql, criteria, worldInfo)).ToCalendarList();
        }

        public async Task Put(Calendar calendar, WorldInfo info)
        {
            var wrapper = new CalendarDataAccessWrapper(calendar);
            string sql = @"INSERT INTO Calendars (Id, CalendarObject) VALUES (@Id, @Data) 
                    ON DUPLICATE KEY UPDATE
                        CalendarObject = @Data";

            await Execute(sql, wrapper, info);
        }

        public async Task Delete(int id, WorldInfo worldInfo)
        {
            string sql = "DELETE FROM Calendars WHERE Id = @Id";
            await Execute(sql, new { id }, worldInfo);
        }
    }
}
