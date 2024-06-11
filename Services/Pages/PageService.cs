using SardCoreAPI.DataAccess.Easy;
using SardCoreAPI.Models.Pages.Pages;

namespace SardCoreAPI.Services.Pages
{
    public interface IPageService
    {
        public Task<Dictionary<ObjectType, List<ElementSetting>>> GetPageObjects();
        public Task<List<Page>> GetPages(PageSearchCriteria criteria);
        public Task<int> GetPageCount(PageSearchCriteria criteria);
        public Task PutPage(Page page);
        public Task DeletePage(int id);
    }
    public class PageService : IPageService
    {
        private IEasyDataAccess _dataAccess;

        public PageService(IEasyDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public async Task<Dictionary<ObjectType, List<ElementSetting>>> GetPageObjects()
        {
            return PageServiceConstants.PAGE_OBJECT_SETTINGS;
        }

        public async Task<List<Page>> GetPages(PageSearchCriteria criteria)
        {
            return await _dataAccess.Get<Page>(criteria);
        }

        public async Task<int> GetPageCount(PageSearchCriteria criteria)
        {
            return await _dataAccess.Count<Page>(criteria);
        }

        public async Task PutPage(Page page)
        {
            await _dataAccess.Put(page);
        }

        public async Task DeletePage(int id)
        {
            await _dataAccess.Delete<Page>(id);
        }
    }
}
