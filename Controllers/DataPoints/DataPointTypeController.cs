using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using SardCoreAPI.Controllers.Map;
using SardCoreAPI.DataAccess.DataPoints;
using SardCoreAPI.DataAccess.Map;
using SardCoreAPI.Models.Common;
using SardCoreAPI.Models.DataPoints;
using SardCoreAPI.Models.DataPoints.DataPointParameters;
using SardCoreAPI.Models.Map.Location;
using SardCoreAPI.Utility.Error;

namespace SardCoreAPI.Controllers.DataPoints
{
    [ApiController]
    [Route("Library/[controller]/[action]")]
    public class DataPointTypeController : GenericController
    {
        private readonly ILogger<DataPointTypeController> _logger;

        public DataPointTypeController(ILogger<DataPointTypeController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetDataPointTypes")]
        public async Task<IActionResult> GetDataPointTypes([FromQuery] PagedSearchCriteria criteria)
        {
            if (criteria == null) { return new BadRequestResult(); }

            List<DataPointType> result = await new DataPointTypeDataAccess().GetDataPointTypes(criteria, WorldInfo);
            if (result != null)
            {
                return new OkObjectResult(result);
            }
            return new BadRequestResult();
        }

        [HttpGet(Name = "GetDataPointType")]
        public async Task<IActionResult> GetDataPointType([FromQuery] int? Id)
        {
            if (Id == null) { return new BadRequestResult(); }

            // Get Data Point Type
            DataPointType? result = (await new DataPointTypeDataAccess().GetDataPointTypes(new PagedSearchCriteria() { Id = Id }, WorldInfo)).FirstOrDefault();

            if (result == null) { return new NotFoundResult(); }

            // Attach Parameters
            List<DataPointTypeParameter> parameters = (await new DataPointTypeParameterDataAccess().GetDataPointTypeParameters(result.Id, WorldInfo)).ToList();
            result.TypeParameters = parameters;

            return new OkObjectResult(result);
        }

        [Authorize(Roles = "Administrator,Editor")]
        [HttpPost(Name = "PostDataPointType")]
        public async Task<IActionResult> PostDataPointType([FromBody] DataPointType data)
        {
            if (data == null) { return new BadRequestResult(); }

            int result = await new DataPointTypeDataAccess().PostDataPointType(data, WorldInfo);

            if (result != 0)
            {
                return new OkObjectResult(result);
            }

            return new BadRequestResult();
        }

        [Authorize(Roles = "Administrator,Editor")]
        [HttpPut(Name = "PutDataPointType")]
        public async Task<IActionResult> PutDataPointType([FromBody] DataPointType data)
        {
            int result = await new DataPointTypeDataAccess().PutDataPointType(data, WorldInfo);

            List<DataPointTypeParameter> currentParameters = (await new DataPointTypeParameterDataAccess().GetDataPointTypeParameters(data.Id, WorldInfo)).ToList();
            List<DataPointTypeParameter> newParameters = data.TypeParameters ?? new List<DataPointTypeParameter>();

            DataPointTypeParameterComparer comparer = new DataPointTypeParameterComparer();

            List<DataPointTypeParameter> toEdit = newParameters.Intersect(currentParameters, comparer).ToList();
            List<DataPointTypeParameter> toCreate = newParameters.Except(toEdit, comparer).ToList();
            List<DataPointTypeParameter> toDelete = currentParameters.Except(toEdit, comparer).ToList();

            DataPointTypeParameterDataAccess parameterDataAccess = new DataPointTypeParameterDataAccess();

            List<Task> tasks = new List<Task>();

            foreach (DataPointTypeParameter parameter in toCreate)
            {
                tasks.Add(parameterDataAccess.PostDataPointTypeParameter(parameter, WorldInfo));
            }
            foreach (DataPointTypeParameter parameter in toEdit)
            {
                tasks.Add(parameterDataAccess.PutDataPointTypeParameter(parameter, WorldInfo));
            }
            foreach (DataPointTypeParameter parameter in toDelete)
            {
                tasks.Add(parameterDataAccess.DeleteDataPointTypeParameter(parameter, WorldInfo));
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
            }
        }

        [Authorize(Roles = "Administrator,Editor")]
        [HttpDelete(Name = "DeleteDataPointType")]
        public async Task<IActionResult> DeleteDataPointType([FromQuery] int? Id)
        {
            if (Id == null) { return new BadRequestResult(); }

            try
            {
                await new DataPointTypeParameterDataAccess().DeleteDataPointTypeParametersOfDataType((int)Id, WorldInfo);

                int result = await new DataPointTypeDataAccess().DeleteDataPointType((int)Id, WorldInfo);

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
            catch (MySqlException ex)
            {
                return ex.Handle();
            }
            catch (Exception ex)
            {
                return ex.Handle();
            }
        }
    }
}
