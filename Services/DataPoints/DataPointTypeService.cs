using Microsoft.EntityFrameworkCore;
using SardCoreAPI.Models.DataPoints;
using SardCoreAPI.Models.Hub.Worlds;
using SardCoreAPI.Services.Context;
using SardCoreAPI.Utility.DataAccess;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SardCoreAPI.Services.DataPoints
{
    public interface IDataPointTypeService
    {
        public Task<List<DataPointType>> GetDataPointTypes(DataPointTypeSearchCriteria criteria);
    }

    public class DataPointTypeService : IDataPointTypeService
    {
        private readonly IDataService data;

        public DataPointTypeService(IDataService data) 
        {
            this.data = data;
        }

        public async Task<List<DataPointType>> GetDataPointTypes(DataPointTypeSearchCriteria criteria)
        {
            return await data.Context.DataPointType
                .Where(criteria.GetQuery())
                .Sort(criteria)
                .Paginate(criteria)
                .ToListAsync();
        }
    }
}
