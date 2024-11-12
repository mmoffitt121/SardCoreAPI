namespace SardCoreAPI.Models.Common
{
    public class IdNamePair
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public IdNamePair() { }

        public IdNamePair(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public IdNamePair(string id)
        {
            Id = id;
            Name = id;
        }
    }
}
