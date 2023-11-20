namespace SardCoreAPI.Models.Calendars.CalendarData
{
    public class Calendar
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string? Summary { get; set; }
        public int UnitTimePerDay { get; set; }
        public List<CalendarMonth> Months { get; set; }
        public List<CalendarTimeUnit> TimeUnits { get; set; }
        public List<CalendarTimeUnit> Eras { get; set; }
        public List<CalendarTimeZone> TimeZones { get; set; }
        public List<CalendarFormatter> Formatters { get; set; }
        public List<CalendarWeekday> Weekdays { get; set; }
    }
}
