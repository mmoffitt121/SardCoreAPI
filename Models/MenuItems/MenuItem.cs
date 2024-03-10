namespace SardCoreAPI.Models.MenuItems
{
    public class MenuItem
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Route { get; set; }
        public string Expanded { get; set; }
        public MenuItem[] Children { get; set; }
    }
}
