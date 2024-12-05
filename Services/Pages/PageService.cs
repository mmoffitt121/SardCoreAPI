using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SardCoreAPI.DataAccess.Easy;
using SardCoreAPI.Models.Common;
using SardCoreAPI.Models.Pages.Pages;
using SardCoreAPI.Models.Settings;
using SardCoreAPI.Services.Context;
using SardCoreAPI.Services.Setting;
using SardCoreAPI.Utility.DataAccess;
using System.Collections.Generic;

namespace SardCoreAPI.Services.Pages
{
    public interface IPageService
    {
        public Task<Dictionary<ObjectType, List<ElementSetting>>> GetPageObjects();
        public Task<List<Page>> GetPages(PageSearchCriteria criteria);
        public Task<int> GetPageCount(PageSearchCriteria criteria);
        public Task<string> GetDefaultPagePath();
        public Task<List<string>> GetPossiblePaths();
        public Task<StringIDResponse> PutPage(Page page);
        public Task DeletePage(string id);
    }

    public class PageService : IPageService
    {
        private IDataService data;
        private IViewService viewService;
        private ISettingJSONService settingJSONService;

        private static readonly string PAGE_PATH_PREFIX = "page/";

        public PageService(IDataService data, IViewService viewService, ISettingJSONService settingJSONService)
        {
            this.data = data;
            this.viewService = viewService;
            this.settingJSONService = settingJSONService;
        }

        public async Task<Dictionary<ObjectType, List<ElementSetting>>> GetPageObjects()
        {
            Dictionary<ObjectType, List<ElementSetting>> dict = PageServiceConstants.GetPageObjectSettings();

            dict.Add(ObjectType.View, new List<ElementSetting>()
                {
                    new ElementSetting(ElementSettingType.String, "Element Name", "View", false),
                    new ElementSetting(ElementSettingType.View, "View", "View", true, (await viewService.GetViewIds()).ToArray()),
                }
            );

            return dict;
        }

        public async Task<List<Page>> GetPages(PageSearchCriteria criteria)
        {
            return await data.Context.Page.Where(criteria.GetQuery()).OrderBy(p => p.Name).Paginate(criteria).ToListAsync();
        }

        public async Task<int> GetPageCount(PageSearchCriteria criteria)
        {
            return await data.Context.Page.Where(criteria.GetQuery()).CountAsync();
        }

        public async Task<string> GetDefaultPagePath()
        {
            List<SettingJSON> settings = await settingJSONService.Get("libratlas.pages.default");
            if (settings.Count() < 1)
            {
                return "\"document\"";
            } 
            else
            {
                string setting = JsonConvert.DeserializeObject<string>(settings.First().Setting) ?? "";
                List<Page> pages = await GetPages(new PageSearchCriteria() { Ids = new List<string>() { setting } });
                if (pages.Count() > 0)
                {
                    return $"\"page/{pages.First().Path}\"";
                }
                return "\"document\"";
            }
        }

        public async Task<List<string>> GetPossiblePaths()
        {
            List<string> paths = new List<string>();
            paths.Add("document");
            paths.Add("map");
            paths.AddRange(data.Context.Page.Select(p => PAGE_PATH_PREFIX + p.Path).ToList());

            return paths;
        }

        public async Task<StringIDResponse> PutPage(Page page)
        {
            if (page.Id == null)
            {
                page.Id = Guid.NewGuid().ToString();
                page.Root = new PageElement(ObjectType.Root);
            }
            data.Context.Page.Put(page, p => p.Id == page.Id);
            await data.Context.SaveChangesAsync();

            return new StringIDResponse(page.Id);
        }

        public async Task DeletePage(string id)
        {
            await data.Context.Page.Where(p => p.Id.Equals(id)).ExecuteDeleteAsync();
        }
    }
}
