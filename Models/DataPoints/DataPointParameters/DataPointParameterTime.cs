using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace SardCoreAPI.Models.DataPoints.DataPointParameters
{
    public class DataPointParameterTime : DataPointParameter
    {
        public BigInteger? TimeValue { get; set; }
        [NotMapped]
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

        public override string GetStringValue()
        {
            return TimeValue.ToString();
        }
    }
}
