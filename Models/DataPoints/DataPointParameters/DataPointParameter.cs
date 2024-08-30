using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SardCoreAPI.Models.DataPoints.Queried;
using SardCoreAPI.Utility.Json;

namespace SardCoreAPI.Models.DataPoints.DataPointParameters
{
    [JsonConverter(typeof(ParameterJsonConverter))]
    [PrimaryKey("DataPointId", "DataPointTypeParameterId", "Sequence")]
    public class DataPointParameter
    {
        public int? DataPointId { get; set; }
        public int DataPointTypeParameterId { get; set; }
        public DataPointTypeParameter? DataPointTypeParameter { get; set; }
        public int Sequence { get; set; }

        public virtual string GetStringValue()
        {
            return string.Empty;
        }

        public DataPointParameter SetSequence(int sequence)
        {
            Sequence = sequence;
            return this;
        }

        private class ParameterJsonConverter : DataPointJsonConverter<DataPointParameter>
        {
            protected override DataPointParameter Create(Type objectType, JObject jObject)
            {
                if (jObject.Value<string>("boolValue") != null || jObject.Value<string>("BoolValue") != null)
                {
                    return new DataPointParameterBoolean();
                }
                if (jObject.Value<string>("dataPointValueId") != null || jObject.Value<string>("DataPointValueId") != null)
                {
                    return new DataPointParameterDataPoint();
                }
                if (jObject.Value<string>("documentValue") != null || jObject.Value<string>("DocumentValue") != null)
                {
                    return new DataPointParameterDocument();
                }
                if (jObject.Value<string>("doubleValue") != null || jObject.Value<string>("DoubleValue") != null)
                {
                    return new DataPointParameterDouble();
                }
                if (jObject.Value<string>("intValueString") != null || jObject.Value<string>("IntValueString") != null)
                {
                    return new DataPointParameterInt();
                }
                if (jObject.Value<string>("stringValue") != null || jObject.Value<string>("StringValue") != null)
                {
                    return new DataPointParameterString();
                }
                if (jObject.Value<string>("summaryValue") != null || jObject.Value<string>("SummaryValue") != null)
                {
                    return new DataPointParameterSummary();
                }
                if (jObject.Value<string>("unitValue") != null || jObject.Value<string>("UnitValue") != null)
                {
                    return new DataPointParameterUnit();
                }
                if (jObject.Value<string>("timeValue") != null || jObject.Value<string>("TimeValue") != null)
                {
                    return new DataPointParameterTime();
                }
                if (jObject.Value<string>("timeValueString") != null || jObject.Value<string>("TimeValueString") != null)
                {
                    return new DataPointParameterTime();
                }
                return new DataPointParameter();
            }
        }
    }
}
