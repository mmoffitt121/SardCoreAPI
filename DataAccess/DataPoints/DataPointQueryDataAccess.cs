using SardCoreAPI.Models.DataPoints;

namespace SardCoreAPI.DataAccess.DataPoints
{
    public interface IDataPointQueryDataAccess 
    {
        public List<DataPoint> Get();
    }

    public class DataPointQueryDataAccess : GenericDataAccess, IDataPointQueryDataAccess
    {
        public List<DataPoint> Get()
        {
            return null;
        }
    }
}
