using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SardCoreAPI.Services.Database;

namespace SardCoreAPI.Controllers.Administration.Database
{
    [ApiController]
    [Route("Library/[controller]/[action]")]
    public class DatabaseController : Controller
    { 
        private IDatabaseService databaseDataAccess;

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public async Task<IActionResult> GetServerVersion()
        {
            return Ok(new { Version = await databaseDataAccess.GetServerVersion() });
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public async Task<IActionResult> GetDatabases()
        {
            return Ok(await databaseDataAccess.GetDatabases());
        }

        public DatabaseController(IDatabaseService databaseDataAccess)
        {
            this.databaseDataAccess = databaseDataAccess;
        }
    }
}
