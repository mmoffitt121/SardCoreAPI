using SardCoreAPI.Models.Common;

namespace SardCoreAPI.Models.Calendars.CalendarData
{
    public class CalendarTimeUnit : IValidatable
    {
        public int? Id { get; set; }
        public int? AmountPerDerived { get; set; }
        public char Formatter { get; set; }
        public string Name { get; set; }
        public string? Summary { get; set; }

        public List<string> Validate()
        {
            return new List<string> { };
        }
    }
}
