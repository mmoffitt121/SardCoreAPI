using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SardCoreAPI.Attributes.Security;
using SardCoreAPI.DataAccess.Units;
using SardCoreAPI.Models.Common;
using SardCoreAPI.Models.Units;
using SardCoreAPI.Services.Context;
using SardCoreAPI.Utility.DataAccess;

namespace SardCoreAPI.Controllers.Units
{
    [ApiController]
    [Route("Library/[controller]/[action]")]
    public class MeasurableController : GenericController
    {
        private readonly IDataService data;
        public MeasurableController(IDataService data) 
        { 
            this.data = data; 
        }

        [HttpGet]
        [Resource("Library.Setup.Units.Read")]
        public async Task<IActionResult> Get([FromQuery] PagedSearchCriteria criteria)
        {
            return await Handle(data.Context.Measurable
                .Where(criteria.GetQuery<Measurable>()
                    .AndIf(criteria.Id != null, x => x.Id.Equals(criteria.Id))
                    .AndIf(criteria.Query != null, x => x.Name.Contains(criteria.Query!))
                    .AndIf(true, x => true))
                .Paginate(criteria)
                .OrderBy(x => x.Name)
                .ToListAsync());
        }

        [HttpPost]
        [Resource("Library.Setup.Units")]
        public async Task<IActionResult> Post([FromBody] Measurable measurable)
        {
            data.Context.Measurable.Add(measurable);
            return await Handle(data.Context.SaveChangesAsync());
        }

        [HttpPut]
        [Resource("Library.Setup.Units")]
        public async Task<IActionResult> Put([FromBody] Measurable measurable)
        {
            data.Context.Measurable.Update(measurable);
            return await Handle(data.Context.SaveChangesAsync());
        }

        [HttpDelete]
        [Resource("Library.Setup.Units")]
        public async Task<IActionResult> Delete([FromQuery] int? Id)
        {
            return await Handle(data.Context.Measurable.Where(x => x.Id == Id).ExecuteDeleteAsync());
        }
    }
}
