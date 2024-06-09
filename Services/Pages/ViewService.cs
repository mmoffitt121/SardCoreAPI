using SardCoreAPI.DataAccess.Easy;
using SardCoreAPI.Models.Common;
using SardCoreAPI.Models.Settings;
using Newtonsoft.Json;
using SardCoreAPI.Models.Pages.Views;

namespace SardCoreAPI.Services.Pages
{
    public interface IViewService
    {
        public Task<int> GetViewCount(ViewSearchCriteria criteria); 
        public Task<List<View>> GetViews(ViewSearchCriteria criteria);
        public Task PutView(View view);
        public Task DeleteView(string id);
    }

    public class ViewService : IViewService
    {
        private IEasyDataAccess _dataAccess;

        public ViewService(IEasyDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public async Task<int> GetViewCount(ViewSearchCriteria criteria)
        {
            int count = 0;
            if (criteria.Ids == null || criteria.Ids.Count == 0)
            {
                count = await _dataAccess.Count<ViewWrapper>(new { id = $"{ViewServiceConstants.VIEW_SETTING}.%" }, queryOptions: criteria);
            }
            else
            {
                criteria.Ids = criteria.Ids.Select(id => $"{ViewServiceConstants.VIEW_SETTING}.{id}").ToList();
                count = await _dataAccess.Count<ViewWrapper>(new { id = criteria.Ids }, queryOptions: criteria);
            }

            return count;
        }

        public async Task<List<View>> GetViews(ViewSearchCriteria criteria)
        {
            List<ViewWrapper> wrappers = null;
            if (criteria.Ids == null || criteria.Ids.Count == 0)
            {
                wrappers = await _dataAccess.Get<ViewWrapper>(new {id = $"{ViewServiceConstants.VIEW_SETTING}.%"}, queryOptions: criteria);
            }
            else
            {
                criteria.Ids = criteria.Ids.Select(id => $"{ViewServiceConstants.VIEW_SETTING}.{id}").ToList();
                wrappers = await _dataAccess.Get<ViewWrapper>(new { id = criteria.Ids }, queryOptions: criteria);
            }

            if (wrappers == null || wrappers.Count() == 0)
            {
                return new List<View> { };
            }

            List<View> views = new List<View>();
            wrappers.ForEach(wrapper =>
            {
                View? view = JsonConvert.DeserializeObject<View>(wrapper.View);
                if (view != null) views.Add(view);
            });

            views = views.OrderBy(view => view.Name).ToList();

            return views;
        }

        public async Task PutView(View view)
        {
            if (view.Id == null)
            {
                view.Id = Guid.NewGuid().ToString();
            }
            ViewWrapper setting = new ViewWrapper($"{ViewServiceConstants.VIEW_SETTING}.{view.Id}", view.Name, JsonConvert.SerializeObject(view));
            await _dataAccess.Put(setting, insert: true);
        }

        public async Task DeleteView(string id)
        {
            await _dataAccess.Delete<ViewWrapper>(new { id = $"{ViewServiceConstants.VIEW_SETTING}.{id}" });
        }
    }
}
