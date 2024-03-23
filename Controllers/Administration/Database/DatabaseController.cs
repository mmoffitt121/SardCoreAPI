using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SardCoreAPI.DataAccess.Administration.Database;
using SardCoreAPI.DataAccess.Content;
using SardCoreAPI.Models.Content;

namespace SardCoreAPI.Controllers.Administration.Database
{
    [ApiController]
    [Route("Library/[controller]/[action]")]
    public class DatabaseController : Controller
    { 
        private IDatabaseDataAccess databaseDataAccess;

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

        public DatabaseController(IDatabaseDataAccess databaseDataAccess)
        {
            this.databaseDataAccess = databaseDataAccess;
        }
    }
}
