using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SardCoreAPI.Attributes.Security;
using SardCoreAPI.Models.DataPoints;
using SardCoreAPI.Models.DataPoints.DataPointParameters;
using SardCoreAPI.Services.Context;
using SardCoreAPI.Services.DataPoints;
using SardCoreAPI.Utility.DataAccess;

namespace SardCoreAPI.Controllers.DataPoints
{
    [ApiController]
    [Route("Library/[controller]/[action]")]
    public class DataPointTypeController : GenericController
    {
        private readonly IDataService data;
        private readonly IDataPointTypeService _typeService;

        public DataPointTypeController(IDataService data, IDataPointTypeService typeService)
        {
            this.data = data;
            this._typeService = typeService;
        }

        [HttpGet]
        [Resource("Library.Document.Type.Read")]
        public async Task<IActionResult> GetDataPointTypes([FromQuery] DataPointTypeSearchCriteria criteria)
        {
            return await Handle(_typeService.GetDataPointTypes(criteria));
        }

        [HttpGet]
        [Resource("Library.Document.Type.Read")]
        public async Task<IActionResult> GetDataPointTypesFull([FromQuery] DataPointTypeSearchCriteria criteria)
        {
            return await Handle(data.Context.DataPointType
                .Where(criteria.GetQuery())
                .Sort(criteria)
                .Paginate(criteria)
                .Include(x => x.TypeParameters)
                .ToListAsync());
        }

        [HttpGet]
        [Resource("Library.Document.Type.Read")]
        public async Task<IActionResult> GetDataPointTypesCount([FromQuery] DataPointTypeSearchCriteria criteria)
        {
            return await Handle(data.Context.DataPointType
                .Where(criteria.GetQuery())
                .Sort(criteria)
                .CountAsync());
        }

        [HttpGet]
        [Resource("Library.Document.Type.Read")]
        public async Task<IActionResult> GetDataPointType([FromQuery] int? Id)
        {
            return await Handle(data.Context.DataPointType
                .Include(x => x.TypeParameters)
                .SingleAsync(x => x.Id.Equals(Id)));
        }

        [HttpPost]
        [Resource("Library.Setup.Types")]
        public async Task<IActionResult> PostDataPointType([FromBody] DataPointType type)
        {
            data.Context.DataPointType.Add(type);
            await Handle(data.Context.SaveChangesAsync());
            return Ok(type.Id);
        }

        [HttpPut]
        [Resource("Library.Setup.Types")]
        public async Task<IActionResult> PutDataPointType([FromBody] DataPointType type)
        {
            data.Context.DataPointType.Update(type);
            List<DataPointTypeParameter> toDelete = data.Context.DataPointTypeParameter.Where(x => x.DataPointTypeId == type.Id && !(type.TypeParameters ?? new()).Contains(x)).ToList();
            data.Context.DataPointTypeParameter.RemoveRange(toDelete);
            return await Handle(data.Context.SaveChangesAsync());
        }

        [HttpDelete]
        [Resource("Library.Setup.Types")]
        public async Task<IActionResult> DeleteDataPointType([FromQuery] int? Id)
        {
            DataPointType type = data.Context.DataPointType.Single(x => x.Id == Id);
            data.Context.DataPointType.Remove(type);
            List<DataPointTypeParameter> parameters = data.Context.DataPointTypeParameter.Where(x => x.DataPointTypeId.Equals(Id)).ToList();
            data.Context.DataPointTypeParameter.RemoveRange(parameters);
            return await Handle(data.Context.SaveChangesAsync());
        }
    }
}
