using SardCoreAPI.Models.Units;
using System.Numerics;
using System.Text.Json.Serialization;

namespace SardCoreAPI.Models.DataPoints.DataPointParameters
{
    public class DataPointParameterUnit : DataPointParameter
    {
        public Unit? Unit { get; set; }
        public int UnitID { get; set; }
        public string UnitValue { get; set; }
        [JsonIgnore]
        public BigInteger NumericalValue { get; set; }
        [JsonIgnore]
        public int NumericalValuePower { get; set; }
    }
}
