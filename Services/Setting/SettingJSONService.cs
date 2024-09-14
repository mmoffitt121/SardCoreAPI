using Microsoft.EntityFrameworkCore;
using SardCoreAPI.DataAccess.Easy;
using SardCoreAPI.Models.Settings;
using SardCoreAPI.Services.Context;

namespace SardCoreAPI.Services.Setting
{
    public interface ISettingJSONService
    {
        public Task<List<SettingJSON>> Get(string Id);
        public Task Put(SettingJSON setting);
        public Task Delete(string Id);
    }

    public class SettingJSONService : ISettingJSONService
    {
        private readonly IDataService _dataService;
        public SettingJSONService(IDataService dataService) 
        {
            _dataService = dataService;
        }

        public async Task<List<SettingJSON>> Get(string Id)
        {
            if (Id.EndsWith('%'))
            {
                return _dataService.Context.SettingJSON.Where(sj => sj.Id.StartsWith(Id.Replace("%", ""))).ToList();
            }
            else
            {
                return _dataService.Context.SettingJSON.Where(sj => sj.Id.Equals(Id)).ToList();
            }
            
        }

        public async Task Put(SettingJSON setting)
        {
            if (!_dataService.Context.SettingJSON.Any(sj => sj.Id.Equals(setting.Id)))
            {
                _dataService.Context.SettingJSON.Add(setting);
            }
            else
            {
                _dataService.Context.SettingJSON.Update(setting);
            }

            _dataService.Context.SaveChanges();
        }

        public async Task Delete(string Id)
        {
            await _dataService.Context.SettingJSON.Where(sj => sj.Id.Equals(Id)).ExecuteDeleteAsync();
        }
    }
}
