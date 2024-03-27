using SardCoreAPI.DataAccess.Easy;
using SardCoreAPI.Models.Settings;

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
        private readonly IEasyDataAccess _dataAccess;
        public SettingJSONService(IEasyDataAccess dataAccess) 
        {
            _dataAccess = dataAccess;
        }

        public async Task<List<SettingJSON>> Get(string Id)
        {
            return await _dataAccess.Get<SettingJSON>(new {Id});
        }

        public async Task Put(SettingJSON setting)
        {
            await _dataAccess.Put(setting, insert: true);
        }

        public async Task Delete(string Id)
        {
            await _dataAccess.Delete<SettingJSON>(new {Id});
        }
    }
}
