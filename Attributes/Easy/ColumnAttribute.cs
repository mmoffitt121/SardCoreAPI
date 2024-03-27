namespace SardCoreAPI.Attributes.Easy
{
    public class ColumnAttribute : Attribute
    {
        public string? Name { get; set; }
        public bool OrderBy { get; set; }
        public bool PrimaryKey { get; set; }
        public ColumnAttribute(string? Name = null, bool OrderBy = false, bool PrimaryKey = false)
        {
            this.Name = Name;
            this.OrderBy = OrderBy;
            this.PrimaryKey = PrimaryKey;
        }
    }
}
