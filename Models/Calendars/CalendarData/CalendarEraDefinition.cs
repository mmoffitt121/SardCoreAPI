using System.Numerics;

namespace SardCoreAPI.Models.Calendars.CalendarData
{
    public class CalendarEraDefinition
    {
        public string Name { get; set; }
        public string Summary { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public bool Backwards { get; set; }
    }
}
