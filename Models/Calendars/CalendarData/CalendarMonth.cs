using SardCoreAPI.Models.Common;

namespace SardCoreAPI.Models.Calendars.CalendarData
{
    public class CalendarMonth : IValidatable
    {
        public string Name { get; set; }
        public string? Summary { get; set; }
        public int Days { get; set; }
        public int Sequence { get; set; }

        public List<string> Validate()
        {
            return new List<string> { };
        }
    }
}
