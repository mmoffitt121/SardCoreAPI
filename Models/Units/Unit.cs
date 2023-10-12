namespace SardCoreAPI.Models.Units
{
    public class Unit
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string? Summary { get; set; }
        public int? ParentId { get; set; }
        public double? AmountPerParent { get; set; }
        public int MeasurableId { get; set; }
        public Measurable? Measurable { get; set; }
        public string Symbol { get; set; }
    }
}
