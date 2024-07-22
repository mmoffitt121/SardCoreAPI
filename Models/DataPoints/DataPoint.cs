using Microsoft.IdentityModel.Tokens;
using SardCoreAPI.Models.DataPoints.DataPointParameters;
using SardCoreAPI.Models.DataPoints.Queried;
using System.Text.Json.Serialization;

namespace SardCoreAPI.Models.DataPoints
{
    public class DataPoint
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public int TypeId { get; set; }
        public DataPointType? Type { get; set; }
        public List<DataPointParameter>? Parameters { get; set; }

        public DataPoint() { }

        public DataPoint(QueriedDataPoint qdp)
        {
            Id = qdp.Id;
            Name = qdp.Name;
            TypeId = qdp.TypeId;
            Parameters = new List<DataPointParameter>();
            qdp.Parameters = qdp.Parameters?.Where(p => !p.Value.IsNullOrEmpty()).ToList();
            qdp.Parameters?.ForEach(p =>
            {
                Parameters.Add(p.GetDataPointParameter(Id ?? -1));
            });
        }
    }
}
