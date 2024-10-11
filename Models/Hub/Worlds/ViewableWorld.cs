namespace SardCoreAPI.Models.Hub.Worlds
{
    public class ViewableWorld : World
    {
        public string OwnerName { get; set; }

        public ViewableWorld(World world, string ownerName)
        {
            this.Id = world.Id;
            this.OwnerId = world.OwnerId;
            this.Location = world.Location;
            this.Name = world.Name;
            this.Summary = world.Summary;
            this.CreatedDate = world.CreatedDate;
            this.IconId = world.IconId;
            this.OwnerName = ownerName;
        }
    }
}
