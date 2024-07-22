namespace SardCoreAPI.Models.DataPoints.DataPointParameters
{
    public class DataPointParameterInt : DataPointParameter
    {
        public long? IntValue { get; set; }
        public string? IntValueString
        {
            get
            {
                return IntValue != null ? IntValue + "" : null;
            }
            set
            {
                long result;
                if (value?.Contains("null") ?? false)
                {
                    IntValue = null;
                }
                if (long.TryParse(value, out result))
                {
                    IntValue = result;
                }
                else
                {
                    return;
                }
            }
        }

        public override string GetStringValue()
        {
            return IntValue.ToString();
        }
    }
}
