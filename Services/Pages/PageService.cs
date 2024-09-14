using Microsoft.EntityFrameworkCore;
using SardCoreAPI.DataAccess.Easy;
using SardCoreAPI.Models.Common;
using SardCoreAPI.Models.Pages.Pages;
using SardCoreAPI.Services.Context;
using SardCoreAPI.Utility.DataAccess;

namespace SardCoreAPI.Services.Pages
{
    public interface IPageService
    {
        public Task<Dictionary<ObjectType, List<ElementSetting>>> GetPageObjects();
        public Task<List<Page>> GetPages(PageSearchCriteria criteria);
        public Task<int> GetPageCount(PageSearchCriteria criteria);
        public Task<StringIDResponse> PutPage(Page page);
        public Task DeletePage(string id);
    }

    public class PageService : IPageService
    {
        private IDataService data;

        public PageService(IDataService data)
        {
            this.data = data;
        }

        public async Task<Dictionary<ObjectType, List<ElementSetting>>> GetPageObjects()
        {
            return PageServiceConstants.PAGE_OBJECT_SETTINGS;
        }

        public async Task<List<Page>> GetPages(PageSearchCriteria criteria)
        {
            return await data.Context.Page.Where(criteria.GetQuery()).OrderBy(p => p.Name).Paginate(criteria).ToListAsync();
        }

        public async Task<int> GetPageCount(PageSearchCriteria criteria)
        {
            return await data.Context.Page.Where(criteria.GetQuery()).CountAsync();
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
