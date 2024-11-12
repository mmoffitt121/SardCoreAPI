using SardCoreAPI.Models.DataPoints.DataPointParameters;
using System.Collections.ObjectModel;
using System.Numerics;

namespace SardCoreAPI.Models.DataPoints.Queried
{
    public class QueriedDataPointParameter
    {
        public int TypeParameterId { get; set; }
        public string TypeParameterName { get; set; }
        public string? TypeParameterSummary { get; set; }
        public string TypeParameterTypeValue { get; set; }
        public string? TypeParameterSubType { get; set; }
        public int TypeParameterSequence { get; set; }
        public int? DataPointTypeReferenceId { get; set; }
        public string? TypeParameterSettings { get; set; }
        public string? Value { get; set; }
        public object? ValueData { get; set; }
        public List<string>? Values { get; set; }
        public List<object>? ValuesData { get; set; }
        public bool IsMultiple { get; set; }

        public DataPointParameter GetDataPointParameter(int dataPointId)
        {
            switch (TypeParameterTypeValue)
            {
                case "bit":
                    return new DataPointParameterBoolean()
                    {
                        DataPointId = dataPointId,
                        DataPointTypeParameterId = TypeParameterId,
                        BoolValue = bool.Parse(Value ?? "false"),
                    };
                case "dat":
                    return new DataPointParameterDataPoint()
                    {
                        DataPointId = dataPointId,
                        DataPointTypeParameterId = TypeParameterId,
                        DataPointValueId = int.Parse(Value ?? "-1"),
                    };
                case "doc":
                    return new DataPointParameterDocument()
                    {
                        DataPointId = dataPointId,
                        DataPointTypeParameterId = TypeParameterId,
                        DocumentValue = Value ?? "",
                    };
                case "dub":
                    return new DataPointParameterDouble()
                    {
                        DataPointId = dataPointId,
                        DataPointTypeParameterId = TypeParameterId,
                        DoubleValue = double.Parse(Value ?? "0"),
                    };
                case "int":
                    return new DataPointParameterInt()
                    {
                        DataPointId = dataPointId,
                        DataPointTypeParameterId = TypeParameterId,
                        IntValue = int.Parse(Value ?? "0"),
                    };
                case "str":
                    return new DataPointParameterString()
                    {
                        DataPointId = dataPointId,
                        DataPointTypeParameterId = TypeParameterId,
                        StringValue = Value ?? "",
                    };
                case "sum":
                    return new DataPointParameterSummary()
                    {
                        DataPointId = dataPointId,
                        DataPointTypeParameterId = TypeParameterId,
                        SummaryValue = Value ?? "",
                    };
                case "tim":
                    return new DataPointParameterTime()
                    {
                        DataPointId = dataPointId,
                        DataPointTypeParameterId = TypeParameterId,
                        TimeValue = BigInteger.Parse(Value ?? "0"),
                    };
                case "uni":
                    return new DataPointParameterUnit()
                    {
                        DataPointId = dataPointId,
                        DataPointTypeParameterId = TypeParameterId,
                        UnitValue = double.Parse(Value ?? "0"),
                        UnitID = DataPointTypeReferenceId == -1 ? null : DataPointTypeReferenceId,
                    };
                default:
                    throw new ArgumentException($"Invalid type value '{TypeParameterTypeValue}' specified for parameter {TypeParameterId} of data point {dataPointId}.");
            }
        }

        public List<DataPointParameter> GetDataPointParameters(int dataPointId)
        {
            List<DataPointParameter> parameters = new List<DataPointParameter>();
            if (Values != null)
            {
                int sequence = 0;
                foreach (string value in Values)
                {
                    parameters.Add(new QueriedDataPointParameter(this) 
                    { 
                        Value = value
                    }
                    .GetDataPointParameter(dataPointId).SetSequence(sequence));
                    sequence++;
                }
            }
            return parameters;
        }

        public QueriedDataPointParameter() { }

        public QueriedDataPointParameter(QueriedDataPointParameter value, bool includeValues = false)
        {
            TypeParameterId = value.TypeParameterId;
            TypeParameterName = value.TypeParameterName;
            TypeParameterSummary = value.TypeParameterSummary;
            TypeParameterTypeValue = value.TypeParameterTypeValue;
            TypeParameterSequence = value.TypeParameterSequence;
            DataPointTypeReferenceId = value.DataPointTypeReferenceId;
            TypeParameterSettings = value.TypeParameterSettings;
            IsMultiple = value.IsMultiple;

            if (includeValues)
            {
                Value = value.Value;
                ValueData = value.ValueData;
                Values = value.Values;
                ValuesData = value.ValuesData;

            }
        }
    }
}
