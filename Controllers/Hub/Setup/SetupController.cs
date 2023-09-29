using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SardCoreAPI.Areas.Identity.Data;

namespace SardCoreAPI.Controllers.Hub.Setup
{
    [ApiController]
    [Route("Library/[controller]/[action]")]
    public class SetupController : GenericController
    {
        private UserManager<SardCoreAPIUser> _userManager;

        public SetupController(UserManager<SardCoreAPIUser> userManager) { _userManager = userManager; }

        [HttpPost]
        public async Task<IActionResult> SetupUser()
        {
            SardCoreAPIUser user = await _userManager.FindByNameAsync("admin");
            if (user == null)
            {
                SardCoreAPIUser adminUser = new SardCoreAPIUser() { UserName = "admin", Email = "noemail" };
                var result = await _userManager.CreateAsync(adminUser, "admin");
                string[] roles = { "Viewer", "Editor", "Administrator" };
                await _userManager.AddToRolesAsync(adminUser, roles);
                return Ok();
            }

            return Ok("User already present");
        }
    }
}
