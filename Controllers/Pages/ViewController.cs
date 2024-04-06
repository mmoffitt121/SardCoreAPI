﻿using Microsoft.AspNetCore.Mvc;
using SardCoreAPI.Attributes.Security;
using SardCoreAPI.Models.Common;
using SardCoreAPI.Models.MenuItems;
using SardCoreAPI.Models.Pages;
using SardCoreAPI.Services.MenuItems;
using SardCoreAPI.Services.Pages;
using SardCoreAPI.Utility.Error;

namespace SardCoreAPI.Controllers.Pages
{
    [ApiController]
    [Route("Library/[controller]/[action]")]
    public class ViewController : Controller
    {
        private IViewService _viewService;

        public ViewController(IViewService viewService)
        {
            _viewService = viewService;
        }

        [HttpPost]
        [Resource("Library.Setup.Pages.Read")]
        public async Task<IActionResult> Get(PagedSearchCriteria criteria)
        {
            try
            {
                return new OkObjectResult(await _viewService.GetViews(criteria));
            }
            catch (Exception ex)
            {
                return ex.Handle();
            }
        }

        [HttpPut]
        [Resource("Library.Setup.Pages")]
        public async Task<IActionResult> Put([FromBody] View data)
        {
            try
            {
                await _viewService.PutView(data);
                return Ok();
            }
            catch (Exception ex)
            {
                return ex.Handle();
            }
        }
    }
}
