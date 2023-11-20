namespace SardCoreAPI.Models.Calendars.CalendarData
{
    public class CalendarTimeUnit
    {
        public int? Id { get; set; }
        public int? DerivedFromId { get; set; }
        public int AmountPerDerived { get; set; }
        public char Formatter { get; set; }
        public string Name { get; set; }
        public string? Summary { get; set; }
    }
}
