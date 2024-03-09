using SardCoreAPI.Models.Calendars.CalendarData;
using System.Text.Json;

namespace SardCoreAPI.Models.Calendars
{
    /// <summary>
    /// Class that handles serialization and deserialization of calendars when loading and saving to the database.
    /// </summary>
    public class CalendarDataAccessWrapper
    {
        public int? Id { get; set; }
        private string _data { get; set; }
        public Calendar Calendar
        {
            get
            {
                var cal = JsonSerializer.Deserialize<Calendar>(_data);
                if (cal != null) cal.Id = Id;
                return cal;
            }
            set
            {
                _data = JsonSerializer.Serialize(value);
            }
        }
        public string Data
        {
            get { return _data; }
            set { _data = value; } 
        }

        public CalendarDataAccessWrapper() { }

        public CalendarDataAccessWrapper(string data)
        {
            _data = data;
        }

        public CalendarDataAccessWrapper(Calendar calendar)
        {
            Id = calendar.Id;
            Calendar = calendar;
        }
    }
}
