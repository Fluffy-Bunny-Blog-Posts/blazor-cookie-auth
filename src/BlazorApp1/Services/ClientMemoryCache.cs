using System.Collections.Concurrent;

namespace BlazorApp1.Services
{
    public class ClientMemoryCache : IClientMemoryCache
    {
        ConcurrentDictionary<string, object> _store;
        public ClientMemoryCache()
        {
            _store = new ConcurrentDictionary<string, object>();
        }

        public void Clear()
        {
            _store.Clear();
        }

        public T GetValue<T>(string key) where T: class
        {
            object value;
            if(_store.TryGetValue(key,out value))
            {
                return value as T;
            }
            return null;
        }

        public void SetValue<T>(string key, T value) where T : class
        {
            _store[key] = value;
        }
    }
}
