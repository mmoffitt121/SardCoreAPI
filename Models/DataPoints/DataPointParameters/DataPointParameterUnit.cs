using SardCoreAPI.Models.Units;
using System.Numerics;
using System.Text.Json.Serialization;

namespace SardCoreAPI.Models.DataPoints.DataPointParameters
{
    public class DataPointParameterUnit : DataPointParameter
    {
        public Unit? Unit { get; set; }
        public int? UnitID { get; set; }
        public double UnitValue { get; set; }

        public override string GetStringValue()
        {
            return UnitValue.ToString();
        }
    }
}
