using SardCoreAPI.Models.Common;

namespace SardCoreAPI.Models.Calendars.CalendarData
{
    public class CalendarEra : CalendarTimeUnit, IValidatable
    {
        public List<CalendarEraDefinition>? EraDefinitions { get; set; }
        public bool Defined { get; set; }
        public char NameFormatter { get; set; }
    }
}
