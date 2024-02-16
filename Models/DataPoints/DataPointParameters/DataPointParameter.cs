using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SardCoreAPI.Utility.Json;

namespace SardCoreAPI.Models.DataPoints.DataPointParameters
{
    [JsonConverter(typeof(ParameterJsonConverter))]
    public class DataPointParameter
    {
        public int? DataPointId { get; set; }
        public int DataPointTypeParameterId { get; set; }

        private class ParameterJsonConverter : DataPointJsonConverter<DataPointParameter>
        {
            protected override DataPointParameter Create(Type objectType, JObject jObject)
            {
                if (jObject.Value<string>("boolValue") != null)
                {
                    return new DataPointParameterBoolean();
                }
                if (jObject.Value<string>("dataPointValueId") != null)
                {
                    return new DataPointParameterDataPoint();
                }
                if (jObject.Value<string>("documentValue") != null)
                {
                    return new DataPointParameterDocument();
                }
                if (jObject.Value<string>("doubleValue") != null)
                {
                    return new DataPointParameterDouble();
                }
                if (jObject.Value<string>("intValueString") != null)
                {
                    return new DataPointParameterInt();
                }
                if (jObject.Value<string>("stringValue") != null)
                {
                    return new DataPointParameterString();
                }
                if (jObject.Value<string>("summaryValue") != null)
                {
                    return new DataPointParameterSummary();
                }
                if (jObject.Value<string>("unitValue") != null)
                {
                    return new DataPointParameterUnit();
                }
                if (jObject.Value<string>("timeValue") != null)
                {
                    return new DataPointParameterTime();
                }
                if (jObject.Value<string>("timeValueString") != null)
                {
                    return new DataPointParameterTime();
                }
                return new DataPointParameter();
            }
        }
    }
}
