namespace SardCoreAPI.Attributes.Easy
{
    public class ColumnAttribute : Attribute
    {
        public string? Name { get; set; }
        public bool OrderBy { get; set; }
        public ColumnAttribute(string? Name = null, bool OrderBy = false)
        {
            this.Name = Name;
            this.OrderBy = OrderBy;
        }
    }
}
