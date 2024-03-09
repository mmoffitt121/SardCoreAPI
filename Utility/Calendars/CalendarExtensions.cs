using SardCoreAPI.Models.Calendars;
using SardCoreAPI.Models.Calendars.CalendarData;

namespace SardCoreAPI.Utility.Calendars
{
    public static class CalendarExtensions
    {
        public static List<Calendar> ToCalendarList(this List<CalendarDataAccessWrapper> wrappers)
        {
            List<Calendar> list = new List<Calendar>();
            wrappers.ForEach(wrapper =>
            {
                list.Add(wrapper.Calendar);
            });
            return list;
        }
    }
}
