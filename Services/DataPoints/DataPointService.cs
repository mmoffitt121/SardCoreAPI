using LinqKit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using SardCoreAPI.DataAccess;
using SardCoreAPI.DataAccess.DataPoints;
using SardCoreAPI.Models.DataPoints;
using SardCoreAPI.Models.DataPoints.DataPointParameters;
using SardCoreAPI.Models.DataPoints.Queried;
using SardCoreAPI.Models.Hub.Worlds;
using SardCoreAPI.Models.Map.Location;
using SardCoreAPI.Services.Context;
using SardCoreAPI.Services.WorldContext;
using SardCoreAPI.Utility.DataAccess;
using System.Dynamic;
using System.Linq;
using System.Numerics;
using System.Runtime.Intrinsics.Arm;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SardCoreAPI.Services.DataPoints
{
    public interface IDataPointService
    {
        public Task<DataPointQueryResult> GetEmpty(int typeId);
        public Task<DataPointQueryResult> GetDataPoints(DataPointSearchCriteria criteria);
        public Task<int> PutDataPoint(DataPoint dp);
        public Task DeleteDataPoint(int id);
    }

    public class DataPointService : IDataPointService
    {
        private readonly IDataPointQueryService _queryService;
        private IGenericDataAccess _genericDataAccess;
        private IWorldInfoService _worldInfoService;
        private IDataPointTypeDataAccess _typeDataAccess;
        private readonly IDataService data;

        public DataPointService(IDataPointQueryService queryService, IGenericDataAccess genericDataAccess, IWorldInfoService worldInfoService, 
            IDataPointTypeDataAccess dataPointTypeDataAccess, IDataService data)
        {
            _genericDataAccess = genericDataAccess;
            _queryService = queryService;
            _worldInfoService = worldInfoService;
            _typeDataAccess = dataPointTypeDataAccess;
            this.data = data;
        }

        public async Task<DataPointQueryResult> GetEmpty(int typeId)
        {
            DataPointType type = await data.Context.DataPointType
                .Where(t => t.Id.Equals(typeId))
                .Include(t => t.TypeParameters)
                .FirstAsync();

            QueriedDataPoint dp = new QueriedDataPoint()
            {
                Id = -1,
                Name = "New",
                Settings = "",
                TypeId = type.Id,
                TypeName = type.Name,
                TypeSummary = type.Summary,
                TypeSettings = type.Settings,
                Parameters = type.TypeParameters?.Select(tp => new QueriedDataPointParameter()
                {
                    TypeParameterId = tp.Id ?? -1,
                    TypeParameterName = tp.Name,
                    TypeParameterSummary = tp.Summary,
                    TypeParameterTypeValue = tp.TypeValue,
                    TypeParameterSequence = tp.Sequence,
                    DataPointTypeReferenceId = tp.DataPointTypeReferenceId,
                    TypeParameterSettings = tp.Settings ?? "",
                    Value = null,
                    ValueData = null,
                    IsMultiple = tp.IsMultiple ?? false
                }).ToList(),
            };

            return new DataPointQueryResult()
            {
                Count = 1,
                Types = [type],
                Results = [dp],
            };
        }

        public async Task<DataPointQueryResult> GetDataPoints(DataPointSearchCriteria criteria)
        {
            // Get relevant locations TODO

            // Get relevant data points TODO


            IQueryable<DataPoint> queryable = data.Context.DataPoint.Include(dp => dp.Type);

            // Filtering
            if (criteria.Id != null && criteria.Id != -1) queryable = queryable.Where(dp => dp.Id.Equals(criteria.Id));
            if (criteria.TypeId != null && criteria.TypeId != -1) queryable = queryable.Where(dp => dp.TypeId.Equals(criteria.TypeId));
            if (criteria.DataPointIds != null) queryable = queryable.Where(dp => criteria.DataPointIds.Contains(dp.Id ?? -1));
            if (criteria.TypeIds != null) queryable = queryable.Where(dp => criteria.TypeIds.Contains(dp.TypeId));
            if (criteria.Query != null) queryable = queryable.Where(dp => dp.Name.Contains(criteria.Query));

            int? moveToFront = null;
            if (criteria.LocationIds != null)
            {
                List<DataPointLocation> dataPointLocations = data.Context.DataPointLocation.Where(l => criteria.LocationIds.Contains(l.LocationId)).ToList();
                List<int> dataPointLocationIds = dataPointLocations.Select(l => l.DataPointId).ToList();
                queryable = queryable.Where(dp => dataPointLocationIds.Contains(dp.Id ?? -1));

                moveToFront = dataPointLocations.FirstOrDefault(dpl => dpl.IsPrimary)?.DataPointId;
            }

            if (criteria.Parameters != null && criteria.ParameterSearchOptions != null)
            {
                if (criteria.Parameters.Count() != criteria.ParameterSearchOptions.Count()) throw new ArgumentException("Parameters and parameter search options did not match.");
                for (int i = 0; i < criteria.Parameters.Count(); i++)
                {
                    queryable = FilterByParam(queryable, criteria.Parameters[i], criteria.ParameterSearchOptions[i]);
                }
            }

            // Inclusion
            if (criteria.IncludeParameters == true) queryable = queryable.Include(dp => dp.Parameters).ThenInclude(p => p.DataPointTypeParameter);
            if (criteria.IncludeChildDataPoints == true) queryable = queryable.Include(dp => dp.Parameters).ThenInclude(p => ((DataPointParameterDataPoint)p).DataPointValue);
            if (criteria.IncludeChildParameters == true) queryable = queryable.Include(dp => dp.Parameters).ThenInclude(p => ((DataPointParameterDataPoint)p).DataPointValue).ThenInclude(dp => dp.Parameters);

            queryable = OrderByParam(queryable, criteria);

            List<DataPoint> dataPoints = await queryable.Paginate(criteria).ToListAsync();
            int count = await queryable.CountAsync();

            
            if (moveToFront != null)
            {
                DataPoint? primary = dataPoints.FirstOrDefault(dp => dp.Id.Equals(moveToFront));

                if (primary != null)
                {
                    dataPoints.RemoveAt(dataPoints.IndexOf(primary));
                    dataPoints.Insert(0, primary);
                }
            }

            List<DataPointTypeParameter> typeParams = data.Context.DataPointTypeParameter
                .Where(dptp => dataPoints
                    .Select(dp => dp.TypeId)
                    .ToList()
                    .Contains(dptp.DataPointTypeId))
                .OrderBy(t => t.Sequence)
                .ToList();

            // Include child data points

            List<QueriedDataPoint> queriedDataPoints = dataPoints.Select(dp => new QueriedDataPoint()
            {
                Id = dp.Id ?? -1,
                Name = dp.Name,
                Settings = dp.Type.Settings,
                TypeId = dp.TypeId,
                TypeName = dp.Type.Name,
                TypeSummary = dp.Type.Summary,
                TypeSettings = dp.Type.Settings,
                Parameters = (criteria.IncludeParameters == true) ? typeParams.Where(tp => tp.DataPointTypeId.Equals(dp.TypeId)).Select(typeParam => new QueriedDataPointParameter()
                {
                    TypeParameterId = (int)typeParam.Id,
                    TypeParameterName = typeParam.Name,
                    TypeParameterSummary = typeParam.Summary,
                    TypeParameterTypeValue = typeParam.TypeValue,
                    TypeParameterSequence = typeParam.Sequence,
                    DataPointTypeReferenceId = typeParam.DataPointTypeReferenceId,
                    TypeParameterSettings = typeParam.Settings,
                    Value = dp.Parameters.FirstOrDefault(p => p.DataPointTypeParameterId.Equals(typeParam.Id))?.GetStringValue(),
                    ValueData = 
                        (dp.Parameters.FirstOrDefault(p => p.DataPointTypeParameterId.Equals(typeParam.Id))?.GetType().IsAssignableFrom(typeof(DataPointParameterDataPoint)) ?? false)
                        ? (((DataPointParameterDataPoint)dp.Parameters.FirstOrDefault(p => p.DataPointTypeParameterId.Equals(typeParam.Id))).DataPointValue)
                        : (null),
                    Values = dp.Parameters.Where(p => p.DataPointTypeParameterId.Equals(typeParam.Id))?.Select(x => x.GetStringValue()).ToList(),
                    ValuesData =
                        (dp.Parameters.FirstOrDefault(p => p.DataPointTypeParameterId.Equals(typeParam.Id))?.GetType().IsAssignableFrom(typeof(DataPointParameterDataPoint)) ?? false && (typeParam.IsMultiple ?? false))
                        ? ((dp.Parameters.Where(p => p.DataPointTypeParameterId.Equals(typeParam.Id)).Select(x => (object)((DataPointParameterDataPoint)x).DataPointValue))).ToList()
                        : (null),
                    IsMultiple = typeParam.IsMultiple ?? false
                }).ToList() : null,
            }).ToList();

            

            List<DataPointType> types = null;
            if (criteria.IncludeTypes == true)
            {
                if (criteria.TypeIds != null)
                {
                    types = await data.Context.DataPointType.Where(t => (criteria.TypeIds ?? new List<int>()).Contains(t.Id)).ToListAsync();
                }
                else
                {
                    types = await data.Context.DataPointType.Where(t => dataPoints.Select(dp => dp.TypeId).Contains(t.Id)).ToListAsync();
                }
            }

            return new DataPointQueryResult(count, queriedDataPoints, types);
        }

        private IQueryable<DataPoint> FilterByParam(IQueryable<DataPoint> queryable, DataPointParameter searchParameter, ParameterSearchOptions searchOptions)
        {
            switch (data.Context.DataPointTypeParameter.Single(x => x.Id.Equals(searchParameter.DataPointTypeParameterId)).TypeValue)
            {
                case "bit":
                    bool boolValue = ((DataPointParameterBoolean)searchParameter).BoolValue;
                    switch (searchOptions.FilterMode)
                    {
                        case ParameterSearchOptions.FilterModeEnum.Equals:
                        case ParameterSearchOptions.FilterModeEnum.True:
                        case ParameterSearchOptions.FilterModeEnum.False:
                            queryable = queryable.Where(dp =>
                                ((DataPointParameterBoolean)dp.Parameters.Where(p => p.DataPointTypeParameter.Id.Equals(searchParameter.DataPointTypeParameterId)).First())
                                .BoolValue.Equals(boolValue));
                            break;
                    }
                    
                    break;
                case "dat":
                    int dataPointValue = ((DataPointParameterDataPoint)searchParameter).DataPointValueId;
                    switch (searchOptions.FilterMode)
                    {
                        case ParameterSearchOptions.FilterModeEnum.Equals:
                            queryable = queryable.Where(dp =>
                                ((DataPointParameterDataPoint)dp.Parameters.Where(p => p.DataPointTypeParameter.Id.Equals(searchParameter.DataPointTypeParameterId)).First())
                                .DataPointValueId.Equals(dataPointValue));
                            break;
                    }
                    break;
                case "doc":
                    string documentValue = ((DataPointParameterDocument)searchParameter).DocumentValue;
                    switch (searchOptions.FilterMode)
                    {
                        case ParameterSearchOptions.FilterModeEnum.Equals:
                            queryable = queryable.Where(dp =>
                                ((DataPointParameterDocument)dp.Parameters.Where(p => p.DataPointTypeParameter.Id.Equals(searchParameter.DataPointTypeParameterId)).First())
                                .DocumentValue.Equals(documentValue));
                            break;
                        case ParameterSearchOptions.FilterModeEnum.Contains:
                            queryable = queryable.Where(dp =>
                                ((DataPointParameterDocument)dp.Parameters.Where(p => p.DataPointTypeParameter.Id.Equals(searchParameter.DataPointTypeParameterId)).First())
                                .DocumentValue.Contains(documentValue));
                            break;
                    }
                    break;
                case "dub":
                    double doubleValue = ((DataPointParameterDouble)searchParameter).DoubleValue ?? 0;
                    switch (searchOptions.FilterMode)
                    {
                        case ParameterSearchOptions.FilterModeEnum.Equals:
                            queryable = queryable.Where(dp =>
                                ((DataPointParameterDouble)dp.Parameters.Where(p => p.DataPointTypeParameter.Id.Equals(searchParameter.DataPointTypeParameterId)).First())
                                .DoubleValue.Equals(doubleValue));
                            break;
                        case ParameterSearchOptions.FilterModeEnum.GreaterThan:
                            queryable = queryable.Where(dp =>
                                ((DataPointParameterDouble)dp.Parameters.Where(p => p.DataPointTypeParameter.Id.Equals(searchParameter.DataPointTypeParameterId)).First())
                                .DoubleValue >= doubleValue);
                            break;
                        case ParameterSearchOptions.FilterModeEnum.LessThan:
                            queryable = queryable.Where(dp =>
                                ((DataPointParameterDouble)dp.Parameters.Where(p => p.DataPointTypeParameter.Id.Equals(searchParameter.DataPointTypeParameterId)).First())
                                .DoubleValue <= doubleValue);
                            break;
                    }
                    break;
                case "int":
                    long? intValue = ((DataPointParameterInt)searchParameter).IntValue;
                    switch (searchOptions.FilterMode)
                    {
                        case ParameterSearchOptions.FilterModeEnum.Equals:
                            queryable = queryable.Where(dp =>
                                ((DataPointParameterInt)dp.Parameters.Where(p => p.DataPointTypeParameter.Id.Equals(searchParameter.DataPointTypeParameterId)).First())
                                .IntValue.Equals(intValue));
                            break;
                        case ParameterSearchOptions.FilterModeEnum.GreaterThan:
                            queryable = queryable.Where(dp =>
                                ((DataPointParameterInt)dp.Parameters.Where(p => p.DataPointTypeParameter.Id.Equals(searchParameter.DataPointTypeParameterId)).First())
                                .IntValue >= intValue);
                            break;
                        case ParameterSearchOptions.FilterModeEnum.LessThan:
                            queryable = queryable.Where(dp =>
                                ((DataPointParameterInt)dp.Parameters.Where(p => p.DataPointTypeParameter.Id.Equals(searchParameter.DataPointTypeParameterId)).First())
                                .IntValue <= intValue);
                            break;
                    }
                    break;
                case "str":
                    string stringValue = ((DataPointParameterString)searchParameter).StringValue;
                    switch (searchOptions.FilterMode)
                    {
                        case ParameterSearchOptions.FilterModeEnum.Equals:
                            queryable = queryable.Where(dp =>
                                ((DataPointParameterString)dp.Parameters.Where(p => p.DataPointTypeParameter.Id.Equals(searchParameter.DataPointTypeParameterId)).First())
                                .StringValue.Equals(stringValue));
                            break;
                        case ParameterSearchOptions.FilterModeEnum.Contains:
                            queryable = queryable.Where(dp =>
                                ((DataPointParameterString)dp.Parameters.Where(p => p.DataPointTypeParameter.Id.Equals(searchParameter.DataPointTypeParameterId)).First())
                                .StringValue.Contains(stringValue));
                            break;
                        case ParameterSearchOptions.FilterModeEnum.StartsWith:
                            queryable = queryable.Where(dp =>
                                ((DataPointParameterString)dp.Parameters.Where(p => p.DataPointTypeParameter.Id.Equals(searchParameter.DataPointTypeParameterId)).First())
                                .StringValue.StartsWith(stringValue));
                            break;
                        case ParameterSearchOptions.FilterModeEnum.EndsWith:
                            queryable = queryable.Where(dp =>
                                ((DataPointParameterString)dp.Parameters.Where(p => p.DataPointTypeParameter.Id.Equals(searchParameter.DataPointTypeParameterId)).First())
                                .StringValue.EndsWith(stringValue));
                            break;
                    }
                    break;
                case "sum":
                    string summaryValue = ((DataPointParameterSummary)searchParameter).SummaryValue;
                    switch (searchOptions.FilterMode)
                    {
                        case ParameterSearchOptions.FilterModeEnum.Equals:
                            queryable = queryable.Where(dp =>
                                ((DataPointParameterSummary)dp.Parameters.Where(p => p.DataPointTypeParameter.Id.Equals(searchParameter.DataPointTypeParameterId)).First())
                                .SummaryValue.Equals(summaryValue));
                            break;
                        case ParameterSearchOptions.FilterModeEnum.Contains:
                            queryable = queryable.Where(dp =>
                                ((DataPointParameterSummary)dp.Parameters.Where(p => p.DataPointTypeParameter.Id.Equals(searchParameter.DataPointTypeParameterId)).First())
                                .SummaryValue.Contains(summaryValue));
                            break;
                        case ParameterSearchOptions.FilterModeEnum.StartsWith:
                            queryable = queryable.Where(dp =>
                                ((DataPointParameterSummary)dp.Parameters.Where(p => p.DataPointTypeParameter.Id.Equals(searchParameter.DataPointTypeParameterId)).First())
                                .SummaryValue.StartsWith(summaryValue));
                            break;
                        case ParameterSearchOptions.FilterModeEnum.EndsWith:
                            queryable = queryable.Where(dp =>
                                ((DataPointParameterSummary)dp.Parameters.Where(p => p.DataPointTypeParameter.Id.Equals(searchParameter.DataPointTypeParameterId)).First())
                                .SummaryValue.EndsWith(summaryValue));
                            break;
                    }
                    break;
                case "tim":
                    BigInteger? timeValue = ((DataPointParameterTime)searchParameter).TimeValue;
                    switch (searchOptions.FilterMode)
                    {
                        case ParameterSearchOptions.FilterModeEnum.Equals:
                            queryable = queryable.Where(dp =>
                                ((DataPointParameterTime)dp.Parameters.Where(p => p.DataPointTypeParameter.Id.Equals(searchParameter.DataPointTypeParameterId)).First())
                                .TimeValue.Equals(timeValue));
                            break;
                        case ParameterSearchOptions.FilterModeEnum.GreaterThan:
                            queryable = queryable.Where(dp =>
                                ((DataPointParameterTime)dp.Parameters.Where(p => p.DataPointTypeParameter.Id.Equals(searchParameter.DataPointTypeParameterId)).First())
                                .TimeValue >= timeValue);
                            break;
                        case ParameterSearchOptions.FilterModeEnum.LessThan:
                            queryable = queryable.Where(dp =>
                                ((DataPointParameterTime)dp.Parameters.Where(p => p.DataPointTypeParameter.Id.Equals(searchParameter.DataPointTypeParameterId)).First())
                                .TimeValue <= timeValue);
                            break;
                    }
                    break;
                case "uni":
                    double unitValue = ((DataPointParameterUnit)searchParameter).UnitValue;
                    switch (searchOptions.FilterMode)
                    {
                        case ParameterSearchOptions.FilterModeEnum.Equals:
                            queryable = queryable.Where(dp =>
                                ((DataPointParameterUnit)dp.Parameters.Where(p => p.DataPointTypeParameter.Id.Equals(searchParameter.DataPointTypeParameterId)).First())
                                .UnitValue.Equals(unitValue));
                            break;
                        case ParameterSearchOptions.FilterModeEnum.GreaterThan:
                            queryable = queryable.Where(dp =>
                                ((DataPointParameterUnit)dp.Parameters.Where(p => p.DataPointTypeParameter.Id.Equals(searchParameter.DataPointTypeParameterId)).First())
                                .UnitValue >= unitValue);
                            break;
                        case ParameterSearchOptions.FilterModeEnum.LessThan:
                            queryable = queryable.Where(dp =>
                                ((DataPointParameterUnit)dp.Parameters.Where(p => p.DataPointTypeParameter.Id.Equals(searchParameter.DataPointTypeParameterId)).First())
                                .UnitValue <= unitValue);
                            break;
                    }
                    break;
                default:
                    break;
            }

            return queryable;
        }

        private IQueryable<DataPoint> OrderByParam(IQueryable<DataPoint> queryable, DataPointSearchCriteria criteria)
        {
            if (criteria.OrderByTypeParam == null)
            {
                if (criteria.OrderByTypeParamDesc == true)
                {
                    queryable = queryable.OrderByDescending(dp => dp.Name);
                }
                else
                {
                    queryable = queryable.OrderBy(dp => dp.Name);
                }
                return queryable;
            }

            switch (criteria.OrderByTypeParam.TypeValue)
            {
                case "bit":
                    if (criteria.OrderByTypeParamDesc == true)
                    {
                        queryable = queryable.OrderByDescending(dp => ((DataPointParameterBoolean)dp.Parameters.FirstOrDefault(p => p.DataPointTypeParameterId == criteria.OrderByTypeParam.Id)).BoolValue);
                    }
                    else
                    {
                        queryable = queryable.OrderBy(dp => ((DataPointParameterBoolean)dp.Parameters.FirstOrDefault(p => p.DataPointTypeParameterId == criteria.OrderByTypeParam.Id)).BoolValue);
                    }
                    break;
                case "dat":
                    break;
                case "doc":
                    break;
                case "dub":
                    if (criteria.OrderByTypeParamDesc == true)
                    {
                        queryable = queryable.OrderByDescending(dp => ((DataPointParameterDouble)dp.Parameters.FirstOrDefault(p => p.DataPointTypeParameterId == criteria.OrderByTypeParam.Id)).DoubleValue);
                    }
                    else
                    {
                        queryable = queryable.OrderBy(dp => ((DataPointParameterDouble)dp.Parameters.FirstOrDefault(p => p.DataPointTypeParameterId == criteria.OrderByTypeParam.Id)).DoubleValue);
                    }
                    break;
                case "int":
                    if (criteria.OrderByTypeParamDesc == true)
                    {
                        queryable = queryable.OrderByDescending(dp => ((DataPointParameterInt)dp.Parameters.FirstOrDefault(p => p.DataPointTypeParameterId == criteria.OrderByTypeParam.Id)).IntValue);
                    }
                    else
                    {
                        queryable = queryable.OrderBy(dp => ((DataPointParameterInt)dp.Parameters.FirstOrDefault(p => p.DataPointTypeParameterId == criteria.OrderByTypeParam.Id)).IntValue);
                    }
                    break;
                case "str":
                    if (criteria.OrderByTypeParamDesc == true)
                    {
                        queryable = queryable.OrderByDescending(dp => ((DataPointParameterString)dp.Parameters.FirstOrDefault(p => p.DataPointTypeParameterId == criteria.OrderByTypeParam.Id)).StringValue);
                    }
                    else
                    {
                        queryable = queryable.OrderBy(dp => ((DataPointParameterString)dp.Parameters.FirstOrDefault(p => p.DataPointTypeParameterId == criteria.OrderByTypeParam.Id)).StringValue);
                    }
                    break;
                case "sum":
                    if (criteria.OrderByTypeParamDesc == true)
                    {
                        queryable = queryable.OrderByDescending(dp => ((DataPointParameterSummary)dp.Parameters.FirstOrDefault(p => p.DataPointTypeParameterId == criteria.OrderByTypeParam.Id)).SummaryValue);
                    }
                    else
                    {
                        queryable = queryable.OrderBy(dp => ((DataPointParameterSummary)dp.Parameters.FirstOrDefault(p => p.DataPointTypeParameterId == criteria.OrderByTypeParam.Id)).SummaryValue);
                    }
                    break;
                case "tim":
                    if (criteria.OrderByTypeParamDesc == true)
                    {
                        queryable = queryable.OrderByDescending(dp => ((DataPointParameterTime)dp.Parameters.FirstOrDefault(p => p.DataPointTypeParameterId == criteria.OrderByTypeParam.Id)).TimeValue);
                    }
                    else
                    {
                        queryable = queryable.OrderBy(dp => ((DataPointParameterTime)dp.Parameters.FirstOrDefault(p => p.DataPointTypeParameterId == criteria.OrderByTypeParam.Id)).TimeValue);
                    }
                    break;
                case "uni":
                    if (criteria.OrderByTypeParamDesc == true)
                    {
                        queryable = queryable.OrderByDescending(dp => ((DataPointParameterUnit)dp.Parameters.FirstOrDefault(p => p.DataPointTypeParameterId == criteria.OrderByTypeParam.Id)).UnitValue);
                    }
                    else
                    {
                        queryable = queryable.OrderBy(dp => ((DataPointParameterUnit)dp.Parameters.FirstOrDefault(p => p.DataPointTypeParameterId == criteria.OrderByTypeParam.Id)).UnitValue);
                    }
                    break;
                default:
                    break;
            
            }
            return queryable;
        }

        public async Task<int> PutDataPoint(DataPoint dp)
        {
            if (dp.Id == -1)
            {
                dp.Id = null;
            }

            DataPoint? newdp = data.Context.DataPoint.FirstOrDefault(d => d.Id.Equals(dp.Id));
            if (newdp == null)
            {
                data.Context.DataPoint.Add(dp);
            }
            else
            {
                await ClearParameters(dp);
                newdp.Parameters = dp.Parameters;
                newdp.Name = dp.Name;
                newdp.TypeId = dp.TypeId;
                data.Context.DataPoint.Update(newdp);
            }

            
            await data.Context.SaveChangesAsync();
            return dp.Id ?? -1;
        }

        private async Task ClearParameters(DataPoint dp)
        {
            if (dp.Id == -1)
            {
                return;
            }
            List<DataPointParameter> parameters = data.Context.DataPointParameter.Where(p => p.DataPointId == dp.Id).ToList();
            data.Context.RemoveRange(parameters);
        }

        public async Task DeleteDataPoint(int id)
        {
            await data.Context.DataPoint.Where(d => d.Id.Equals(id)).ExecuteDeleteAsync();
        }
    }
}
