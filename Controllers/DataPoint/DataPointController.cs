using Microsoft.AspNetCore.Mvc;

namespace SardCoreAPI.Controllers.DataPoint
{
    public class DataPointController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
