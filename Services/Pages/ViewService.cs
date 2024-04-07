using SardCoreAPI.DataAccess.Easy;
using SardCoreAPI.Models.Common;
using SardCoreAPI.Models.Pages;
using SardCoreAPI.Models.Settings;
using Newtonsoft.Json;

namespace SardCoreAPI.Services.Pages
{
    public interface IViewService
    {
        public Task<List<View>> GetViews(List<string>? ids);
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

        public async Task<List<View>> GetViews(List<string>? ids)
        {
            List<SettingJSON> settings = null;
            if (ids == null || ids.Count == 0)
            {
                settings = await _dataAccess.Get<SettingJSON>(new {id = $"{ViewServiceConstants.VIEW_SETTING}.%"});
            }
            else
            {
                ids = ids.Select(id => $"{ViewServiceConstants.VIEW_SETTING}.{id}").ToList();
                settings = await _dataAccess.Get<SettingJSON>(new { id = ids });
            }

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

            views = views.OrderBy(view => view.Name).ToList();

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

        public async Task DeleteView(string id)
        {
            await _dataAccess.Delete<SettingJSON>(new { id = $"{ViewServiceConstants.VIEW_SETTING}.{id}" });
        }
    }
}
