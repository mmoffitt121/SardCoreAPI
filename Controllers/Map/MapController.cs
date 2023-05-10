using Microsoft.AspNetCore.Mvc;
using SardCoreAPI.Models;
using SardCoreAPI.Models.Document.SearchCriteria;

namespace SardCoreAPI.Controllers.Map
{
    [ApiController]
    [Route("Library/[controller]/[action]")]
    public class MapController : ControllerBase
    {
        private readonly ILogger<MapController> _logger;

        public MapController(ILogger<MapController> logger)
        {
            _logger = logger;
        }
    }
}