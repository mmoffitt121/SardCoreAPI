using Microsoft.AspNetCore.Mvc;
using SardCoreAPI.Controllers.Map;
using SardCoreAPI.DataAccess.DataPoints;
using SardCoreAPI.Models.Common;
using SardCoreAPI.Models.DataPoints.DataPointParameters;
using SardCoreAPI.Models.DataPoints;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using SardCoreAPI.Models.Hub.Worlds;
using SardCoreAPI.DataAccess.Units;
using SardCoreAPI.Models.Units;

namespace SardCoreAPI.Controllers.DataPoints
{
    [ApiController]
    [Route("Library/[controller]/[action]")]
    public class DataPointController : GenericController
    {
        private readonly ILogger<MapController> _logger;

        public DataPointController(ILogger<MapController> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "GetDataPoints")]
        public async Task<IActionResult> GetDataPoints([FromBody] DataPointSearchCriteria criteria)
        {
            if (criteria == null) { return new BadRequestResult(); }

            List<DataPoint> result = await new DataPointDataAccess().GetDataPoints(criteria, WorldInfo);
            if (result != null)
            {
                return new OkObjectResult(result);
            }
            return new BadRequestResult();
        }

        [HttpPost(Name = "GetDataPointsCount")]
        public async Task<IActionResult> GetDataPointsCount([FromBody] DataPointSearchCriteria criteria)
        {
            if (criteria == null) { return new BadRequestResult(); }

            int? result = await new DataPointDataAccess().GetDataPointsCount(criteria, WorldInfo);
            if (result != null)
            {
                return new OkObjectResult(result);
            }
            return new BadRequestResult();
        }

        [HttpGet]
        public async Task<IActionResult> GetDataPointsReferencingDataPoint(int? id)
        {
            if (id == null) { return new BadRequestResult(); }

            List<DataPoint> result = await new DataPointDataAccess().GetDataPointsReferencingDataPoint((int)id, WorldInfo);
            if (result != null)
            {
                return new OkObjectResult(result);
            }
            return new BadRequestResult();

        }

        [HttpGet(Name = "GetDataPoint")]
        public async Task<IActionResult> GetDataPoint([FromQuery] int? Id)
        {
            if (Id == null) { return new BadRequestResult(); }

            // Get Data Point
            DataPoint? result = (await new DataPointDataAccess().GetDataPoints(new DataPointSearchCriteria() { Id = Id }, WorldInfo)).FirstOrDefault();

            if (result == null) { return new NotFoundResult(); }

            // Attach Type
            DataPointType? type = (await new DataPointTypeDataAccess().GetDataPointTypes(new DataPointTypeSearchCriteria() { Id = result.TypeId }, WorldInfo)).FirstOrDefault();

            if (type == null) { return new OkObjectResult(result); }

            // Attach Type Parameters
            List<DataPointTypeParameter> parameters = (await new DataPointTypeParameterDataAccess().GetDataPointTypeParameters(type.Id, WorldInfo)).ToList();
            type.TypeParameters = parameters;

            if (type.TypeParameters == null) { return new OkObjectResult(result); }

            // Attach Parameters
            result.Parameters = new List<DataPointParameter>();
            DataPointParameterDataAccess dataAccess = new DataPointParameterDataAccess();
            foreach (DataPointTypeParameter tp in type.TypeParameters)
            {
                if (tp.Id == null || result.Id == null) { continue; }
                DataPointParameter p;

                switch (tp.TypeValue)
                {
                    case "int":
                        result.Parameters.Add(await dataAccess.GetParameter<DataPointParameterInt>((int)result.Id, (int)tp.Id, tp.TypeValue, WorldInfo));
                        break;
                    case "dub":
                        result.Parameters.Add(await dataAccess.GetParameter<DataPointParameterDouble>((int)result.Id, (int)tp.Id, tp.TypeValue, WorldInfo));
                        break;
                    case "str":
                        result.Parameters.Add(await dataAccess.GetParameter<DataPointParameterString>((int)result.Id, (int)tp.Id, tp.TypeValue, WorldInfo));
                        break;
                    case "sum":
                        result.Parameters.Add(await dataAccess.GetParameter<DataPointParameterSummary>((int)result.Id, (int)tp.Id, tp.TypeValue, WorldInfo));
                        break;
                    case "doc":
                        result.Parameters.Add(await dataAccess.GetParameter<DataPointParameterDocument>((int)result.Id, (int)tp.Id, tp.TypeValue, WorldInfo));
                        break;
                    case "dat":
                        result.Parameters.Add(await dataAccess.GetParameter<DataPointParameterDataPoint>((int)result.Id, (int)tp.Id, tp.TypeValue, WorldInfo));
                        break;
                    case "bit":
                        result.Parameters.Add(await dataAccess.GetParameter<DataPointParameterBoolean>((int)result.Id, (int)tp.Id, tp.TypeValue, WorldInfo));
                        break;
                    case "uni":
                        result.Parameters.Add(await dataAccess.GetParameter<DataPointParameterUnit>((int)result.Id, (int)tp.Id, tp.TypeValue, WorldInfo));
                        UnitSearchCriteria unitSearch = new UnitSearchCriteria() { Id = tp.DataPointTypeReferenceId };
                        List<Unit> unitList = await new UnitDataAccess().GetUnits(unitSearch, WorldInfo);
                        if (unitList.Count() < 1 || result.Parameters.Last() == null) break;
                        ((DataPointParameterUnit)result.Parameters.Last()).Unit = unitList?.First();
                        
                        break;
                    case "tim":
                        result.Parameters.Add(await dataAccess.GetParameter<DataPointParameterTime>((int)result.Id, (int)tp.Id, tp.TypeValue, WorldInfo));
                        break;
                    default:
                        result.Parameters.Add(await dataAccess.GetParameter<DataPointParameter>((int)result.Id, (int)tp.Id, tp.TypeValue, WorldInfo));
                        break;
                }
            }

            return new OkObjectResult(result);
        }

