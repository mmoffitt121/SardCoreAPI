using System.ComponentModel.DataAnnotations.Schema;

namespace SardCoreAPI.Models.DataPoints.DataPointParameters
{
    public class DataPointParameterSummary : DataPointParameter
    {
        [Column(TypeName = "varchar(5000)")]
        public string SummaryValue { get; set; }

        public override string GetStringValue()
        {
            return SummaryValue.ToString();
        }
    }
}
