﻿using Microsoft.AspNetCore.Mvc;
using SardCoreAPI.Attributes.Security;
using SardCoreAPI.Models.Pages.Pages;
using SardCoreAPI.Services.Pages;

namespace SardCoreAPI.Controllers.Pages
{
    [ApiController]
    [Route("Library/[controller]/[action]")]
    public class PageController : GenericController
    {
        private IPageService _pageService;
        public PageController(IPageService pageService)
        {
            _pageService = pageService;
        }

        [HttpGet]
        [Resource("Library.Setup.Pages.Read")]
        public async Task<IActionResult> GetPageObjects()
        {
            return await Handle(_pageService.GetPageObjects());
        }

        [HttpGet]
        [Resource("Library.Setup.Pages.Read")]
        public async Task<IActionResult> GetPages(PageSearchCriteria criteria)
        {
            return await Handle(_pageService.GetPages(criteria));
        }

        [HttpGet]
        [Resource("Library.Setup.Pages.Read")]
        public async Task<IActionResult> GetPageCount(PageSearchCriteria criteria)
        {
            return await Handle(_pageService.GetPages(criteria));
        }

        [HttpPut]
        [Resource("Library.Setup.Pages")]
        public async Task<IActionResult> PutPage(Page page)
        {
            return await Handle(_pageService.PutPage(page));
        }

        [HttpDelete]
        [Resource("Library.Setup.Pages")]
        public async Task<IActionResult> DeletePage(int id)
        {
            return await Handle(_pageService.DeletePage(id));
        }

    }
}
