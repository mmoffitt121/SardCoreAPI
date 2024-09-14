using System.ComponentModel.DataAnnotations.Schema;

namespace SardCoreAPI.Models.DataPoints.DataPointParameters
{
    public class DataPointParameterString : DataPointParameter
    {
        [Column(TypeName = "varchar(1000)")]
        public string StringValue { get; set; }

        public override string GetStringValue()
        {
            return StringValue.ToString();
        }
    }
}
