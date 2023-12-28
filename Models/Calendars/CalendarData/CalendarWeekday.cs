using SardCoreAPI.Models.Common;

namespace SardCoreAPI.Models.Calendars.CalendarData
{
    public class CalendarWeekday : IValidatable
    {
        public int Sequence { get; set; }
        public string Name { get; set; }
        public string? Summary { get; set; }
        public char Formatter { get; set; }

        public List<string> Validate()
        {
            return new List<string> { };
        }
    }
}
