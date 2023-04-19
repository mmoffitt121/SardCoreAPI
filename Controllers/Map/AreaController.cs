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
