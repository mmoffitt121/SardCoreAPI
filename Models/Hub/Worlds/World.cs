namespace SardCoreAPI.Models.Hub.Worlds
{
    public class World
    {
        public int Id { get; set; }
        public string OwnerId { get; set; }
        public string Location { get; set; }
        public string Name { get; set; }
        public string? Summary { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
