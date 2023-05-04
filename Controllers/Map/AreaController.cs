using Microsoft.AspNetCore.Mvc;
using SardCoreAPI.DataAccess.Map;
using SardCoreAPI.Models.Map.Location;
using SardCoreAPI.Models.Map.Area;

namespace SardCoreAPI.Controllers.Map
{
    [ApiController]
    [Route("Library/[controller]/[action]")]
    public class AreaController
    {
        private readonly ILogger<AreaController> _logger;

        public AreaController(ILogger<AreaController> logger)
        {
            _logger = logger;
        }

        #region Area
        [HttpGet(Name = "GetAreas")]
        public IActionResult GetAreas([FromQuery] string? query)
        {
            if (query == null) { query = ""; }

            List<Area> result = new AreaDataAccess().GetAreas(query);
            if (result != null)
            {
                return new OkObjectResult(result);
            }
            return new BadRequestResult();
        }

        [HttpGet(Name = "GetArea")]
        public IActionResult GetArea([FromQuery] int? Id)
        {
            var result = new AreaDataAccess().GetArea(Id);
            if (result != null)
            {
                return new OkObjectResult(result);
            }
            return new NotFoundResult();
        }

        [HttpPost(Name = "PostArea")]
        public IActionResult PostArea([FromBody] Area area)
        {
            if (area == null) { return new BadRequestResult(); }

            if (new AreaDataAccess().PostArea(area))
            {
                return new OkResult();
            }

            return new BadRequestResult();
        }
        #endregion

        #region Subregion
        [HttpGet(Name = "GetSubregions")]
        public IActionResult GetSubregions([FromQuery] string? query)
        {
            if (query == null) { query = ""; }

            List<Subregion> result = new AreaDataAccess().GetSubregions(query);
            if (result != null)
            {
                return new OkObjectResult(result);
            }
            return new BadRequestResult();
        }

        [HttpGet(Name = "GetSubregion")]
        public IActionResult GetSubregion([FromQuery] int? Id)
        {
            var result = new AreaDataAccess().GetSubregion(Id);
            if (result != null)
            {
                return new OkObjectResult(result);
            }
            return new NotFoundResult();
        }

        [HttpPost(Name = "PostSubregion")]
        public IActionResult PostSubregion([FromBody] Subregion data)
        {
            if (data == null) { return new BadRequestResult(); }

            if (new AreaDataAccess().PostSubregion(data))
            {
                return new OkResult();
            }

            return new BadRequestResult();
        }
        #endregion

        #region Region
        [HttpGet(Name = "GetRegions")]
        public IActionResult GetRegions([FromQuery] string? query)
        {
            if (query == null) { query = ""; }

            List<Region> result = new AreaDataAccess().GetRegions(query);
            if (result != null)
            {
                return new OkObjectResult(result);
            }
            return new BadRequestResult();
        }

        [HttpGet(Name = "GetRegion")]
        public IActionResult GetRegion([FromQuery] int? Id)
        {
            var result = new AreaDataAccess().GetRegion(Id);
            if (result != null)
            {
                return new OkObjectResult(result);
            }
            return new NotFoundResult();
        }

        [HttpPost(Name = "PostRegion")]
        public IActionResult PostRegion([FromBody] Region data)
        {
            if (data == null) { return new BadRequestResult(); }

            if (new AreaDataAccess().PostRegion(data))
            {
                return new OkResult();
            }

            return new BadRequestResult();
        }
        #endregion

        #region Subcontinent
        [HttpGet(Name = "GetSubcontinents")]
        public IActionResult GetSubcontinents([FromQuery] string? query)
        {
            if (query == null) { query = ""; }

            List<Subcontinent> result = new AreaDataAccess().GetSubcontinents(query);
            if (result != null)
            {
                return new OkObjectResult(result);
            }
            return new BadRequestResult();
        }

        [HttpGet(Name = "GetSubcontinent")]
        public IActionResult GetSubcontinent([FromQuery] int? Id)
        {
            var result = new AreaDataAccess().GetSubcontinent(Id);
            if (result != null)
            {
                return new OkObjectResult(result);
            }
            return new NotFoundResult();
        }

