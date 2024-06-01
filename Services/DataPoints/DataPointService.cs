using SardCoreAPI.DataAccess;
using SardCoreAPI.DataAccess.DataPoints;
using SardCoreAPI.Models.DataPoints;
using SardCoreAPI.Models.DataPoints.DataPointParameters;
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
        private IDataPointTypeDataAccess _typeDataAccess;

        public DataPointService(IDataPointQueryService queryService, IGenericDataAccess genericDataAccess, IWorldInfoService worldInfoService, 
            IDataPointTypeDataAccess dataPointTypeDataAccess)
        {
            _genericDataAccess = genericDataAccess;
            _queryService = queryService;
            _worldInfoService = worldInfoService;
            _typeDataAccess = dataPointTypeDataAccess;
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

            List<DataPoint> baseDataPoints = await _genericDataAccess.Query<DataPoint>(idQueryInfo.Item2, idQueryInfo.Item1, info);
            List<int> ids = baseDataPoints.Select(x => x.Id ?? -1).ToList();

            string constructionQuery = _queryService.BuildDataPointQuery(criteria, idQueryInfo.Item1);

            idQueryInfo.Item1.TryAdd("ids", ids);

            List<FlatDataPointComponent> flat = await _genericDataAccess.Query<FlatDataPointComponent>(constructionQuery, idQueryInfo.Item1, info);

            List<QueriedDataPoint> dataPoints = new List<QueriedDataPoint>();
            baseDataPoints.ForEach(dp =>
            {
                List<QueriedDataPointParameter> parameters = flat.Where(p => dp.Id.Equals(p.Id)).Select(p => new QueriedDataPointParameter()
                {
                    TypeParameterId = p.TypeParameterId,
                    TypeParameterName = p.TypeParameterName,
                    TypeParameterSummary = p.TypeParameterSummary,
                    TypeParameterTypeValue = p.TypeParameterTypeValue,
                    TypeParameterSequence = p.TypeParameterSequence,
                    DataPointTypeReferenceId = p.DataPointTypeReferenceId,
                    TypeParameterSettings = p.TypeParameterSettings,
                    Value = p.Value,
                }).ToList();
                FlatDataPointComponent? first = flat.Where(p => dp.Id.Equals(p.Id)).FirstOrDefault();
                QueriedDataPoint qdp = new QueriedDataPoint()
                {
                    Id = dp.Id ?? -1,
                    Name = dp.Name,
                    Settings = first?.Settings,
                    TypeId = dp.TypeId,
                    TypeName = first?.TypeName ?? "",
                    TypeSummary = first?.TypeSummary,
                    TypeSettings = first?.TypeSettings,
                    Parameters = parameters,
                };
                dataPoints.Add(qdp);
            });

            List<DataPointType>? types;

            if (criteria.IncludeTypes == true)
            {
                types = await _typeDataAccess.GetDataPointTypes(new DataPointTypeSearchCriteria() { DataPointTypeIds = criteria.TypeIds?.ToArray() ?? new int[0] }, _worldInfoService.GetWorldInfo());
            }
            else
            {
                types = null;
            }

            // TODO: Relevant locations and related data points

            return new DataPointQueryResult() { Count = count, Results = dataPoints, Types = types };
        }
    }
}
