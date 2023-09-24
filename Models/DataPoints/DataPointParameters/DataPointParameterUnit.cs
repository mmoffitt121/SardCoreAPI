using SardCoreAPI.Models.Units;

namespace SardCoreAPI.Models.DataPoints.DataPointParameters
{
    public class DataPointParameterUnit : DataPointParameter
    {
        public Unit? Unit { get; set; }
        public int UnitID { get; set; }
        public string UnitValue { get; set; }
    }
}
