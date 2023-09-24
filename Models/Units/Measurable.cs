namespace SardCoreAPI.Models.Units
{
    public class Measurable
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string? Summary { get; set; }
        public Base UnitType { get; set; }
        public enum Base
        {
            Generic = 0,
            Time = 1
        }
    }
}
