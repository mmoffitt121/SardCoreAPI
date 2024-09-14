using SardCoreAPI.DataAccess.Easy;
using SardCoreAPI.Models.Common;
using SardCoreAPI.Models.Settings;
using Newtonsoft.Json;
using SardCoreAPI.Models.Pages.Views;
using SardCoreAPI.Services.Context;
using SardCoreAPI.Utility.DataAccess;
using Microsoft.EntityFrameworkCore;

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
        private IDataService data;

        public ViewService(IDataService data)
        {
            this.data = data;
        }

        public async Task<int> GetViewCount(ViewSearchCriteria criteria)
        {
            int count = 0;
            if (criteria.Ids == null || criteria.Ids.Count == 0)
            {
                count = await data.Context.View.CountAsync();
            }
            else
            {
                count = await data.Context.View.Where(v => criteria.Ids.Contains(v.Id)).CountAsync();
            }

            return count;
        }

        public async Task<List<View>> GetViews(ViewSearchCriteria criteria)
        {
            List<ViewWrapper> wrappers = null;
            if (criteria.Ids == null || criteria.Ids.Count == 0)
            {
                wrappers = await data.Context.View.Paginate(criteria).ToListAsync();
            }
            else
            {
                wrappers = await data.Context.View.Where(v => criteria.Ids.Contains(v.Id)).Paginate(criteria).ToListAsync();
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

            return views;
        }

        public async Task PutView(View view)
        {
            if (view.Id == null)
            {
                view.Id = Guid.NewGuid().ToString();
            }
            ViewWrapper? setting = data.Context.View.SingleOrDefault(v => v.Id.Equals(view.Id));

            if (setting != null)
            {
                setting.Name = view.Name;
                setting.View = JsonConvert.SerializeObject(view);
                data.Context.View.Update(setting);
            }
            else
            {
                setting = new ViewWrapper(view.Id, view.Name, JsonConvert.SerializeObject(view));
                data.Context.View.Add(setting);
            }

            data.Context.SaveChanges();
        }

        public async Task DeleteView(string id)
        {
            await data.Context.View.Where(v => v.Id.Equals(id)).ExecuteDeleteAsync();
        }
    }
}
