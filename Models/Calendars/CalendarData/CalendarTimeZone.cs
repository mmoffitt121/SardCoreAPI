using SardCoreAPI.Models.Common;

namespace SardCoreAPI.Models.Calendars.CalendarData
{
    public class CalendarTimeZone : IValidatable
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string? Summary { get; set; }
        public int Offset { get; set; }
        public int DerivedTimeUnitId { get; set; }

        public List<string> Validate()
        {
            return new List<string> { };
        }
    }
}
