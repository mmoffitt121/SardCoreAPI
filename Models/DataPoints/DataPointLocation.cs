using Microsoft.EntityFrameworkCore;
using SardCoreAPI.Models.Map.Location;

namespace SardCoreAPI.Models.DataPoints
{
    [PrimaryKey("DataPointId", "LocationId")]
    public class DataPointLocation
    {
        public int DataPointId { get; set; }
        public int LocationId { get; set; }
        public bool IsPrimary { get; set; }

        public DataPoint? DataPoint { get; set; }
        public Location? Location { get; set; }
    }
}
