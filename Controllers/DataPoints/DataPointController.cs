using Microsoft.AspNetCore.Mvc;
using SardCoreAPI.Controllers.Map;
using SardCoreAPI.DataAccess.DataPoints;
using SardCoreAPI.Models.Common;
using SardCoreAPI.Models.DataPoints.DataPointParameters;
using SardCoreAPI.Models.DataPoints;
using Microsoft.AspNetCore.Authorization;

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

        [HttpGet(Name = "GetDataPoints")]
        public async Task<IActionResult> GetDataPoints([FromQuery] DataPointSearchCriteria criteria)
        {
            if (criteria == null) { return new BadRequestResult(); }

            List<DataPoint> result = await new DataPointDataAccess().GetDataPoints(criteria, WorldInfo);
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
            DataPointType? type = (await new DataPointTypeDataAccess().GetDataPointTypes(new PagedSearchCriteria() { Id = result.TypeId }, WorldInfo)).FirstOrDefault();

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
                if (tp.Id == null) { continue; }
                DataPointParameter p;

                switch (tp.TypeValue)
                {
                    case "int":
                        result.Parameters.Add(await dataAccess.GetParameter<DataPointParameterInt>(result.Id, (int)tp.Id, tp.TypeValue, WorldInfo));
                        break;
                    case "dub":
                        result.Parameters.Add(await dataAccess.GetParameter<DataPointParameterDouble>(result.Id, (int)tp.Id, tp.TypeValue, WorldInfo));
                        break;
                    case "str":
                        result.Parameters.Add(await dataAccess.GetParameter<DataPointParameterString>(result.Id, (int)tp.Id, tp.TypeValue, WorldInfo));
                        break;
                    case "sum":
                        result.Parameters.Add(await dataAccess.GetParameter<DataPointParameterSummary>(result.Id, (int)tp.Id, tp.TypeValue, WorldInfo));
                        break;
                    case "doc":
                        result.Parameters.Add(await dataAccess.GetParameter<DataPointParameterDocument>(result.Id, (int)tp.Id, tp.TypeValue, WorldInfo));
                        break;
                    case "dat":
                        result.Parameters.Add(await dataAccess.GetParameter<DataPointParameterDataPoint>(result.Id, (int)tp.Id, tp.TypeValue, WorldInfo));
                        break;
                    case "bit":
                        result.Parameters.Add(await dataAccess.GetParameter<DataPointParameterBoolean>(result.Id, (int)tp.Id, tp.TypeValue, WorldInfo));
                        break;
                    default:
                        result.Parameters.Add(await dataAccess.GetParameter<DataPointParameter>(result.Id, (int)tp.Id, tp.TypeValue, WorldInfo));
                        break;
                }
            }

            return new OkObjectResult(result);
        }

        [Authorize(Roles = "Administrator,Editor")]
        [HttpPost(Name = "PostDataPoint")]
        public async Task<IActionResult> PostDataPoint([FromBody] DataPoint data)
        {
            if (data == null) { return new BadRequestResult(); }

            // Get data point type
            DataPointType? type = (await new DataPointTypeDataAccess().GetDataPointTypes(new PagedSearchCriteria() { Id = data.TypeId }, WorldInfo)).FirstOrDefault();
            if (type == null) { return new BadRequestResult(); }
            // Attach type Parameters
            List<DataPointTypeParameter> typeParameters = (await new DataPointTypeParameterDataAccess().GetDataPointTypeParameters(type.Id, WorldInfo)).ToList();
            type.TypeParameters = typeParameters;

            // Post initial data point
            int? id = await new DataPointDataAccess().PostDataPoint(data, WorldInfo);
            if (id == null)
            {
                return new BadRequestResult();
            }

            // Initialize parameter data access
            DataPointParameterDataAccess parameterDataAccess = new DataPointParameterDataAccess();

            // Post all parameters
            if (data.Parameters == null) { return new OkResult(); }
            List<Task> tasks = new List<Task>();
            foreach (DataPointParameter param in data.Parameters)
            {
                string? typeValue = type.TypeParameters.Where(p => p.Id == param.DataPointTypeParameterId).FirstOrDefault()?.TypeValue;
                if (typeValue != null)
                {
                    param.DataPointId = (int)id;
                    tasks.Add(parameterDataAccess.PostParameter(param, typeValue, WorldInfo));
                }
            }
            await Task.WhenAll(tasks);

            return new OkObjectResult(id);
        }

        [Authorize(Roles = "Administrator,Editor")]
        [HttpPut(Name = "PutDataPoint")]
        public async Task<IActionResult> PutDataPoint([FromBody] DataPoint data)
        {
            /*int result = await new DataPointTypeDataAccess().PutDataPointType(data);

            List<DataPointTypeParameter> currentParameters = (await new DataPointTypeParameterDataAccess().GetDataPointTypeParameters(data.Id)).ToList();
            List<DataPointTypeParameter> newParameters = data.TypeParameters ?? new List<DataPointTypeParameter>();

            DataPointTypeParameterComparer comparer = new DataPointTypeParameterComparer();

            List<DataPointTypeParameter> toEdit = newParameters.Intersect(currentParameters, comparer).ToList();
            List<DataPointTypeParameter> toCreate = newParameters.Except(toEdit, comparer).ToList();
            List<DataPointTypeParameter> toDelete = currentParameters.Except(toEdit, comparer).ToList();

            DataPointTypeParameterDataAccess parameterDataAccess = new DataPointTypeParameterDataAccess();

            List<Task> tasks = new List<Task>();

            foreach (DataPointTypeParameter parameter in toCreate)
            {
                tasks.Add(parameterDataAccess.PostDataPointTypeParameter(parameter));
            }
            foreach (DataPointTypeParameter parameter in toEdit)
            {
                tasks.Add(parameterDataAccess.PutDataPointTypeParameter(parameter));
            }
            foreach (DataPointTypeParameter parameter in toDelete)
            {
                tasks.Add(parameterDataAccess.DeleteDataPointTypeParameter(parameter));
            }

            await Task.WhenAll(tasks);

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
            }*/
            return null;
        }

        [Authorize(Roles = "Administrator,Editor")]
        [HttpDelete(Name = "DeleteDataPoint")]
        public async Task<IActionResult> DeleteDataPoint([FromQuery] int? Id)
        {
            if (Id == null) { return new BadRequestResult(); }

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
    }
}
