namespace SardCoreAPI.Util.DateTime
{
    public class LevadianDateTime
    {
        private long _timeinseconds;
        private int _era;
        private int _age;
        private int _epoch;
        private int _eon;
        private int _stage;

        private readonly int _secondsperminute = 64;
        private readonly int _minutesperhour = 64;
        private readonly int _hoursperday = 32;
        private readonly int _dayspermonth = 32;
        private readonly int _monthsperyear = 16;


        public string DateTime { get { return $"|{Stage}|{Eon}|{Epoch}|{Age}|{Era}|-|{Year}|{Month}|{Day}|-|{Hour}|{Minute}|{Second}|"; } }
        public string Angorainne { get { return $"|{Stage}|{Eon}|{Epoch}|{Age}|{Era}|"; } }
        public string Moyorainne { get { return $"|{Year}|{Month}|{Day}|"; } }
        public string Limitainne { get { return $"|{Hour}|{Minute}|{Second}|"; } }

        public int Second 
        {
            get { return (int)(_timeinseconds % _secondsperminute); }
            set { _timeinseconds = _timeinseconds - Second + value; }
        }
        public int Minute
        {
            get { return (int)((_timeinseconds / _secondsperminute) % _minutesperhour); }
            set { _timeinseconds = _timeinseconds - Minute + value; }
        }
        public int Hour
        {
            get { return (int)((_timeinseconds / (_secondsperminute * _minutesperhour)) % _hoursperday); }
            set { _timeinseconds = _timeinseconds - Hour + value; }
        }
        public int Day
        {
            get { return (int)((_timeinseconds / (_secondsperminute * _minutesperhour * _hoursperday)) % _dayspermonth); }
            set { _timeinseconds = _timeinseconds - Day + value; }
        }
        public int Month
        {
            get { return (int)((_timeinseconds / (_secondsperminute * _minutesperhour * _hoursperday * _dayspermonth)) % _monthsperyear); }
            set { _timeinseconds = _timeinseconds - Month + value; }
        }
        public int Year
        {
            get { return (int)(_timeinseconds / (_secondsperminute * _minutesperhour * _hoursperday * _dayspermonth * _monthsperyear)); }
            set { _timeinseconds = _timeinseconds - Year + value; }
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

        // stage-eon-epoch-age-era|year-month-day-hour-minute-second
        public void SetFromDBOForm(string dboform)
        {
            try
            {
                string[] splitform = dboform.Split('|');
                string[] angorainne = splitform[0].Split('-');
                string[] moyorainnelimitainne = splitform[1].Split('-');

                switch (moyorainnelimitainne.Count())
                {
                    case 6:
                        Second = Int32.Parse(moyorainnelimitainne[5]);
                        goto case 5;
                    case 5:
                        Minute = Int32.Parse(moyorainnelimitainne[4]);
                        goto case 4;
                    case 4:
                        Hour = Int32.Parse(moyorainnelimitainne[3]);
                        goto case 3;
                    case 3:
                        Day = Int32.Parse(moyorainnelimitainne[2]);
                        goto case 2;
                    case 2:
                        Month = Int32.Parse(moyorainnelimitainne[1]);
                        goto case 1;
                    case 1:
                        Year = Int32.Parse(moyorainnelimitainne[0]);
                        goto default;
                    default:
                        break;
                }
                

                switch (angorainne.Count())
                {
                    case 5:
                        Era = Int32.Parse(angorainne[4]);
                        goto case 4;
                    case 4:
                        Age = Int32.Parse(angorainne[3]);
                        goto case 3;
                    case 3:
                        Epoch = Int32.Parse(angorainne[2]);
                        goto case 2;
                    case 2:
                        Eon = Int32.Parse(angorainne[1]);
                        goto case 1;
                    case 1:
                        Stage = Int32.Parse(angorainne[0]);
                        goto default;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }

        public string GetDBOForm()
        {
            return $"{Stage}-{Eon}-{Epoch}-{Age}-{Era}|{Year}-{Month}-{Day}-{Hour}-{Minute}-{Second}";
        }
    }
}
