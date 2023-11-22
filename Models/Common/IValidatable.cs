namespace SardCoreAPI.Models.Common
{
    public interface IValidatable
    {
        public abstract List<string> Validate();
    }
}
