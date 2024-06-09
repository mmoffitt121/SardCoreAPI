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
            // Build Queries
            (ExpandoObject, string) idQueryInfo = _queryService.BuildIdQuery(criteria);
            (ExpandoObject, string) countQueryInfo = _queryService.BuildCountQuery(criteria);
            WorldInfo info = _worldInfoService.GetWorldInfo();

            // Get count
            int count = await _genericDataAccess.QueryFirst<int>(countQueryInfo.Item2, countQueryInfo.Item1, info);

            if (count == 0)
            {
                return new DataPointQueryResult() { Count = 0, Results = new List<QueriedDataPoint>() };
            }

            // Get base list
            List<DataPoint> baseDataPoints = await _genericDataAccess.Query<DataPoint>(idQueryInfo.Item2, idQueryInfo.Item1, info);
            
            // Add parameters if requested
            List<QueriedDataPoint> dataPoints = new List<QueriedDataPoint>();
            if (criteria.IncludeParameters == true)
            {
                List<int> ids = baseDataPoints.Select(x => x.Id ?? -1).ToList();

                // Build construction query
                string constructionQuery = _queryService.BuildDataPointQuery(criteria, idQueryInfo.Item1);

                idQueryInfo.Item1.TryAdd("ids", ids);

                // Get flat list of needed parameters
                List<FlatDataPointComponent> flat = await _genericDataAccess.Query<FlatDataPointComponent>(constructionQuery, idQueryInfo.Item1, info);

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
            }
            else
            {
                // We don't need the parameters, so just format data point as queried data point
                baseDataPoints.ForEach(dp =>
                {
                    QueriedDataPoint qdp = new QueriedDataPoint()
                    {
                        Id = dp.Id ?? -1,
                        Name = dp.Name,
                        Settings = null,
                        TypeId = dp.TypeId,
                        TypeName = "",
                        TypeSummary = null,
                        TypeSettings = null,
                        Parameters = null,
                    };
                    dataPoints.Add(qdp);
                });
            }
            

            // Get attached datapoints
            if (criteria.IncludeChildDataPoints == true)
            {
                List<QueriedDataPoint> childResult;
                DataPointSearchCriteria childCriteria = new DataPointSearchCriteria();
                childCriteria.DataPointIds = new List<int>();
                childCriteria.IncludeParameters = criteria.IncludeChildParameters;
                dataPoints.ForEach(dp =>
                {
                    dp.Parameters?.ForEach(p =>
                    {
                        int childId;
                        if ("dat".Equals(p.TypeParameterTypeValue) && Int32.TryParse(p.Value, out childId))
                        {
                            childCriteria.DataPointIds.Add(childId);
                        }
                    });
                });

                childResult = (await GetList(childCriteria)).Results;

                dataPoints.ForEach(dp =>
                {
                    dp.Parameters?.ForEach(p =>
                    {
                        int childId;
                        if ("dat".Equals(p.TypeParameterTypeValue) && Int32.TryParse(p.Value, out childId))
                        {
                            p.ValueData = childResult.Find(c => c.Id == childId);
                        }
                    });
                });
            }

            // Get attached units TODO

            // Get included types if requested
            List<DataPointType>? types;

            if (criteria.IncludeTypes == true)
            {
                types = await _typeDataAccess.GetDataPointTypes(new DataPointTypeSearchCriteria() { DataPointTypeIds = criteria.TypeIds?.ToArray() ?? new int[0] }, _worldInfoService.GetWorldInfo());
            }
            else
            {
                types = null;
            }



            // Get relevant locations TODO

            // Get relevant data points TODO

            return new DataPointQueryResult() { Count = count, Results = dataPoints, Types = types };
        }
    }
}
