using SardCoreAPI.Models.Common;
using SardCoreAPI.Utility.Validation;

namespace SardCoreAPI.Models.Calendars.CalendarData
{
    public class Calendar : IValidatable
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

        public List<string> Validate()
        {
            var result = new List<string>();

            if ("".Equals(Name)) result.Add("Calendar Name is required");
            if (UnitTimePerDay < 1) result.Add("Unit Time Per Day must be 1 or greater.");
            if (Formatters.Count() == 0) result.Add("At least one formatter is required.");
            
            Months.ForEach(item => result = result.Concat(item.Validate()).ToList());
            TimeUnits.ForEach(item => result = result.Concat(item.Validate()).ToList());
            Eras.ForEach(item => result = result.Concat(item.Validate()).ToList());
            TimeZones.ForEach(item => result = result.Concat(item.Validate()).ToList());
            Formatters.ForEach(item => result = result.Concat(item.Validate()).ToList());
            Weekdays.ForEach(item => result = result.Concat(item.Validate()).ToList());

            return result;
        }
    }
}
