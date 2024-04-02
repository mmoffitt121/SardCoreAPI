using SardCoreAPI.DataAccess.DataPoints;
using SardCoreAPI.Models.DataPoints;
using SardCoreAPI.Models.DataPoints.Queried;

namespace SardCoreAPI.Services.DataPoints
{
    public interface IDataPointService
    {
        public Task<List<DataPoint>> GetList(DataPointQuery criteria);
    }

    public class DataPointService : IDataPointService
    {
        private readonly IDataPointQueryService _queryService;
        private readonly IDataPointQueryDataAccess _dataAccess;

        public DataPointService(IDataPointQueryService queryService, IDataPointQueryDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
            _queryService = queryService;
        }

        public async Task<List<DataPoint>> GetList(DataPointQuery criteria)
        {
            _queryService.BuildGet(criteria);
            return null;
        }
    }
}
