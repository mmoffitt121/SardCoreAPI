using SardCoreAPI.DataAccess.Easy;
using SardCoreAPI.Models.Common;
using SardCoreAPI.Models.Pages;
using SardCoreAPI.Models.Settings;
using Newtonsoft.Json;

namespace SardCoreAPI.Services.Pages
{
    public interface IViewService
    {
        public Task<List<View>> GetViews(PagedSearchCriteria criteria);
    }

    public class ViewService : IViewService
    {
        private IEasyDataAccess _dataAccess;

        public ViewService(IEasyDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public async Task<List<View>> GetViews(PagedSearchCriteria criteria)
        {
            List<SettingJSON> settings = await _dataAccess.Get<SettingJSON>(criteria);

            if (settings == null)
            {
                return new List<View> { new View() };
            }

            List<View> views = new List<View>();
            settings.ForEach(setting =>
            {
                View? view = JsonConvert.DeserializeObject<View>(setting.Setting);
                if (view != null) views.Add(view);
            });

            return views;
        }
    }
}
