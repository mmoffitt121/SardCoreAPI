namespace SardCoreAPI.Attributes.Easy
{
    public class TableAttribute : Attribute
    {
        public string Name { get; set; }
        public TableAttribute(string name)
        {
            this.Name = name; 
        }
    }
}
