using SardCoreAPI.Models.Common;

namespace SardCoreAPI.Models.Calendars.CalendarData
{
    public class CalendarFormatter : IValidatable
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string? Summary { get; set; }
        public string Formatter { get; set; }

        public List<string> Validate()
        {
            return new List<string> { };
        }
    }
}
