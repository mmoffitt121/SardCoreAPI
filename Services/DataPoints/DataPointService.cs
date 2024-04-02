using SardCoreAPI.DataAccess.DataPoints;
using SardCoreAPI.Models.DataPoints;

namespace SardCoreAPI.Services.DataPoints
{
    public interface IDataPointService
    {
        public Task<List<DataPoint>> GetList(DataPointSearchCriteria criteria);
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

        public async Task<List<DataPoint>> GetList(DataPointSearchCriteria criteria)
        {
            return null;
        }
    }
}
