namespace SardCoreAPI.Models.Units
{
    public class UnitConversionRequest
    {
        public double Value { get; set; }
        public Unit UnitFrom { get; set; }
        public Unit UnitTo { get; set; }
    }
}
