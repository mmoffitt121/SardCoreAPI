namespace SardCoreAPI.Models.Administration.Database
{
    public class DatabaseInfo
    {
        public string Name { get; set; }
        public double Size { get; set; }

        public DatabaseInfo(string name, double size)
        {
            Name = name;
            Size = size;
        }
    }
}
