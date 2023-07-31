namespace SardCoreAPI.Models.Hub.Worlds
{
    public class WorldInfo
    {
        public string WorldLocation { get; set; }

        public WorldInfo(string worldLocation)
        {
            WorldLocation = worldLocation;
        }   
    }
}
