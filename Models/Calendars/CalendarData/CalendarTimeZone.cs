namespace SardCoreAPI.Models.Calendars.CalendarData
{
    public class CalendarTimeZone
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string? Summary { get; set; }
        public int Offset { get; set; }
        public int DerivedTimeUnitId { get; set; }
    }
}
