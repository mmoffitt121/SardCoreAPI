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
        public Task PutView(View view);
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
            criteria.StringId = criteria.StringId != null ? $"{ViewServiceConstants.VIEW_SETTING}.{criteria.StringId}" : $"{ViewServiceConstants.VIEW_SETTING}.%";
            List <SettingJSON> settings = await _dataAccess.Get<SettingJSON>(new { id = criteria.StringId});

            if (settings == null || settings.Count() == 0)
            {
                return new List<View> { };
            }

            List<View> views = new List<View>();
            settings.ForEach(setting =>
            {
                View? view = JsonConvert.DeserializeObject<View>(setting.Setting);
                if (view != null) views.Add(view);
            });

            return views;
        }

        public async Task PutView(View view)
        {
            if (view.Id == null)
            {
                view.Id = Guid.NewGuid().ToString();
            }
            SettingJSON setting = new SettingJSON($"{ViewServiceConstants.VIEW_SETTING}.{view.Id}", JsonConvert.SerializeObject(view));
            await _dataAccess.Put(setting, insert: true);
        }
    }
}
