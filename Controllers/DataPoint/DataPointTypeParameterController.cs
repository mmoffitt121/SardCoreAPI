using Microsoft.AspNetCore.Mvc;
using SardCoreAPI.Controllers.Map;

namespace SardCoreAPI.Controllers.DataPoint
{
    [ApiController]
    [Route("Library/[controller]/[action]")]
    public class DataPointTypeParameterController
    {
        private readonly ILogger<MapController> _logger;

        public DataPointTypeParameterController(ILogger<MapController> logger)
        {
            _logger = logger;
        }
    }
}