        [HttpPost(Name = "PostSubcontinent")]
        public IActionResult PostSubcontinent([FromBody] Subcontinent data)
        {
            if (data == null) { return new BadRequestResult(); }

            if (new AreaDataAccess().PostSubcontinent(data))
            {
                return new OkResult();
            }

            return new BadRequestResult();
        }
        #endregion

        #region Continent
        [HttpGet(Name = "GetContinents")]
        public IActionResult GetContinents([FromQuery] string? query)
        {
            if (query == null) { query = ""; }

            List<Continent> result = new AreaDataAccess().GetContinents(query);
            if (result != null)
            {
                return new OkObjectResult(result);
            }
            return new BadRequestResult();
        }

        [HttpGet(Name = "GetContinent")]
        public IActionResult GetContinent([FromQuery] int? Id)
        {
            var result = new AreaDataAccess().GetContinent(Id);
            if (result != null)
            {
                return new OkObjectResult(result);
            }
            return new NotFoundResult();
        }

        [HttpPost(Name = "PostContinent")]
        public IActionResult PostContinent([FromBody] Continent data)
        {
            if (data == null) { return new BadRequestResult(); }

            if (new AreaDataAccess().PostContinent(data))
            {
                return new OkResult();
            }

            return new BadRequestResult();
        }
        #endregion

        #region Celestial Object
        [HttpGet(Name = "GetCelestialObjects")]
        public IActionResult GetCelestialObjects([FromQuery] string? query)
        {
            if (query == null) { query = ""; }

            List<CelestialObject> result = new AreaDataAccess().GetCelestialObjects(query);
            if (result != null)
            {
                return new OkObjectResult(result);
            }
            return new BadRequestResult();
        }

        [HttpGet(Name = "GetCelestialObject")]
        public IActionResult GetCelestialObject([FromQuery] int? Id)
        {
            var result = new AreaDataAccess().GetCelestialObject(Id);
            if (result != null)
            {
                return new OkObjectResult(result);
            }
            return new NotFoundResult();
        }

        [HttpPost(Name = "PostCelestialObject")]
        public IActionResult PostCelestialObject([FromBody] CelestialObject data)
        {
            if (data == null) { return new BadRequestResult(); }

            if (new AreaDataAccess().PostCelestialObject(data))
            {
                return new OkResult();
            }

            return new BadRequestResult();
        }
        #endregion

        #region Celestial System
        [HttpGet(Name = "GetCelestialSystems")]
        public IActionResult GetCelestialSystems([FromQuery] string? query)
        {
            if (query == null) { query = ""; }

            List<CelestialSystem> result = new AreaDataAccess().GetCelestialSystems(query);
            if (result != null)
            {
                return new OkObjectResult(result);
            }
            return new BadRequestResult();
        }

        [HttpGet(Name = "GetCelestialSystem")]
        public IActionResult GetCelestialSystem([FromQuery] int? Id)
        {
            var result = new AreaDataAccess().GetCelestialSystem(Id);
            if (result != null)
            {
                return new OkObjectResult(result);
            }
            return new NotFoundResult();
        }

        [HttpPost(Name = "PostCelestialSystem")]
        public IActionResult PostCelestialSystem([FromBody] CelestialSystem data)
        {
            if (data == null) { return new BadRequestResult(); }

            if (new AreaDataAccess().PostCelestialSystem(data))
            {
                return new OkResult();
            }

            return new BadRequestResult();
        }
        #endregion

        #region Manifold
        [HttpGet(Name = "GetManifolds")]
        public IActionResult GetManifolds([FromQuery] string? query)
        {
            if (query == null) { query = ""; }

            List<Manifold> result = new AreaDataAccess().GetManifolds(query);
            if (result != null)
            {
                return new OkObjectResult(result);
            }
            return new BadRequestResult();
        }

        [HttpGet(Name = "GetManifold")]
        public IActionResult GetManifold([FromQuery] int? Id)
        {
            var result = new AreaDataAccess().GetManifold(Id);
            if (result != null)
            {
                return new OkObjectResult(result);
            }
            return new NotFoundResult();
        }

        [HttpPost(Name = "PostManifold")]
        public IActionResult PostManifold([FromBody] Manifold data)
        {
            if (data == null) { return new BadRequestResult(); }

            if (new AreaDataAccess().PostManifold(data))
            {
                return new OkResult();
            }

            return new BadRequestResult();
        }
        #endregion
    }
}
