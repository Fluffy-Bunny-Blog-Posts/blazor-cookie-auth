namespace BlazorApp1.Services
{
    public interface IClientMemoryCache
    {
        T GetValue<T>(string key) where T : class;
        void SetValue<T>(string key, T value) where T : class;
        void Clear();
    }
}
