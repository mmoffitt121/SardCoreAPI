namespace SardCoreAPI.Models.Map.Location
{
    public class Location
    {
        public int Id { get; set; }
        public string LocationName { get; set; }
        public int? AreaId { get; set; }
        public int LocationTypeId { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
