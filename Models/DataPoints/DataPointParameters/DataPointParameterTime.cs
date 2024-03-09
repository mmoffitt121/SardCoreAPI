using System.Numerics;

namespace SardCoreAPI.Models.DataPoints.DataPointParameters
{
    public class DataPointParameterTime : DataPointParameter
    {
        public BigInteger? TimeValue { get; set; }
        public string? TimeValueString {
            get
            {
                return TimeValue?.ToString();
            }
            set
            {
                TimeValue = BigInteger.Parse(value ?? "");
            }
        }
    }
}
