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
    public class UnitController : GenericController
    {
        private readonly IDataService data;
        public UnitController(IDataService data)
        {
            this.data = data;
        }

        [HttpGet]
        [Resource("Library.Setup.Units.Read")]
        public async Task<IActionResult> GetTables([FromQuery] UnitSearchCriteria criteria)
        {
            return await Handle(data.Context.Unit
                .Where(criteria.GetQuery())
                .OrderBy(u => u.Name)
                .Paginate(criteria)
                .Include(u => u.Measurable)
                .GroupBy(u => u.Measurable)
                .Select(group => new UnitTable()
                {
                    Measurable = group.Key!,
                    Units = group.ToArray()
                })
                .OrderBy(tab => tab.Measurable.Name)
                .ToListAsync());
        }

        [HttpGet]
        [Resource("Library.Setup.Units.Read")]
        public async Task<IActionResult> Get([FromQuery] UnitSearchCriteria criteria)
        {
            return await Handle(data.Context.Unit
                .Where(criteria.GetQuery())
                .OrderBy(u => u.Name)
                .Paginate(criteria)
                .ToListAsync());
        }

        [HttpPost]
        [Resource("Library.Setup.Units")]
        public async Task<IActionResult> Post([FromBody] Unit unit)
        {
            data.Context.Unit.Add(unit);
            return await Handle(data.Context.SaveChangesAsync());
        }

        [HttpPut]
        [Resource("Library.Setup.Units")]
        public async Task<IActionResult> Put([FromBody] Unit unit)
        {
            data.Context.Unit.Update(unit);
            return await Handle(data.Context.SaveChangesAsync());
        }

        [HttpDelete]
        [Resource("Library.Setup.Units")]
        public async Task<IActionResult> Delete([FromQuery] int? Id)
        {
            return await Handle(data.Context.Unit.Where(x => x.Id == Id).ExecuteDeleteAsync());
        }
    }
}
