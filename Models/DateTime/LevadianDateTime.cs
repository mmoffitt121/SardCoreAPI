namespace SardCoreAPI.Models.DateTime
{
    public class LevadianDateTime
    {
        private static readonly int SECONDS_PER_MINUTE = 64;
        private static readonly int MINUTES_PER_HOUR = 64;
        private static readonly int HOURS_PER_DAY = 32;
        private static readonly int DAYS_PER_MONTH = 32;
        private static readonly int MONTHS_PER_YEAR = 16;

        private long _timeinseconds;
        private int _eraId;
        private int _era;
        private int _age;
        private int _epoch;
        private int _eon;
        private int _stage;

        public string DateTime { get { return Angorainne + "-" + Moyorainne + "-" + Limitainne; } }
        public string Angorainne { get { return $"|{Stage}|{Eon}|{Epoch}|{Age}|{Era}|"; } }
        public string Moyorainne { get { return $"|{Year}|{Month}|{Day}|"; } }
        public string Limitainne { get { return $"|{Hour}|{Minute}|{Second}|"; } }

        public int Second
        {
            get { return (int)(_timeinseconds % SECONDS_PER_MINUTE); }
            set { _timeinseconds = _timeinseconds - Second + value; }
        }
        public int Minute
        {
            get { return (int)(_timeinseconds / SECONDS_PER_MINUTE % MINUTES_PER_HOUR); }
            set { _timeinseconds = _timeinseconds - Minute + value * SECONDS_PER_MINUTE; }
        }
        public int Hour
        {
            get { return (int)(_timeinseconds / (SECONDS_PER_MINUTE * MINUTES_PER_HOUR) % HOURS_PER_DAY); }
            set { _timeinseconds = _timeinseconds - Hour + value * SECONDS_PER_MINUTE * MINUTES_PER_HOUR; }
        }
        public int Day
        {
            get { return (int)(_timeinseconds / (SECONDS_PER_MINUTE * MINUTES_PER_HOUR * HOURS_PER_DAY) % DAYS_PER_MONTH); }
            set { _timeinseconds = _timeinseconds - Day + value * SECONDS_PER_MINUTE * MINUTES_PER_HOUR * HOURS_PER_DAY; }
        }
        public int Month
        {
            get { return (int)(_timeinseconds / (SECONDS_PER_MINUTE * MINUTES_PER_HOUR * HOURS_PER_DAY * DAYS_PER_MONTH) % MONTHS_PER_YEAR); }
            set { _timeinseconds = _timeinseconds - Month + value * SECONDS_PER_MINUTE * MINUTES_PER_HOUR * HOURS_PER_DAY * DAYS_PER_MONTH; }
        }
        public int Year
        {
            get { return (int)(_timeinseconds / (SECONDS_PER_MINUTE * MINUTES_PER_HOUR * HOURS_PER_DAY * DAYS_PER_MONTH * MONTHS_PER_YEAR)); }
            set { _timeinseconds = _timeinseconds - Year + value * SECONDS_PER_MINUTE * MINUTES_PER_HOUR * HOURS_PER_DAY * DAYS_PER_MONTH * MONTHS_PER_YEAR; }
        }
        public int Era
        {
            get { return _era; }
            set { _era = value; }
        }
        public int Age
        {
            get { return _age; }
            set { _age = value; }
        }
        public int Epoch
        {
            get { return _epoch; }
            set { _epoch = value; }
        }
        public int Eon
        {
            get { return _eon; }
            set { _eon = value; }
        }
        public int Stage
        {
            get { return _stage; }
            set { _stage = value; }
        }

        public enum MonthName
        {
            Leviadias,
            Hivermas,
            Astermas,
            Vedemas,
            Valida,
            Nida,
            Redarmas,
            Horcomas,
            Brulanne,
            Realianne,
            Serdalle,
            Cordomas,
            Joliste,
            Assesste,
            Geleste,
            Repomas
        }
        public enum DayName
        {
            Rofessida,
            Calida,
            Zuda,
            Fairleda,
            Presida,
            Argenda,
            Erasseda,
            Repoda
        }

        public string CurrentMonth
        {
            get { return ((MonthName)Month).ToString(); }
        }
        public string CurrentDay
        {
            get { return ((DayName)Day).ToString(); }
        }

        public string GetDBOForm()
        {
            return $"{Stage}-{Eon}-{Epoch}-{Age}-{Era}|{Year}-{Month}-{Day}-{Hour}-{Minute}-{Second}";
        }

        public void SetFromDBOForm(long dateTime, int eraId, int era, int age, int epoch, int eon, int stage)
        {
            _timeinseconds = dateTime;
            _eraId = eraId;
            _era = era;
            _age = age;
            _epoch = epoch;
            _eon = eon;
            _stage = stage;
        }

        #region Constructors

        public LevadianDateTime(long dateTime, int eraId, int era, int age, int epoch, int eon, int stage)
        {
            SetFromDBOForm(dateTime, eraId, era, age, epoch, eon, stage);
        }

        public LevadianDateTime()
        {

        }

        #endregion
    }
}
