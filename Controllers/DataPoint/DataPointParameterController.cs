using Microsoft.AspNetCore.Mvc;
using SardCoreAPI.Controllers.Map;

namespace SardCoreAPI.Controllers.DataPoint
{
    [ApiController]
    [Route("Library/[controller]/[action]")]
    public class DataPointParameterController
    {
        private readonly ILogger<MapController> _logger;

        public DataPointParameterController(ILogger<MapController> logger)
        {
            _logger = logger;
        }
    }
}