        [Authorize(Roles = "Administrator,Editor")]
        [HttpPut(Name = "PutDataPoint")]
        public async Task<IActionResult> PutDataPoint([FromBody] DataPoint data)
        {
            if (data == null) { return new BadRequestResult(); }

            // Get data point type
            DataPointType? type = (await new DataPointTypeDataAccess().GetDataPointTypes(new DataPointTypeSearchCriteria() { Id = data.TypeId }, WorldInfo)).FirstOrDefault();
            if (type == null) { return new BadRequestResult(); }
            // Attach type Parameters
            List<DataPointTypeParameter> typeParameters = (await new DataPointTypeParameterDataAccess().GetDataPointTypeParameters(type.Id, WorldInfo)).ToList();
            type.TypeParameters = typeParameters;

            int? id = null;

            // Post initial data point
            if (data.Id == null)
            {
                id = await new DataPointDataAccess().PostDataPoint(data, WorldInfo);
            }
            else
            {
                id = data.Id;
                await new DataPointDataAccess().PutDataPoint(data, WorldInfo);
            }

            if (id == null)
            {
                return new BadRequestResult();
            }

            // Initialize parameter data access
            DataPointParameterDataAccess parameterDataAccess = new DataPointParameterDataAccess();

            // Put all parameters
            if (data.Parameters == null) { return new OkResult(); }
            List<Task> tasks = new List<Task>();
            foreach (DataPointParameter param in data.Parameters)
            {
                param.DataPointId = id;
                string? typeValue = type.TypeParameters.Where(p => p.Id == param.DataPointTypeParameterId).FirstOrDefault()?.TypeValue;
                if (typeValue != null && param.GetType() != typeof(DataPointParameter))
                {
                    param.DataPointId = (int)id;
                    tasks.Add(parameterDataAccess.PutParameter(param, typeValue, WorldInfo));
                }
                else
                {
                    // Handle invalid type err here
                }
            }
            await Task.WhenAll(tasks);

            return new OkObjectResult(id);
        }

        [Authorize(Roles = "Administrator,Editor")]
        [HttpDelete(Name = "DeleteDataPoint")]
        public async Task<IActionResult> DeleteDataPoint([FromQuery] int? Id)
        {
            if (Id == null) { return new BadRequestResult(); }

            try
            {
                await new DataPointParameterDataAccess().DeleteParameters(Id, WorldInfo);

                int result = await new DataPointDataAccess().DeleteDataPoint((int)Id, WorldInfo);

                if (result > 0)
                {
                    return new OkResult();
                }
                else if (result == 0)
                {
                    return new NotFoundResult();
                }
                else
                {
                    return new BadRequestResult();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                return BadRequest("Unable to delete data point.");
            }
        }
    }
}
