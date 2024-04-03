using SardCoreAPI.DataAccess;
using SardCoreAPI.DataAccess.DataPoints;
using SardCoreAPI.Models.DataPoints;
using SardCoreAPI.Models.DataPoints.Queried;
using SardCoreAPI.Models.Hub.Worlds;
using SardCoreAPI.Services.WorldContext;
using System.Dynamic;

namespace SardCoreAPI.Services.DataPoints
{
    public interface IDataPointService
    {
        public Task<DataPointQueryResult> GetList(DataPointSearchCriteria criteria);
    }

    public class DataPointService : IDataPointService
    {
        private readonly IDataPointQueryService _queryService;
        private IGenericDataAccess _genericDataAccess;
        private IWorldInfoService _worldInfoService;

        public DataPointService(IDataPointQueryService queryService, IGenericDataAccess genericDataAccess, IWorldInfoService worldInfoService)
        {
            _genericDataAccess = genericDataAccess;
            _queryService = queryService;
            _worldInfoService = worldInfoService;
        }

        public async Task<DataPointQueryResult> GetList(DataPointSearchCriteria criteria)
        {
            (ExpandoObject, string) idQueryInfo = _queryService.BuildIdQuery(criteria);
            (ExpandoObject, string) countQueryInfo = _queryService.BuildCountQuery(criteria);
            WorldInfo info = _worldInfoService.GetWorldInfo();

            int count = await _genericDataAccess.QueryFirst<int>(countQueryInfo.Item2, countQueryInfo.Item1, info);

            if (count == 0)
            {
                return new DataPointQueryResult() { Count = 0, Results = new List<QueriedDataPoint>() };
            }

            List<int> ids = await _genericDataAccess.Query<int>(idQueryInfo.Item2, idQueryInfo.Item1, info);

            string constructionQuery = _queryService.BuildDataPointQuery();

            List<FlatDataPointComponent> flat = await _genericDataAccess.Query<FlatDataPointComponent>(constructionQuery, new { ids }, info);

            List<QueriedDataPoint> dataPoints = new List<QueriedDataPoint>();
            ids.ForEach(id =>
            {
                List<QueriedDataPointParameter> parameters = flat.Where(p => id.Equals(p.Id)).Select(p => new QueriedDataPointParameter()
                {
                    TypeParameterId = p.TypeParameterId,
                    TypeParameterSummary = p.TypeParameterSummary,
                    TypeParameterTypeValue = p.TypeParameterTypeValue,
                    TypeParameterSequence = p.TypeParameterSequence,
                    DataPointTypeReferenceId = p.DataPointTypeReferenceId,
                    TypeParameterSettings = p.TypeParameterSettings,
                    Value = p.Value,
                }).ToList();
                FlatDataPointComponent first = flat.Where(p => id.Equals(p.Id)).First();
                QueriedDataPoint dp = new QueriedDataPoint()
                {
                    Id = id,
                    Name = first.Name,
                    Settings = first.Settings,
                    TypeId = first.TypeId,
                    TypeName = first.TypeName,
                    TypeSummary = first.TypeSummary,
                    TypeSettings = first.TypeSettings,
                    Parameters = parameters,
                };
                dataPoints.Add(dp);
            });
            return new DataPointQueryResult() { Count = count, Results = dataPoints };
        }
    }
}
